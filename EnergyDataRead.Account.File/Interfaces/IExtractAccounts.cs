using System.Collections.Generic;

namespace EnergyDataReader.Account.File.Interfaces
{
    public interface IExtractAccounts
    {
      IEnumerable<Account> GetAccounts();
    }
}
