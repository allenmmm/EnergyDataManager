using EnergyDataManager.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyDataManager.Domain
{
    public class Reading
    {
        public int AccountId { get; set; }

        public EnergyDataSample energyDataSample  { get; set; }

        public Reading(string dateTime, string meterValue)
        {

        }
    }
}
