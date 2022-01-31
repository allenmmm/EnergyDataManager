using EnergyDataManager.Domain;
using EnergyDataManager.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using FluentAssertions;

namespace given_an_energy_data_manager.domain.tests
{
    public class when_updating_an_account_with_latest_reading
    {

        [Fact]
        public void then_update_when_no_existing_readings()
        {
            //ARRANGE
            List<Reading> newReadingsEXP = new List<Reading>()
            {
                 new Reading(
                    new MeterReading_VO(         
                        "14/12/2019  09:23:59",
                        "8080"))
            };
            var sut = new Account(2);

            //ACT
            sut.UpdateReadings(newReadingsEXP);

            //ASSERT
            sut.Readings.Count().Should().Be(newReadingsEXP.Count);
            sut.Readings.First().MeterReading.Should()
                .Be(newReadingsEXP.First().MeterReading);
        }



        [Fact]
        public void then_update_from_mixture_of_old_and_new_readings()
        {
            //ARRANGE
            List<Reading> existingReadings = new List<Reading>()
            {
                 new Reading(
                    new MeterReading_VO(
                        "14/12/2019  09:23:59",
                        "8080")),
                 new Reading(
                    new MeterReading_VO(
                        "14/12/2019  09:24:00",
                        "8081")),
                 new Reading(
                    new MeterReading_VO(
                        "13/12/2019  09:23:59",
                        "7999")) 
            };

            var latereading1EXP = new Reading(
                new MeterReading_VO(
                    "14/12/2019  09:24:00",  // OK 
                    "8083"));

            var latereading2EXP = new Reading(
                new MeterReading_VO(
                "14/12/2019  09:24:00",  // OK
                "8082"));

            //Only the 2 element is later 
            List<Reading> newReadings = new List<Reading>()
            {
                 new Reading(
                    new MeterReading_VO(          //NO - TOO EARLY
                        "14/12/2019  09:23:59",
                        "8080")),
                 new Reading(
                    new MeterReading_VO(           //NO  READING IS LESS
                        "14/12/2019  09:24:00",
                        "8080")),
                 new Reading(
                    new MeterReading_VO(
                        "14/12/2019  09:24:00",  //NO READING IS THE SAME 
                        "8081")),
                 latereading1EXP,
                 latereading2EXP
            };
            var sut = new Account(2, existingReadings);

            //ACT
            sut.UpdateReadings(newReadings);

            //ASSERT
            sut.Readings.Count().Should().Be(5);
            sut.Readings.ToList().First(x =>
                x.MeterReading == latereading1EXP.MeterReading)
                    .Should().NotBeNull();
            sut.Readings.ToList().First(x =>
               x.MeterReading == latereading2EXP.MeterReading)
                   .Should().NotBeNull();
        }
    }
}
