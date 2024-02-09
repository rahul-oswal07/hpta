using HPTA.Data.Configurations;
using HPTA.DTO;
using HPTA.Repositories.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace HPTA.Repositories
{
    public partial class HPTADbContext(DbContextOptions options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetDefaultFieldSize(50);
            modelBuilder.SetShadowProperties();
            modelBuilder.SetGlobalQueryFilters();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AnswerConfiguration).Assembly);
            modelBuilder
                .Entity<UspTeamDataReturnModel>(
                    eb =>
                    {
                        eb.HasNoKey();
                    });
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<UspTeamDataReturnModel> UspTeamDataReturnModels { get; set; } = null!;

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Remove(typeof(CascadeDeleteConvention));
            configurationBuilder.Conventions.Remove(typeof(SqlServerOnDeleteConvention));
        }

        public override int SaveChanges()
        {
            ChangeTracker.SetShadowProperties();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.SetShadowProperties();
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
