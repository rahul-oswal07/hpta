using HPTA.Data.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace HPTA.Repositories
{
    public partial class HPTADbContext(DbContextOptions options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetDefaultFieldSize(modelBuilder, 50);
            base.OnModelCreating(modelBuilder);
        }

        private static void SetDefaultFieldSize(ModelBuilder builder, int size)
        {
            foreach (IMutableProperty property in builder.Model.GetEntityTypes()
               .SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(string) && p.PropertyInfo != null && !p.PropertyInfo.GetCustomAttributes().Any(c => c is IsMaxStringLengthAttribute)))
            {
                if (!property.GetMaxLength().HasValue)
                    property.SetMaxLength(size);
            }
        }
    }
}
