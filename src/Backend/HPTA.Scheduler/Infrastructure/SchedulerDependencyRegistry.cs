using Hangfire;
using HPTA.Common.Configurations;
using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HPTA.Scheduler.Infrastructure
{
    public static class SchedulerDependencyRegistry
    {
        public static void RegisterScheduler(this IServiceCollection services, ConnectionStrings appSettings)
        {
            services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(appSettings.MasterDbConnectionString));

            services.AddHangfireServer();
        }

        public static void InitTeamSync(this IApplicationBuilder app)
        {
#if DEBUG
            app.UseHangfireDashboard();
#endif
            var recurringJobService = app.ApplicationServices.GetRequiredKeyedService<IRecurringJobManager>(null);
            //Scheduled to run the job every day at 1:00 AM UTC
            recurringJobService.AddOrUpdate<IUserService>("devcentral_user_sync", u => u.SyncAllUsersAsync(), "0 1 * * *");
        }
    }
}
