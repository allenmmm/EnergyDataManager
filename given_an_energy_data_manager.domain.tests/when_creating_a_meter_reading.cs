using EnergyDataManager.Domain;
using EnergyDataManager.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace given_an_energy_data_manager.domain.tests
{
    public class when_creating_a_meter_reading
    {
        [Fact]
        public void then_populate_a_reading_vo()
        {
            //ARRANGE
            string dateTimeStringEXP = "22/04/2019  09:24:00";
            int meterValueEXP = 1002;
            DateTime dateTimeEXP = new DateTime(2019, 4, 22, 9, 24, 0);
            //ACT
            var sut = new MeterReading_VO(
                dateTimeStringEXP,
                meterValueEXP.ToString());
            //ASSERT
            sut.Date.Should().Be(dateTimeEXP);
            sut.Value.Should().Be(meterValueEXP);
        }

        [Theory]
        [InlineData("32/04/2019  09:24:00")]
        [InlineData("Clearly in valid")]
        [InlineData(null)]
        [InlineData("12/04/2019  24:00:00")]
        [InlineData("12/04/2019")]
        [InlineData("23:00:00")]
        [InlineData("12/13/2019  09:24:00")]
        [InlineData("2019/12/14  09:24:00")]
        public void then_assert_if_date_is_invalid(string dateEXP)
        {
           //ARRANGE

            //ACT
            Action action = () => new MeterReading_VO(
                     dateEXP,
                     "1002");

            //ASSERT
            action.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("-123")]
        [InlineData("123")]
        [InlineData("12.2")]
        [InlineData(null)]
        [InlineData("80808")]
        [InlineData("yeah")]
        public void then_assert_if_meter_reading_is_invalid(string meterReadingEXP)
        {
            //ARRANGE

            //ACT
            Action action = () => new MeterReading_VO(
                     "22/04/2019  09:24:00",
                     meterReadingEXP);

            //ASSERT
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void then_populate_a_reading()
        {
            //ARRANGE
            string dateTimeStringEXP = "22/04/2019  09:24:00";
            int meterValueEXP = 1002;
            DateTime dateTimeEXP = new DateTime(2019, 4, 22, 9, 24, 0);
            //ACT
            var sut = Reading.Create(
                dateTimeStringEXP,
                meterValueEXP.ToString());
            //ASSERT
            sut.MeterReading.Date.Should().Be(dateTimeEXP);
            sut.MeterReading.Value.Should().Be(meterValueEXP);
        }

        [Theory]
        [InlineData("bad date","1111")]
        [InlineData("32/04/2019  09:24:00","1111")]
        [InlineData("12/04/2019  09:24:00", "11121")]
        [InlineData(null, "11121")]
        public void then_handle_invalid_reading(string dateTime, string meterValue)
        {
            //ACT
            var readingACT =  Reading.Create(
                dateTime,
                meterValue);
            //ASSERT
            readingACT.Should().BeNull();
        }

    }
}
