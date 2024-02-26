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
        public string GetEmail() => GetClaimValueByType("preferred_username")?.Value;

        // Returns the id of the user in Azure AD (GUID format)
        public string GetId() => GetClaimValueByType("oid")?.Value;

        public string GetName() => GetClaimValueByType(ClaimTypes.Name)?.Value;

        public string GetEmployeeCode() => GetClaimValueByType("employeeCode")?.Value;

        public string GetHPTAUserId() => GetClaimValueByType("hptaUserId")?.Value;

        public int? GetCoreTeamId()
        {
            var result = GetClaimValueByType("coreTeamId")?.Value;
            if (result == null)
                return null;
            return Convert.ToInt32(result);
        }

        public bool IsSuperUser()
        {
            var result = GetClaimValueByType("isSuperUser")?.Value;
            if (result == null)
                return false;
            return Convert.ToBoolean(result);
        }

        private Claim GetClaimValueByType(string type)
        {
            return _httpContextAccessor.HttpContext.User.Claims
                            .FirstOrDefault(c => c.Type == type);
        }
    }
}
