using HPTA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HPTA.Data.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(p => p.EmployeeCode).IsUnique().HasFilter($"[IsDeleted] = 0 and [{nameof(User.EmployeeCode)}] is not null"); ;
        }
    }
}
