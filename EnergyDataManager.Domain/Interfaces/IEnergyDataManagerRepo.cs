using EnergyDataManager.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnergyDataManager.Domain.Interfaces
{
    public interface IEnergyDataManagerRepo
    {
        Task<int> UpdateAccountWithMeterReadingsAsync(
            Account account);
    }
}
