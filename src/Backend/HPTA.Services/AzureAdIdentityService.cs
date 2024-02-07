using HPTA.Common;
using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HPTA.Services
{
    public class AzureAdIdentityService(IHttpContextAccessor httpContextAccessor) : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        // Indicates if the user is authenticated
        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        // Returns the principal user login (ie. principal account mail)
        public string GetEmail()
        {
            var emailClaims = _httpContextAccessor.HttpContext.User.Claims
               .FirstOrDefault(c => c.Type == ClaimTypes.Email);

            return emailClaims?.Value;
        }

        // Returns the id of the user in Azure AD (GUID format)
        public string GetId()
        {
            var idClaims = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == AzureAdClaimTypes.ObjectId);

            return idClaims?.Value;
        }

        public string GetName()
        {
            var nameClaims = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Name);

            return nameClaims?.Value;
        }
    }
}
