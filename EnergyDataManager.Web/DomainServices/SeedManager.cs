using EnergyDataManager.Data;
using EnergyDataManager.Domain.Interfaces;
using EnergyDataManager.SharedKernel.interfaces;
using EnergyDataReader.Account.File;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataManager.Web.DomainServices
{
    public static class SeedManager
    {
        public static IHost SeedData(this IHost host)
        {
            try
            {
                using (var serviceScope = host.Services.CreateScope())
                {
                    using (var context = serviceScope.ServiceProvider.GetService<EDMContext>())
                    {
                        var conf = serviceScope.ServiceProvider
                            .GetService<IConfiguration>();

                        context.Seed(
                            new AccountFile(conf,
                                new AccountFileConverter(),
                                new FileReadService<Account>())
                                    .GetAccounts());
                    };
                };
            }
            catch (Exception ex)
            {

                throw;
            }
      
            return host;
        }
    }
}
