using HPTA.Common;
using HPTA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HPTA.Repositories.Infrastructure;

public static class RepositoryDependencyRegistry
{
    public static void DependencyRegistry(this IServiceCollection services, ConnectionStrings appSettings)
    {
        services.AddDbContext<HPTADbContext>(options =>
        {
            options.UseSqlServer(appSettings.MasterDbConnectionString, migration => migration.MigrationsAssembly("HPTA.Migrations"));
        });

        services.AddDbContext<HPTADbContext>();
        services.AddTransient<IQuestionRepository, QuestionRepository>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<ISurveyRepository, SurveyRepository>();
        services.AddTransient<IAnswerRepository, AnswerRepository>();
    }
}
