using SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyDataManager.Domain.ValueObjects
{
    public class MeterReading_VO : ValueObject<MeterReading_VO> { 
        public DateTime DateOfReading { get; private set; }
        public  int Reading { get; private set; }

        public MeterReading_VO(string dateOfReading, string meterValue)
        {
            Guard.AgainstNull(dateOfReading, "Date of reading is not a valid data type");
            Guard.AgainstNull(meterValue, "Meter value is not a valid a data type");
            int reading;
            var dateTimeArray = dateOfReading.Split("  ");
            var dateArray = dateTimeArray[0].Split("/");
            var timeArray = dateTimeArray[1].Split(":");

            DateTime date = new DateTime(
                int.Parse(dateArray[2]),
                int.Parse(dateArray[1]),
                int.Parse(dateArray[0]),
                int.Parse(timeArray[0]),
                int.Parse(timeArray[1]),
                int.Parse(timeArray[2]));

            DateOfReading = Guard.AgainstInvalidDateTime(dateOfReading, "Date of reading is not a valid a data type"); 
            Guard.AgainstFalse(int.TryParse(meterValue, out reading), "Meter value is not a valid data type");
            reading = Guard.AgainstLessThan(0,int.Parse(meterValue), "Meter value is not a valid data type");
            Guard.AgainstNotEqual(meterValue.ToString().Length,4, "Meter value must be an int of 4 digits");
            Reading = reading;
        }

        public static bool operator > (MeterReading_VO x, MeterReading_VO y)
        {
            if (x.DateOfReading > y.DateOfReading)
                return true;
            if (x.DateOfReading == y.DateOfReading && x.Reading > y.Reading)
                return true;
            return false;
        }

        public static bool operator < (MeterReading_VO x, MeterReading_VO y)
        {
            if (x.DateOfReading < y.DateOfReading)
                return true;
            if (x.DateOfReading == y.DateOfReading && x.Reading < y.Reading)
                return true;
            return false;
        }
    }
}
