using HPTA.Common.Configurations;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;
using HPTA.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace HPTA.Services.Infrastructure;

public static class ServiceDependencyRegistry
{
    public static void RegisterServices(this IServiceCollection services, ApplicationSettings appSettings)
    {
        RepositoryDependencyRegistry.RetisterRepositories(services, appSettings);
        services.AddScoped<IIdentityService, AzureAdIdentityService>();
        services.AddTransient<IQuestionService, QuestionService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<ISubCategoryService, SubCategoryService>();
        services.AddTransient<ISurveyService, SurveyService>();
        services.AddTransient<IAnswerService, AnswerService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITeamService, TeamService>();
        services.AddTransient<IOpenAIService, OpenAIService>();

        services.AddSingleton<IHashingService, HashingService>((services) =>
        {
            return new HashingService(appSettings.OTPConfig.Secret);
        });
        services.AddTransient<IOtpService, OtpService>(services =>
        {
            return new OtpService(services.GetRequiredService<IOTPRequestRepository>(), services.GetRequiredService<IUserRepository>(), services.GetRequiredService<IHashingService>(), appSettings.OTPConfig.ValidityInMinutes);
        });
        services.AddSingleton<IJwtTokenService, JwtTokenService>(services =>
        {
            return new JwtTokenService(appSettings.JwtConfig);
        });
    }
}
