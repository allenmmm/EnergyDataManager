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
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace given_an_energy_data_manager.web.tests
{
    public class when_uploading_meter_readings
    {


        /*   public static IEnumerable<object[]> MeterTestData =>
            new List<object[]>
            {
                   new object[] {
                       new List<SourceMeterReading>() {
                               new SourceMeterReading(){
                                   AccountId = "2344",
                                   MeterReadingDateTime = "22/04/2019  09:24:00",
                                   MeterReadValue = "1002"
                               },
                               new SourceMeterReading(){
                                   AccountId = "8766",
                                   MeterReadingDateTime = "22/04/2019  12:25:00",
                                   MeterReadValue = "3440"
                               },
                               new SourceMeterReading(){
                                   AccountId = "2350",
                                   MeterReadingDateTime = "22/04/2019  12:25:00",
                                   MeterReadValue = "9787"
                       },
                   }
                  }
            };*/
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
                            }
                        },
                        null
                    }),
                    new Queue<List<Reading>>( new[] {
                        new List<Reading>(){
                            new Reading(2344, new List<EnergyDataSample>(){
                                new EnergyDataSample("22/04/2019  09:24:00", "1002" ),
                                new EnergyDataSample("22/04/2019  09:24:00", "2002" ),
                            }),
                            new Reading(8080, new List<EnergyDataSample>(){
                                new EnergyDataSample("22/04/2019  09:24:00", "1343" )
                            })

                    } }) } }; 
                    /*    new Queue<List<EnergyDataSample>>( new[] { 
                            new List<EnergyDataSample>(){ 
                                new EnergyDataSample("22/04/2019  09:24:00", "1002" ),
                                new EnergyDataSample("22/04/2019  09:24:00", "2002" ),
                            },
                             new List<EnergyDataSample>(){
                                new EnergyDataSample("22/04/2019  09:24:00", "1343" ),
                            }

                        })*/
        //   }

        //    };

        [Theory]
        [MemberData(nameof(MeterTestData))]
        public void then_return_valid_number_of_rows(
            Queue<List<SourceMeterReading>> sourceMeterReadingsEXP,
            Queue<List<Reading>> readingsEXP)
        {
            //ARRANGE
            string idACT = null;
            string dateACT = null;
            string meterACT = null;
            var idEXP = "1234";
            var dateEXP = "22/04/2019  09:24:00";
            var meterEXP = "8080";
            var readingCountEXP = sourceMeterReadingsEXP.Count;
            var fileNameEXP = "Meter_Reading.csv";
            string fileNameACT = null;
            Reading readingACT = null;

            List<SourceMeterReading> sourceMeterReadingACT = null;

            Mock<IConfiguration> configurationMOCK =
                new Mock<IConfiguration>();

            Mock<IMeterReader> meterReaderMOCK =
                new Mock<IMeterReader>();

            Mock<IConverter<SourceMeterReading, Reading>> converterMOCK =
                new Mock<IConverter<SourceMeterReading, Reading>>();

            Mock<IEnergyDataManagerRepo> edmRepoMOCK =
                new Mock<IEnergyDataManagerRepo>();


            meterReaderMOCK.Setup(fn =>
                fn.LoadReadings(It.IsAny<string>()))
                    .Callback<string>(fileName => fileNameACT = fileName);

            meterReaderMOCK.Setup(fn => fn.RetrieveReadingsByAccountId())
                .Returns(sourceMeterReadingsEXP.Peek);

            converterMOCK.Setup(fn => fn.Convert(It.IsAny<List<SourceMeterReading>>()))
                .Callback<List<SourceMeterReading>>(smr => sourceMeterReadingACT = smr)
                .Returns(readingsEXP.Dequeue);

            configurationMOCK.Setup(fn =>
                fn.GetValue<string>(It.IsAny<string>()))
                .Returns(fileNameEXP);

            edmRepoMOCK.Setup(fn =>
               fn.UpdateAccountWithMeterReadings(
                   It.IsAny<Reading>())).
                   Callback<Reading>(
                        reading => readingACT = reading)
                   .Returns(readingCountEXP);


            var sut = new MeterReadingUploadsController(
                configurationMOCK.Object,
                meterReaderMOCK.Object,
                converterMOCK.Object,
                edmRepoMOCK.Object);

            //ACT
            var responseACT = sut.MeterReadingAsync().Result;


            //ASSERT
            configurationMOCK.Verify(fn => fn.GetValue<string>(It.IsAny<string>()),Times.Once);

            meterReaderMOCK.Verify(fn => 
                fn.LoadReadings(It.IsAny<string>()), Times.Exactly(readingCountEXP));
            fileNameACT.Should().Be(fileNameEXP);

            meterReaderMOCK.Verify(fn => fn.RetrieveReadingsByAccountId(), Times.Exactly(readingCountEXP));

            converterMOCK.Verify(fn => 
                fn.Convert(It.IsAny<List<SourceMeterReading>>()), Times.Exactly(readingCountEXP));
            sourceMeterReadingACT.Should().BeEquivalentTo(sourceMeterReadingsEXP.Dequeue());

            edmRepoMOCK.Verify(fn => fn.UpdateAccountWithMeterReadings(
                It.IsAny<Reading>()), Times.Exactly(readingCountEXP));

            responseACT.Should().BeOfType<OkObjectResult>();
            var responseObject = responseACT as OkObjectResult;
            string numerOfRowsACT = responseObject.Value as string;
            numerOfRowsACT.Should().Be(readingCountEXP.ToString());
        }
    }
}
