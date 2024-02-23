using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HPTA.Api
{
    public static class AuthenticationSchemes
    {
        public const string AzureAD = JwtBearerDefaults.AuthenticationScheme;

        public const string CustomJwt = "Bearer2";
    }
}
