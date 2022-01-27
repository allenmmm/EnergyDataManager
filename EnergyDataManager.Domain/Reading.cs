using EnergyDataManager.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyDataManager.Domain
{
    public class Reading
    {
        public string AccountId { get; set; }

        private readonly List<EnergyDataSample> _EnergyDataSamples = new List<EnergyDataSample>();
        public IEnumerable<EnergyDataSample> EnergyDataSamples => _EnergyDataSamples.AsReadOnly();

        public Reading(int id, List<EnergyDataSample> energyDataSamples)
        {

        }

    }
}
