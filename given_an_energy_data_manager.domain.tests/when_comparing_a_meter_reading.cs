using EnergyDataManager.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace given_an_energy_data_manager.domain.tests
{
    public class when_comparing_a_meter_reading
    {
        public when_comparing_a_meter_reading()
        {

        }

        [Theory]
        [InlineData("14/12/2019  09:24:00", "8080", false)]
        [InlineData("14/12/2019  09:23:59", "8080", true)]
        [InlineData("14/12/2019  09:24:01", "8080", false)]
        [InlineData("14/12/2019  09:24:00", "8081", false)]
        [InlineData("14/12/2019  09:24:00", "8079", true)]
        public void then_detect_if_meter_reading_is_greater_than(
            string dateTimeInputEXP,
            string meterReadingEXP,
            bool compareEXP)
        {

            var meterDateACT = new MeterReading_VO(
                "14/12/2019  09:24:00", "8080");

            var meterDateEXP =
                new MeterReading_VO(
                    dateTimeInputEXP, meterReadingEXP);

            //ACT + ASSERT
            (meterDateACT > meterDateEXP).Should().Be(compareEXP);
        }

        [Theory]
        [InlineData("14/12/2019  09:24:00", "8080", false)]
        [InlineData("14/12/2019  09:23:59 ","8080", false)]
        [InlineData("14/12/2019  09:24:01", "8081", true)]
        [InlineData("14/12/2019  09:24:00", "8079", false)] //LESS THAN BUT SMALLER READING
        [InlineData("14/12/2019  09:24:00", "8081", true)] //LESS THAN BUT SMALLER READING
        public void then_detect_if_meter_reading_is_less_than(
            string dateTimeInputEXP,
            string meterReadingEXP,
            bool compareEXP)
        {
            //ARRANGE
            var meterDateACT = new MeterReading_VO(
                "14/12/2019  09:24:00", "8080");

            var meterDateEXP =
                new MeterReading_VO(
                    dateTimeInputEXP, meterReadingEXP);

            //ACT + ASSERT
            (meterDateACT < meterDateEXP).Should().Be(compareEXP);
        }

        [Theory]
        [InlineData("14/12/2019  09:24:00", true)]
        [InlineData("14/12/2019  09:23:59", false)]
        [InlineData("14/12/2019  09:24:01", false)]
        public void then_detect_if_meter_reading_are_equal(
            string dateTimeInputEXP,
            bool compareEXP)
        {
            //ARRANGE
            var meterValueEXP = "8080";
            var meterDateACT = new MeterReading_VO(
                "14/12/2019  09:24:00", meterValueEXP);

            var meterDateEXP =
                new MeterReading_VO(
                    dateTimeInputEXP, meterValueEXP);

            //ACT + ASSERT
            (meterDateACT == meterDateEXP).Should().Be(compareEXP);
        }
    }
}
