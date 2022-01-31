using EnergyDataManager.Data;
using EnergyDataManager.Domain;
using EnergyDataReader.File;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace given_an_energy_data_manager.data.tests
{
    //base class created just so the _context class can be accessed and a repo created.
    //otherwise the context would have to be made public in the baseclass
    public class TestBase<TRepo, TContext> : SQLLiteInMemProvider<TContext> where TContext : DbContext
    {
        protected readonly TRepo _Repo;
        public TestBase() :
            base("DataSource=:memory:", @"Scripts\")
        {
            _Repo = (TRepo)Activator.CreateInstance(typeof(TRepo), _context);
        }
    }

    [Collection("Sequential")]
    public class when_posting_energy_meter_values : TestBase<EDMRepo, EDMContext>
    {

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] {
                    new List<SourceMeterReading>() {
                        new SourceMeterReading("2344", "22/04/2019  09:24:00", "1002"),
                        new SourceMeterReading("2344", "22/04/2019  09:25:01", "1003")
                    },
                    1 // because the first row is the same as the db
                },
                new object[] {
                    new List<SourceMeterReading>() {
                        new SourceMeterReading("2345", "22/04/2019  09:24:00", "1002"), 
                    },
                    0//because this id not in db
                },
                new object[] {
                    new List<SourceMeterReading>() {
                        new SourceMeterReading("2344", "22/04/2019  09:23:59", "1002"),
                    },
                    0//because this reading is earlier than one in the db
                },
                new object[] {
                    new List<SourceMeterReading>() {
                        new SourceMeterReading("2344", "22/04/2019  09:24:00", "1003"),
                    },
                    1//because this reading is same date but has higher reading
                },
                new object[] {
                    new List<SourceMeterReading>() {
                        new SourceMeterReading("8080", "22/04/2019  09:24:00", "1003"),
                        new SourceMeterReading("8080", "23/04/2019  08:24:00", "1003"),
                    },
                    2//because these are brand new readings for an empty account so all saved
                }
        };

        [Theory]
        [MemberData(nameof(Data))]
        public async Task then_attempt_posted_when_client_has_readings(
            List<SourceMeterReading> sourceMeterReadingsEXP,
            int rowsUpdatedEXP)
        {
            //ARRANGE
            SeedFromFile("then_populate_accounts.sql");
            var account = Account.Create(sourceMeterReadingsEXP);

            //ACT
           var meterReadingsUpdatedCountACT = 
                await _Repo.UpdateAccountWithMeterReadingsAsync(account);

            //ASSERT
            meterReadingsUpdatedCountACT.Should().Be(rowsUpdatedEXP);
        }


    }
}
