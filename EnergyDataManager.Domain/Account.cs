using EnergyDataReader.File;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnergyDataManager.Domain
{
    public class Account : Entity<int>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        private  List<Reading> _Readings = new List<Reading>();
        public IEnumerable<Reading> Readings => _Readings.AsReadOnly();

        public Account(int id, List<Reading> readings = null)
        {
            Id = id;
            _Readings = readings ?? new List<Reading>(); 
        }

        //write tests for this
        public Account( EnergyDataReader.Account.File.Account account )
        {
            int id;
            Guard.AgainstNull(account.AccountId, "Account Id must be valid");
            Guard.AgainstFalse(
                int.TryParse(account.AccountId, out id),
                "Source meter readings have invalid account id");

            Id = id;
            FirstName = account.FirstName;
            LastName = account.LastName;
        }

        private Account() { }

        public void UpdateReadings(IEnumerable<Reading> readings)
        {
            var latestExistingReading = _Readings.FirstOrDefault();
            if (latestExistingReading == null) //No readings so add them all
            {
                _Readings = readings.Select(r => 
                    new Reading(r.MeterReading)).ToList();
            }
            else
            { 
                _Readings.ForEach(r =>
                {
                    if (r.MeterReading > latestExistingReading.MeterReading)
                        latestExistingReading = r;
                });

                var readingsToAdd = readings.Where(r =>
                    r.MeterReading > latestExistingReading.MeterReading);

                _Readings.AddRange(
                     readingsToAdd.Select(r =>
                        new Reading(r.MeterReading)).ToList());
            }
        }


        public static Account Create(
            IEnumerable<SourceMeterReading> sourceMeterReadings)
        {
            Guard.AgainstNull(sourceMeterReadings, "Source meter readings are invalid");
            var accountIds = sourceMeterReadings.Select(s => s.AccountId);
            var meterReadings = new List<Reading>();
             int id = 0;

            try {
                Guard.AgainstGreaterThan(
                    1,
                    accountIds.Distinct().Count(),
                    "Source meter readings unexpectedly pertain to multiple accounts error");
                Guard.AgainstFalse(
                    int.TryParse(accountIds.First(), out id),
                    "Source meter readings have invalid account id");

                foreach (var sourceMeterReading in sourceMeterReadings)
                {

                    var reading = Reading.Create(
                        sourceMeterReading.MeterReadingDateTime,
                        sourceMeterReading.MeterReadValue);

                    if (reading != null)
                        meterReadings.Add(reading);
                }

                return new Account(
                    id,
                    meterReadings);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
