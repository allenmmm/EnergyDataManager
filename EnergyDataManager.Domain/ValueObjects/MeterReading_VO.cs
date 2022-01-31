using SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyDataManager.Domain.ValueObjects
{
    public class MeterReading_VO : ValueObject<MeterReading_VO> { 
        public DateTime Date { get; private set; }
        public  int Value { get; private set; }

        private  MeterReading_VO()
        {
        }

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

            Date = Guard.AgainstInvalidDateTime(dateOfReading, "Date of reading is not a valid a data type"); 
            Guard.AgainstFalse(int.TryParse(meterValue, out reading), "Meter value is not a valid data type");
            reading = Guard.AgainstLessThan(0,int.Parse(meterValue), "Meter value is not a valid data type");
            Guard.AgainstNotEqual(meterValue.ToString().Length,4, "Meter value must be an int of 4 digits");
            Value = reading;
        }

        public static bool operator > (MeterReading_VO x, MeterReading_VO y)
        {
            if (x.Date > y.Date)
                return true;
            if (x.Date == y.Date && x.Value > y.Value)
                return true;
            return false;
        }

        public static bool operator < (MeterReading_VO x, MeterReading_VO y)
        {
            if (x.Date < y.Date)
                return true;
            if (x.Date == y.Date && x.Value < y.Value)
                return true;
            return false;
        }
    }
}
