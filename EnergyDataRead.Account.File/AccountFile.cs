using EnergyDataManager.SharedKernel.interfaces;
using EnergyDataReader.Account.File.Interfaces;
using Microsoft.Extensions.Configuration;
using SharedKernel;
using SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace EnergyDataReader.Account.File
{
    public class AccountFile : File<Account, string>, IExtractAccounts
    {
        public AccountFile(
            IConfiguration configuration,
            IConverter<Account, string> converter,
            IFileReadService<Account, string> accountFileService)
                : base(
                        configuration.GetSection("AccountsFilePath").Value,
                        converter,
                        accountFileService)
        {
        }

        public virtual IEnumerable<Account> GetAccounts()
        {
            return Data.Select(r =>
                new Account(r)).ToList();
        }
    }
}
