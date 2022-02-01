using EnergyDataManager.Domain;
using EnergyDataManager.SharedKernel.Interfaces;
using EnergyDataReader.File;
using System;
using System.Collections.Generic;

namespace EnergyDataManager.Web.DomainServices
{
    public class Account_SourceMeterReading_Converter : IConverterList<Account, SourceMeterReading>
    {
        public Account Convert(IEnumerable<SourceMeterReading> source_object)
        {
            return Account.Create(source_object);
        }

        public IEnumerable<SourceMeterReading> Convert(Account source_object)
        {
            return null;
        }
    }
}
