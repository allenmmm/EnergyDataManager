using EnergyDataManager.Domain;
using EnergyDataManager.SharedKernel.Interfaces;
using EnergyDataReader.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataManager.Web.DomainServices
{
    public class AccountConverter : IConverterList<Account, SourceMeterReading>
    {
        public Account Convert(IEnumerable<SourceMeterReading> source_object)
        {
            return Account.Create(source_object);
        }

        public IEnumerable<SourceMeterReading> Convert(Account source_object)
        {
            throw new NotImplementedException();
        }
    }
}
