using Microsoft.Extensions.DependencyInjection;

namespace HPTA.Mapping.Infrastructure
{
    public static class MappingProfileRegistry
    {
        public static void RegisterMappingProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfileRegistry).Assembly);
        }
    }
}
