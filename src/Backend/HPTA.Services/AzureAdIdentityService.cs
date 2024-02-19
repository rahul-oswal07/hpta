using HPTA.Api.Controllers;
using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

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
               .FirstOrDefault(c => c.Type == "preferred_username");

            return emailClaims?.Value;
        }

        // Returns the id of the user in Azure AD (GUID format)
        public string GetId()
        {
            var idClaims = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "oid");

            return idClaims?.Value;
        }

        public string GetName()
        {
            var nameClaims = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Name);

            return nameClaims?.Value;
        }

        public string GetEmployeeCode()
        {
            var idClaims = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "employeeCode");

            return idClaims?.Value;
        }

        public List<TeamRoles> GetTeamRoles()
        {
            var idClaims = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "customRoles");

            if (idClaims.Value != null)
            {
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(idClaims.Value));
                return JsonSerializer.Deserialize<List<TeamRoles>>(json);
            }
            return [];
        }
    }
}
