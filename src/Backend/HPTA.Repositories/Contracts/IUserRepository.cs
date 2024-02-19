using HPTA.Data.Entities;

namespace HPTA.Repositories.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<string> GetUserIdByAzureAdUserIdAsync(string azureAdUserId);
        Task<string> GetUserIdByEmailAsync(string email);

        IQueryable<User> GetUserInfoWithClaims(string email);
    }
}
