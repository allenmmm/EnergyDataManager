using EnergyDataManager.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace EnergyDataManager.Domain
{
    public class Account
    {
        public int Id { get;  set; }
        public string LastName { get;  set; }
        public string Surname { get;  set; }

        private readonly List<Reading> _Readings = new List<Reading>();
        public IEnumerable<Reading> Reading => _Readings.AsReadOnly();

        public Account(int Id, IEnumerable<Reading> readings)
        {

        }
    }
}
