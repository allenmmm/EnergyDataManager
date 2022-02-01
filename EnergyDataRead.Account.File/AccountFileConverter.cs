using EnergyDataManager.SharedKernel.interfaces;
using System;

namespace EnergyDataReader.Account.File
{
    public class AccountFileConverter : IConverter<Account, string>
    {
        public string Convert(Account source_object)
        {
            return null;
        }

        public Account Convert(string source_object)
        {
            try
            {
                var decodedSource = source_object.Split(',');
                return new Account(
                    decodedSource[0],
                    decodedSource[1],
                    decodedSource[2]);
            }
            catch (Exception ex)
            {
                throw new
                    FormatException("Account file is incorrectly formatted");
            }
        }
    }
}
