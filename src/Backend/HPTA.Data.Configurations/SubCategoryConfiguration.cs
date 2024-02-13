using HPTA.Data.Configurations.Helpers;
using HPTA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HPTA.Data.Configurations
{
    internal class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
#if DEBUG

            var data = SeedHelper.SeedData<SubCategory>("sub-categories.json");
            builder.HasData(data);
#endif
        }
    }
}
