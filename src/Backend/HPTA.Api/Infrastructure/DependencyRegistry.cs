using HPTA.Common.Configurations;
using HPTA.Services.Infrastructure;

namespace HPTA.Api.Infrastructure;

public static class DependencyRegistry
{
    public static void RegisterDependency(this IServiceCollection services, ApplicationSettings appSettings)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton(appSettings);
        ServiceDependencyRegistry.RegisterServices(services, appSettings);
    }
}
