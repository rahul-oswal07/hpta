using HPTA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HPTA.Data.Configurations
{
    internal class UserTeamConfiguration : IEntityTypeConfiguration<UserTeam>
    {
        public void Configure(EntityTypeBuilder<UserTeam> builder)
        {
            builder.Property(ut => ut.Id).ValueGeneratedNever();
            //builder.HasIndex(p => new { p.UserId, p.TeamId, p.StartDate, p.RoleId }).IsUnique().HasFilter("[IsDeleted] = 0");
        }
    }
}
