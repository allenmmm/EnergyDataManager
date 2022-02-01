using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace given_an_energy_data_manager.integration.tests
{
    public class CustomWebApplicationFactory<TStartupClass> :
        WebApplicationFactory<TStartupClass> where TStartupClass : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // will be called after the `ConfigureServices` from the Startup
            builder.ConfigureTestServices(services =>
            {
                var configuration = new ConfigurationBuilder()
                        .AddJsonFile("integrationsettings.json")
                        .Build();
                services.AddSingleton<IConfiguration>(configuration);
            });
        }
    }
}
