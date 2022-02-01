using EnergyDataManager.Data;
using EnergyDataManager.Web;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace given_an_energy_data_manager.integration.tests
{
    public class when_uploading_meter_readings
        : SQLServerProvider<EDMContext>,
            IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public when_uploading_meter_readings(
            CustomWebApplicationFactory<Startup> factory) : base(@"Scripts\")
        {
            string connectionString = null;
            //Additional customisation of the IOC container this can be performed in test if 
            //further tailoring required
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    // Build the intermediate service provider  to retrieve connection string
                    connectionString = services.BuildServiceProvider()
                        .GetRequiredService<IConfiguration>()
                        .GetConnectionString("EDM_DB");
                });
            }).CreateClient();
            // configure the dbcontext options
            PostConifgureContextOptions(connectionString);
            SeedFromFile("then_seed_test_clients.sql");
        }

        [Fact]
        public async Task then_post_readings()
        {
            //ARRANGE
            HttpContent c = new StringContent("", Encoding.UTF8, "application/json");

            //ACT
            var responseACT = await _client.PostAsync(
                "meter-reading-uploads", c);

            //ASSERT
            responseACT.StatusCode.Should().Be(HttpStatusCode.Created);
            responseACT.Content.Headers.ContentType.ToString()
                 .Should().Contain("application/json");

            var contentACT = await responseACT.Content.ReadAsStringAsync();
            contentACT = JsonSerializer.Deserialize<string>(contentACT);

            contentACT.Should()
                .Be("2/3 meter readings were uploaded");
        }
    }
}
