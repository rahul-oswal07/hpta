using HPTA.Data.Entities;

namespace HPTA.Repositories.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<string> GetUserIdByEmailAsync(string email);

        IQueryable<User> GetUserInfoWithClaims(string email);

        Task<User> AddAnonymousUserIfNotExists(string name, string email);
    }
}
