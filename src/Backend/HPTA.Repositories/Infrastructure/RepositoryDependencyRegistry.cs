using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HPTA.Repositories.Infrastructure;

public static class RepositoryDependencyRegistry
{
    public static void DependencyRegistry(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<HPTADbContext>(options =>
        {
            options.UseSqlServer(connectionString, migration => migration.MigrationsAssembly("HPTA.Migrations"));

        });
    }
}
