using HPTA.Common.Models;
using HPTA.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HPTA.Services
{
    public class JwtTokenService(CustomJwtConfig config) : IJwtTokenService
    {
        private readonly CustomJwtConfig _config = config;

        public string GenerateToken(string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString("N")),
            new Claim("preferred_username", email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            // Add more claims as needed
        };

            var token = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_config.ExpiryDurationInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
