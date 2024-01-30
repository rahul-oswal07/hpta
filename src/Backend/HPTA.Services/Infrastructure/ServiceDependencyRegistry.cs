using HPTA.Common;
using HPTA.Repositories.Infrastructure;
using HPTA.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace HPTA.Services.Infrastructure;

public static class ServiceDependencyRegistry
{
    public static void RegisterDependency(this IServiceCollection services, ConnectionStrings appSettings)
    {
        RepositoryDependencyRegistry.DependencyRegistry(services, appSettings);

        services.AddTransient<IQuestionService, QuestionService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<ISurveyService, SurveyService>();
        services.AddTransient<IAnswerService, AnswerService>();
    }
}
