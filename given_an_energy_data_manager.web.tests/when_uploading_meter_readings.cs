using EnergyDataManager.Domain;
using EnergyDataManager.Domain.Interfaces;
using EnergyDataManager.Domain.ValueObjects;
using EnergyDataManager.Web.Controllers;
using EnergyDataReader;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Threading;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using EnergyDataManager.SharedKernel;
using EnergyDataManager.SharedKernel.Interfaces;
using EnergyDataManager.SharedKernel.interfaces;
using Moq.Protected;
using SharedKernel.Interfaces;
using EnergyDataReader.File;
using EnergyDataReader.File.Interfaces;
using EnergyDataManager.Web.DomainServices;

namespace given_an_energy_data_manager.web.tests
{
    public class when_uploading_meter_readings
    {
        private readonly Mock<IFileReadService<SourceMeterReading, string>> _FileReadServiceMOCk;
        private readonly Mock<IConfiguration> _ConfigurationMOCK;
        private readonly Mock<IConfigurationSection> _ConfigurationSectionMOCK;
        private readonly IEnumerable<SourceMeterReading> _SourceMeterReading;
        public when_uploading_meter_readings()
        {
            _FileReadServiceMOCk = new Mock<IFileReadService<SourceMeterReading, string>>();
            _ConfigurationMOCK = new Mock<IConfiguration>();
            _ConfigurationSectionMOCK = new Mock<IConfigurationSection>();
            _ConfigurationMOCK.Setup(fn =>
                fn.GetSection(It.IsAny<string>()))
                .Returns(_ConfigurationSectionMOCK.Object);


            _SourceMeterReading = new List<SourceMeterReading>(){
                new SourceMeterReading(
                    "2344", "22/04/2019  09:24:00", "1002"),
                new SourceMeterReading(
                    "2344","24/04/2019  09:26:00", "2002"),
                new SourceMeterReading(
                    "2355", "07/05/2019  09:24:00", "8888"),
                new SourceMeterReading(
                    "1248","18/05/2019  09:24:00", "1111")
             };
        }

        public static IEnumerable<object[]> MeterReadingTestData =>
            new List<object[]>
            {
                new object[] {
                    new List<List<SourceMeterReading>>(){
                        new List<SourceMeterReading>(){
                                new SourceMeterReading(
                                    "2344", "22/04/2019  09:24:00", "1002"),
                                new SourceMeterReading(
                                    "2344","24/04/2019  09:26:00", "2002")
                        }
                    },
                    new  Account(
                        2344,
                        new List<Reading>(){
                            Reading.Create("22/04/2019  09:24:00", "1002" ),

                        })
                }
            };

        [Theory]
        [MemberData(nameof(MeterReadingTestData))]
        public void then_return_count_of_updated_meter_readings(
            List<List<SourceMeterReading>> sourceMeterReadingsEXP,
            Account accountEXP)
        {
            //ARRANGE
            var totalReadingsEXP = sourceMeterReadingsEXP.ElementAt(0).Count;
            Account accountACT = null;
            Mock<IEnergyDataManagerRepo> edmRepoMOCK =
                new Mock<IEnergyDataManagerRepo>();

            Mock<IConverterList<Account, SourceMeterReading>> sourceMeterReadingsToAccountConverterMOCK =
                new Mock<IConverterList<Account, SourceMeterReading>>();

            Mock<IExtractMeterReadings> extractMeterReadingsMOCK =
                new Mock<IExtractMeterReadings>();

            Mock<MeterReadingFile> meterReadingFileMOCK =
                new Mock<MeterReadingFile>(
                    _ConfigurationMOCK.Object,
                    It.IsAny<IConverter<SourceMeterReading, string>>(),
                    _FileReadServiceMOCk.Object);

            sourceMeterReadingsToAccountConverterMOCK.Setup(fn =>
                fn.Convert(It.IsAny<IEnumerable<SourceMeterReading>>()))
                    .Returns(accountEXP);

            edmRepoMOCK.Setup(fn =>
               fn.UpdateAccountWithMeterReadingsAsync(
                   It.IsAny<Account>()))
                    .Callback<Account>(
                        account => accountACT = account
                    )
                   .Returns(Task.FromResult(totalReadingsEXP));

            meterReadingFileMOCK.Setup(fn => fn.RetrieveReadingsByAccountId())
                .Returns(sourceMeterReadingsEXP);

            meterReadingFileMOCK.Setup(fn => fn.Rows)
                .Returns(totalReadingsEXP);

            var sut = new MeterReadingUploadsController(
               sourceMeterReadingsToAccountConverterMOCK.Object,
               edmRepoMOCK.Object,
                meterReadingFileMOCK.Object);


            //ACT
            var responseACT = sut.MeterReadingAsync().Result;

            //ASSERT
            accountACT.Should().Be(accountEXP);
            meterReadingFileMOCK.Verify(fn =>
                fn.RetrieveReadingsByAccountId(), Times.Once); //to accommodate null read eof
            meterReadingFileMOCK.Verify(fn => fn.Rows, Times.Once());

            sourceMeterReadingsToAccountConverterMOCK.Verify(fn =>
                fn.Convert(It.IsAny<List<SourceMeterReading>>()), Times.Once);

            edmRepoMOCK.Verify(fn => fn.UpdateAccountWithMeterReadingsAsync(
                It.IsAny<Account>()), Times.Once);


            var responseObject = responseACT as ObjectResult;
            responseObject.StatusCode.Should().Be((int)HttpStatusCode.Created);
            responseObject.Value.Should()
                .Be($"{totalReadingsEXP}/{totalReadingsEXP} meter readings were uploaded");
        }

        [Fact]
        public void then_load_meter_reading_file_on_initialisation()
        {

            //ARRANGE
            var fileNameEXP = "Meter_Reading.csv";
            string fileNameACT = null;

            _ConfigurationSectionMOCK.Setup(a => a.Value)
                .Returns(fileNameEXP);

            _FileReadServiceMOCk.Setup(fn => fn.Read(
                It.IsAny<string>(),
                It.IsAny<IConverter<SourceMeterReading, string>>()))
                    .Callback<string, IConverter<SourceMeterReading, string>>(
                        (fileName, converter) =>
                        {
                            fileNameACT = fileNameEXP;
                        })
                    .Returns(_SourceMeterReading);


            //ACT
            var sut = new MeterReadingFile(
                _ConfigurationMOCK.Object,
                It.IsAny<IConverter<SourceMeterReading, string>>(),
                _FileReadServiceMOCk.Object);


            //ASSERT
            _ConfigurationMOCK.Verify(fn =>
                      fn.GetSection(It.IsAny<string>()), Times.Once);

            fileNameACT.Should().Be(fileNameEXP);
            _FileReadServiceMOCk.Verify(fn =>
                fn.Read(
                    It.IsAny<string>(),
                    It.IsAny<IConverter<SourceMeterReading, string>>()), Times.Once);

            sut.Rows.Should().Be(_SourceMeterReading.Count());

        }

        [Fact]
        public void then_retrieve_meter_readings_by_account()
        {
            //ARRANGE
            _ConfigurationSectionMOCK.Setup(a => a.Value)
                .Returns(It.IsAny<string>());

            _FileReadServiceMOCk.Setup(fn => fn.Read(
                It.IsAny<string>(),
                It.IsAny<IConverter<SourceMeterReading, string>>()))
                    .Returns(_SourceMeterReading);

            var sut = new MeterReadingFile(
                _ConfigurationMOCK.Object,
                It.IsAny<IConverter<SourceMeterReading, string>>(),
                _FileReadServiceMOCk.Object);

            int uniqueAccountReadingCountEXP = _SourceMeterReading.Select(x => x.AccountId).Distinct().Count();
            int countOfUniqueAccountReadingACT = 0;
            int numberOfReadingsReadACT = 0;

            //ACT
            foreach (var readingsACT in sut.RetrieveReadingsByAccountId())
            {
                //ASSERT
                foreach( var readingACT in readingsACT )
                {
                    _SourceMeterReading.First(d =>
                         d.AccountId == readingACT.AccountId &&
                         d.MeterReadingDateTime == readingACT.MeterReadingDateTime &&
                         d.MeterReadValue == readingACT.MeterReadValue)
                        .Should().NotBeNull();

                    numberOfReadingsReadACT++;
                }
                countOfUniqueAccountReadingACT++;
            }
            countOfUniqueAccountReadingACT.Should().Be(uniqueAccountReadingCountEXP);
            numberOfReadingsReadACT.Should().Be(_SourceMeterReading.Count());
        }

        [Fact]
        public void then_convert_string_meter_reading_into_source_meter_reading()
        {
            //ARRAGE
            var inputLine = "2344,22/04/2019 09:24,1002";
            var inputLineEXP = inputLine.Split(',');
            var sut = new MeterReadingConverter();

            //ACT
            var meterReadingACT = sut.Convert(inputLine);

            //ASSERT
            meterReadingACT.AccountId.Should().Be(inputLineEXP[0]);
            meterReadingACT.MeterReadingDateTime.Should().Be(inputLineEXP[1]);
            meterReadingACT.MeterReadValue.Should().Be(inputLineEXP[2]);
        }

        [Fact]
        public void then_convert_source_meter_reading_into_account()
        {
            //ARRANGE
            var readingEXP = 1002;
            var sourceMeterReading = new List<SourceMeterReading>(){
                new SourceMeterReading(
                    "2344", "22/04/2019  09:24:00", readingEXP.ToString())
            };
            var dateTimeEXP = new DateTime(2019, 04, 22, 9, 24, 0);

            var sut = new AccountConverter();

            //ACT
            var accountACT = sut.Convert(sourceMeterReading);

            //ASSERT
            accountACT.Id.ToString().Should()
                .Be(_SourceMeterReading.First().AccountId);
            accountACT.Readings.Count().Should()
                .Be(sourceMeterReading.Count());

           var firstReading = accountACT.Readings.ElementAt(0).MeterReading;
           firstReading.DateOfReading.Should().Be(dateTimeEXP);
           firstReading.Reading.Should().Be(readingEXP);
        
        }


        [Theory]
        [InlineData("2344,22/04/2019 09:24")]
        [InlineData(null)]
        public void then_raise_exception_when_source_meter_reading_invalid(string souceMeterReadingEXP)
        {
            //ARRANGE
            var sut = new MeterReadingConverter();

            //ACT
            Action act = () => sut.Convert(souceMeterReadingEXP);

            //ASSERT
            act.Should().ThrowExactly<FormatException>();
        }
    }
}
