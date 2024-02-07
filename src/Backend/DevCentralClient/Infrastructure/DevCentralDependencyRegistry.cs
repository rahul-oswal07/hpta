using DevCentralClient.Contracts;
using DevCentralClient.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DevCentralClient.Infrastructure
{
    public static class DevCentralDependencyRegistry
    {
        public static void RegisterDevCentralClient(this IServiceCollection services, DevCentralConfig config)
        {
            services.AddSingleton<IDevCentralClientService, DevCentralClientService>((serviceProvider) =>
            {
                return new DevCentralClientService(config);
            });
        }
    }
}
