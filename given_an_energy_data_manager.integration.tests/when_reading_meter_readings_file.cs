using EnergyDataReader;
using EnergyDataReader.File;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace given_an_energy_data_manager.integration.tests
{
    public class when_reading_meter_readings_file
    {
        [Fact]
        public void then_get_meter_readings_from_file()
        {
            //ARRANGE
            var sut = new MeterReadingFileService();
     
            var fileNameEXP = @"TestData\Meter_Reading.csv";
            var fileACT = File.ReadLines(fileNameEXP);
            //ACT
            var sourceMeterReadingsEXP = 
                sut.Read(fileNameEXP, new MeterReadingConverter());

            //ASSERT
            sourceMeterReadingsEXP.Count().Should().Be(fileACT.Count() - 1);
        }

        [Fact]
        public void then_throw_exception_if_file_not_present()
        {
            //ARRANGE
            var sut = new MeterReadingFileService();

            var fileNameEXP = @"TestData\NotTHERE.csv";
            
            //ACT
            Action act =  () => sut.Read(fileNameEXP, new MeterReadingConverter());

            //ASSERT
            act.Should().ThrowExactly<IOException>();
        }
    }
}
