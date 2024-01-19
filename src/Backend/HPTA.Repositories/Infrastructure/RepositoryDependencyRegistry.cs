using HPTA.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HPTA.Repositories.Infrastructure;

public static class RepositoryDependencyRegistry
{
    public static void DependencyRegistry(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddDbContext<HPTADbContext>(options =>
        {
            options.UseSqlServer(appSettings.MasterDbConnectionString);
        });
    }
}
