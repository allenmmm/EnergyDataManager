using EnergyDataManager.Data;
using EnergyDataReader.Account.File;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace given_an_energy_data_manager.data.tests
{

    [Collection("Sequential")]
    public class when_seeding_accounts : TestBase<EDMRepo, EDMContext>
    {
        [Fact]
        public void then_seed()
        {
            //ARRANGE
            List<Account> accEXP = new
                 List<Account>()
            {
                new Account("1111","bob","cratchit"),
                new Account("1112", "rob", "bad"),
            };

            //ACT
            var rowsACT = _context.Seed(accEXP);

            //ASSERT
            rowsACT.Should().Be(accEXP.Count);
        }

    }
}
