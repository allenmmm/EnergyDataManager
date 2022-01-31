using EnergyDataManager.Domain.ValueObjects;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace EnergyDataManager.Domain
{
    public class Reading
    {
        public int AccountId { get; private set; }

        public MeterReading_VO MeterReading { get; private set; }

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
