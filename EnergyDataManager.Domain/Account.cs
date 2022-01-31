using EnergyDataManager.Domain.ValueObjects;
using EnergyDataReader.File;
using EnergyDataManager.Domain;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnergyDataManager.Domain
{
    public class Account 
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        private  List<Reading> _Readings = new List<Reading>();
        public IEnumerable<Reading> Readings => _Readings.AsReadOnly();



        public Account(int id, List<Reading> readings = null)
        {
            Id = id;
            _Readings = readings ?? new List<Reading>(); 
        }

        public void UpdateReading(List<Reading> readings)
        {
            //find latest reading in existing
            var latestExistingReading = _Readings.FirstOrDefault();
            if (latestExistingReading == null)
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
                        new Reading(r.MeterReading)).ToList()
                    );
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
