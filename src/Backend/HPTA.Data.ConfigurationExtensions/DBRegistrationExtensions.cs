using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HPTA.Data.ConfigurationExtensions
{
    public static class DBRegistrationExtensions
    {
        public static void RegisterDB(this IServiceCollection services, IConfiguration configuration, string key = "ConnectionString")
        {
            services.AddDbContextPool<HPTADbContext>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("DbContext")); //Use this option for real database
                options.UseInMemoryDatabase("HPTADB"); // use this option and comment the other one if you want in memory database. Everytime you run app, the data is cleared.
            });
        }
    }
}
