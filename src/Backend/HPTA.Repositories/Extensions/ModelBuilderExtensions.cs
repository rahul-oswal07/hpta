using HPTA.Common.Constraints;
using HPTA.Data.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace HPTA.Repositories.Extensions;

internal static class ModelBuilderExtensions
{

    /// <summary>
    /// Sets the shadow properties to entities
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void SetShadowProperties(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var type = entityType.ClrType;

            // set soft delete property
            if (typeof(ISoftDelete).IsAssignableFrom(type))
            {
                modelBuilder.Entity(type).Property<bool>(Constants.SOFT_DELETE_FIELD_NAME).HasDefaultValue(false);
            }
        }
    }

    /// <summary>
    /// Sets the default field size for the string fields of entities. If a property in entity is decorated with attribute [IsMaxStringLength], the field size is set to max.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="size"></param>
    public static void SetDefaultFieldSize(this ModelBuilder builder, int size)
    {
        foreach (IMutableProperty property in builder.Model.GetEntityTypes()
           .SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(string) && p.PropertyInfo != null && !p.PropertyInfo.GetCustomAttributes().Any(c => c is IsMaxStringLengthAttribute)))
        {
            if (!property.GetMaxLength().HasValue)
                property.SetMaxLength(size);
        }
    }

    /// <summary>
    /// Sets the global query filters for soft delete.
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void SetGlobalQueryFilters(this ModelBuilder modelBuilder)
    {

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var type = entityType.ClrType;

            // set global filters
            if (typeof(ISoftDelete).IsAssignableFrom(type))
            {
                // softdeletable
                var method = SetGlobalQueryForSoftDeleteMethodInfo.MakeGenericMethod(type);
                method.Invoke(modelBuilder, [modelBuilder]);
            }
        }
    }

    /// <summary>
    /// Sets the global query filters for soft delete by type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder"></param>
    public static void SetGlobalQueryForSoftDelete<T>(this ModelBuilder builder) where T : class, ISoftDelete
    {
        builder.Entity<T>().HasQueryFilter(item => EF.Property<bool>(item, Constants.SOFT_DELETE_FIELD_NAME) != true);
    }

    private static readonly MethodInfo SetGlobalQueryForSoftDeleteMethodInfo = typeof(ModelBuilderExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(t => t.IsGenericMethod && t.Name == nameof(SetGlobalQueryForSoftDelete));
}
