using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyDataReader
{
    public class SourceMeterReading
    {
        public string AccountId { get; set; }
        public string MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }


        public SourceMeterReading( 
            string accountId,
            string meterReadingDateTime,
            string meterReadValue)
        {
            AccountId = accountId;
            MeterReadingDateTime = meterReadingDateTime;
            MeterReadValue = meterReadValue;
        }

        public SourceMeterReading(SourceMeterReading sourceMeterReading)
        {
            AccountId = sourceMeterReading.AccountId;
            MeterReadingDateTime = sourceMeterReading.MeterReadingDateTime;
            MeterReadValue = sourceMeterReading.MeterReadValue;
        }

    }
}
