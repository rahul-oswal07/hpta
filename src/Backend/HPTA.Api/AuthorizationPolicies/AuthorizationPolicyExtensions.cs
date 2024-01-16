using Microsoft.AspNetCore.Authorization;

namespace HPTA.Api.AuthorizationPolicies
{
    public static class AuthorizationPolicyExtensions
    {
        public static void Register(this AuthorizationOptions options, IConfiguration configuration, string key)
        {
            foreach (var keyValue in configuration.GetSection(key).GetChildren())
            {
                options.AddPolicy(keyValue.Key,
                        policy => policy.Requirements.Add(
                            new ScopesRequirement(
                                keyValue.Value))
                        );
            }
        }
    }
}
