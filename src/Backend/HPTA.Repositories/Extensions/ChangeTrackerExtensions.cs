using HPTA.Common;
using HPTA.Common.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HPTA.Repositories.Extensions
{
    public static class ChangeTrackerExtensions
    {
        public static void SetShadowProperties(this ChangeTracker changeTracker)
        {
            changeTracker.DetectChanges();

            var timestamp = DateTime.UtcNow;

            foreach (var entry in changeTracker.Entries())
            {
                var type = entry.Entity.GetType();
                if (typeof(IAuditable).IsAssignableFrom(type))
                {
                    var entity = (IAuditable)entry.Entity;
                    entity.UpdatedOn = timestamp;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedOn = timestamp;
                    }
                }

                if (entry.State == EntityState.Deleted && entry.Entity is ISoftDelete)
                {
                    entry.State = EntityState.Unchanged;
                    entry.Reload();
                    entry.Property(Constants.SOFT_DELETE_FIELD_NAME).CurrentValue = true;

                }
            }
        }
    }
}
