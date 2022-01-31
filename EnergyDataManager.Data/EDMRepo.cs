using EnergyDataManager.Domain;
using EnergyDataManager.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnergyDataManager.Data
{
    public class EDMRepo : IEnergyDataManagerRepo
    {
        private readonly EDMContext _Context;

        public EDMRepo(EDMContext context)
        {
            _Context = context;
        }

        public async Task<int> UpdateAccountWithMeterReadingsAsync(Account account)
        {
            var accountToUpdate = await _Context.Accounts
                .Include(r => r.Readings)
                .FirstOrDefaultAsync(a => a.Id == account.Id);
            accountToUpdate?.UpdateReadings (account.Readings);
            return(await _Context.SaveChangesAsync());
        }
    }
}
