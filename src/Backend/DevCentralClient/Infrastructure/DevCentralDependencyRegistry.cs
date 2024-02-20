using DevCentralClient.Contracts;
using HPTA.Common.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace DevCentralClient.Infrastructure
{
    public static class DevCentralDependencyRegistry
    {
        public static void RegisterDevCentralClient(this IServiceCollection services, ApplicationSettings applicationSettings)
        {
            services.AddSingleton<IDevCentralClientService, DevCentralClientService>((serviceProvider) =>
            {
                return new DevCentralClientService(applicationSettings.DevCentralConfig);
            });
        }
    }
}
