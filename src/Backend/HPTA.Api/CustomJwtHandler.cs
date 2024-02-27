using HPTA.Common.Configurations;
using HPTA.Common.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace HPTA.Api
{
    public class CustomJwtHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly CustomJwtConfig _config;
        public CustomJwtHandler(
            ApplicationSettings applicationSettings,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
            _config = applicationSettings.JwtConfig;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            await Task.FromResult(0);
            // Try to get the Authorization header
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Authorization header missing.");
            }
            var header = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ");
            if (header.First() != AuthenticationSchemes.CustomJwt)
            {
                return AuthenticateResult.NoResult();
            }
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Token is missing.");
            }

            var principal = GetPrincipalFromToken(token);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.Secret);
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // Set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                if (!jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return principal;
            }
            catch
            {
                // Note: you might want to log or handle exceptions more gracefully depending on your scenario
                return null;
            }
        }
    }
}
