using HPTA.Common.Configurations;
using HPTA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HPTA.Repositories.Infrastructure;

public static class RepositoryDependencyRegistry
{
    public static void RetisterRepositories(this IServiceCollection services, ConnectionStrings appSettings)
    {
        services.AddDbContext<HPTADbContext>(options =>
        {
            options.UseSqlServer(appSettings.MasterDbConnectionString, migration => migration.MigrationsAssembly("HPTA.Migrations"));
        });

        services.AddDbContext<HPTADbContext>();
        services.AddTransient<IQuestionRepository, QuestionRepository>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<ISubCategoryRepository, SubCategoryRepository>();
        services.AddTransient<ISurveyRepository, SurveyRepository>();
        services.AddTransient<IAnswerRepository, AnswerRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ITeamRepository, TeamRepository>();
        services.AddTransient<IUserTeamRepository, UserTeamRepository>();
        services.AddTransient<ISurveyQuestionRepository, SurveyQuestionRepository>();
    }
}
