using EnergyDataManager.Domain;
using EnergyDataManager.Domain.Interfaces;
using EnergyDataManager.Domain.ValueObjects;
using EnergyDataManager.Web.Controllers;
using EnergyDataManager.Web.Interfaces;
using EnergyDataReader;
using EnergyDataReader.Interfaces;
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

namespace given_an_energy_data_manager.web.tests
{
    public class when_uploading_meter_readings
    {
        public static IEnumerable<object[]> MeterTestData =>
        new List<object[]>
        {
            new object[] {
                new Queue<List<SourceMeterReading>>(new[] {
                    new List<SourceMeterReading>(){
                        new SourceMeterReading(){
                            AccountId = "2344",
                            MeterReadingDateTime = "22/04/2019  09:24:00",
                            MeterReadValue = "1002"
                        },
                        new SourceMeterReading(){
                            AccountId = "2344",
                            MeterReadingDateTime = "24/04/2019  09:26:00",
                            MeterReadValue = "2002"
                        }
                    },
                    new List<SourceMeterReading>(){
                        new SourceMeterReading(){
                            AccountId = "8080",
                            MeterReadingDateTime = "22/04/2019  09:24:00",
                            MeterReadValue = "1343"
                        },
                        new SourceMeterReading(){
                            AccountId = "8080",
                            MeterReadingDateTime = "22/05/2019  09:24:00",
                            MeterReadValue = "2347"
                        }


                    },
                    null
                }),
                new Queue<Account>( new[] {
                    new  Account(
                        2344,
                        new List<Reading>(){
                            new Reading("22/04/2019  09:24:00", "1002" ),
                            new Reading("22/04/2019  09:24:00", "2002" )}),
                    new Account(
                        8080,
                        new List<Reading>(){
                            new Reading("22/04/2019  09:24:00", "1343" ),
                        })

                })
            }
        };
            
        [Theory]
        [MemberData(nameof(MeterTestData))]
        public void then_return_valid_number_of_rows(
            Queue<List<SourceMeterReading>> sourceMeterReadingsEXP,
            Queue<Account> accountsEXP)
        {
            //ARRANGE
            var totalReadingsEXP = 4;
            var totalAccountsEXP = 2;
            var fileNameEXP = "Meter_Reading.csv";
            var readingsPerAccount = 2;
            string fileNameACT = null;


            Mock<IConfiguration> configurationMOCK =
                new Mock<IConfiguration>();

            Mock<IConfigurationSection> configurationSection = new Mock<IConfigurationSection>();

            Mock<IMeterReader> meterReaderMOCK =
                new Mock<IMeterReader>();

            Mock<IConverter<SourceMeterReading, Account>> converterMOCK =
                new Mock<IConverter<SourceMeterReading, Account>>();

            Mock<IEnergyDataManagerRepo> edmRepoMOCK =
                new Mock<IEnergyDataManagerRepo>();


            meterReaderMOCK.Setup(fn =>
                fn.LoadReadingsAsync(It.IsAny<string>()))
                    .Callback<string>(fileName => fileNameACT = fileName)
                    .Returns(Task.FromResult(totalReadingsEXP));

            meterReaderMOCK.Setup(fn => fn.RetrieveReadingsByAccountId())
                .Returns(sourceMeterReadingsEXP.Dequeue);

            converterMOCK.Setup(fn => fn.Convert(It.IsAny<IEnumerable<SourceMeterReading>>()))
                .Returns(accountsEXP.Dequeue);

            configurationSection.Setup(a => a.Value).Returns(fileNameEXP);
            configurationMOCK.Setup(fn =>
                fn.GetSection(It.IsAny<string>()))
                .Returns(configurationSection.Object);

            edmRepoMOCK.Setup(fn =>
               fn.UpdateAccountWithMeterReadingsAsync(
                   It.IsAny<Account>()))
                   .Returns(Task.FromResult(readingsPerAccount));

            var sut = new MeterReadingUploadsController(
                configurationMOCK.Object,
                meterReaderMOCK.Object,
                converterMOCK.Object,
                edmRepoMOCK.Object);

            //ACT
            var responseACT = sut.MeterReadingAsync().Result;

            //ASSERT
            configurationMOCK.Verify(fn => 
                fn.GetSection(It.IsAny<string>()),Times.Once);

            meterReaderMOCK.Verify(fn => 
                fn.LoadReadingsAsync(It.IsAny<string>()), Times.Once);
            fileNameACT.Should().Be(fileNameEXP);

            meterReaderMOCK.Verify(fn => 
                fn.RetrieveReadingsByAccountId(), Times.Exactly(totalAccountsEXP+1)); //to accommodate null read eof

            converterMOCK.Verify(fn => 
                fn.Convert(It.IsAny<List<SourceMeterReading>>()), Times.Exactly(totalAccountsEXP));

            edmRepoMOCK.Verify(fn => fn.UpdateAccountWithMeterReadingsAsync(
                It.IsAny<Account>()), Times.Exactly(totalAccountsEXP));

            var responseObject = responseACT as ObjectResult;
            responseObject.StatusCode.Should().Be((int)HttpStatusCode.Created);
            responseObject.Value.Should()
                .Be($"{readingsPerAccount* totalAccountsEXP}/{totalReadingsEXP} meter readings were uploaded"); 
        }

    }
}
