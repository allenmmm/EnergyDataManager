using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;

namespace given_an_energy_data_manager.integration.tests
{
    public static class Utils
    {
        public static void ReplaceService<T>(
           IServiceCollection services,
           Type interfaceType,
           Mock<T> mockObject) where T : class
        {
            var descriptorIDataClient = services
                .FirstOrDefault(descriptor => descriptor.ServiceType == interfaceType);
            services.Remove(descriptorIDataClient);
            services.AddSingleton<T>(mockObject.Object);
        }
    }
}
