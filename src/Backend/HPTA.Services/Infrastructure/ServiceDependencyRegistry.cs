using HPTA.Common.Configurations;
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

        services.AddTransient<IOtpService, OtpService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
    }
}
