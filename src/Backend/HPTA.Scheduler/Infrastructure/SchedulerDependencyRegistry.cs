﻿using Hangfire;
using HPTA.Common.Configurations;
using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HPTA.Scheduler.Infrastructure
{
    public static class SchedulerDependencyRegistry
    {
        public static void RegisterScheduler(this IServiceCollection services, ApplicationSettings appSettings)
        {
            services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(appSettings.MasterDbConnectionString));

            services.AddHangfireServer();
            services.AddSingleton<IAITaskStatusUpdater>(f => new AITaskManager());
        }

        public static void InitTeamSync(this IApplicationBuilder app)
        {
#if DEBUG
            app.UseHangfireDashboard();
#endif
            var recurringJobService = app.ApplicationServices.GetRequiredKeyedService<IRecurringJobManager>(null);
            //Scheduled to run the job every day at 1:00 AM UTC
            recurringJobService.AddOrUpdate<IUserService>("devcentral_user_sync", u => u.SyncAllUsersAsync(), "0 1 * * *");
            // Scheduled to run the job at 1st day of every month
            recurringJobService.AddOrUpdate<ISurveyService>("auto_create_survey", s => s.CreateSurvey(), "0 0 1 * *");
            //BackgroundJob.Enqueue<ISurveyService>(s => s.CreateSurvey());
        }
    }
}
