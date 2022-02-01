using EnergyDataReader.Account.File;
using FluentAssertions;
using SharedKernel;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace given_an_energy_data_manager.integration.tests
{
    public class when_reading_accounts_file
    {
        [Fact]
        public void then_get_accounts_from_file()
        {
            //ARRANGE
            var sut = new FileReadService<Account>();

            var fileNameEXP = @"TestData\Test_Accounts.csv";
            var fileACT = File.ReadLines(fileNameEXP);

            //ACT
            var accountsEXP =
                sut.Read(fileNameEXP, new AccountFileConverter());

            //ASSERT
            accountsEXP.Count().Should().Be(fileACT.Count() - 1);
        }

        [Fact]
        public void then_throw_exception_if_file_not_present()
        {
            //ARRANGE
            var sut = new FileReadService<Account>();
            var fileNameEXP = @"TestData\NotTHERE.csv";

            //ACT
            Action act = () => sut.Read(fileNameEXP, new AccountFileConverter());

            //ASSERT
            act.Should().ThrowExactly<IOException>();
        }
    }
}
