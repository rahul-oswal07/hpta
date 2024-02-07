using HPTA.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Repositories;

public partial class HPTADbContext
{
    public DbSet<Category> Categories { get; set; }

    public DbSet<SubCategory> SubCategories { get; set; }

    public DbSet<Question> Questions { get; set; }

    public DbSet<Survey> Surveys { get; set; }

    public DbSet<SurveyQuestion> SurveyQuestions { get; set; }

    public DbSet<Answer> Answers { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Team> Teams { get; set; }

    public DbSet<UserTeam> UserTeams { get; set; }
}
