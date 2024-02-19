using HPTA.Api.Controllers;

namespace HPTA.Services.Contracts
{
    public interface IUserService
    {
        Task<CustomClaimsDTO> GetCustomClaims(string email);

        Task SyncAllUsersAsync();
    }
}
