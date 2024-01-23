using HPTA.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HPTA.Migrations
{
    public class MigrationsDbContextFactory : IDesignTimeDbContextFactory<HPTADbContext>
    {
        public HPTADbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<HPTADbContext> optionsBuilder = CreateOptionsBuilder();
            return new HPTADbContext(optionsBuilder.Options);
        }

        protected DbContextOptionsBuilder<HPTADbContext> CreateOptionsBuilder()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../HPTA.Api"))
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                        .Build();

            // Get the connection string
            var connectionString = configuration.GetConnectionString("MasterDbConnectionString");
            var optionsBuilder = new DbContextOptionsBuilder<HPTADbContext>();
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsAssembly("HPTA.Migrations"));
            return optionsBuilder;
        }
    }
}
