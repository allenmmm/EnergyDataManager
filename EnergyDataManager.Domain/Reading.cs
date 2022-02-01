using EnergyDataManager.Domain.ValueObjects;
using SharedKernel;
using System;

namespace EnergyDataManager.Domain
{
    public class Reading : Entity<int>
    {

        public MeterReading_VO MeterReading { get; private set; }
        public int AccountId { get; private set; }

        private Reading() { }

        public Reading(MeterReading_VO meterReading)
        {
            MeterReading = meterReading;
        }

        public static Reading Create(string dateTime, string meterValue)
        {
            try
            {
                return new Reading(new MeterReading_VO(dateTime, meterValue));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
