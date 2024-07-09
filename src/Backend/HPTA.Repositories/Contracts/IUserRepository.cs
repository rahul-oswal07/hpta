using HPTA.Common;
using HPTA.Data.Entities;

namespace HPTA.Repositories.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<string> GetUserIdByEmailAsync(string email);

        Task<Roles> GetRoleByUser(string email);

        IQueryable<User> GetUserInfoWithClaims(string email);

        IQueryable<User> GetByTeamId(int teamId);
        Task<User> AddAnonymousUserIfNotExists(string name, string email);
        Task<bool> ValidateTeamId(int? teamId, string email);
        Task<User> GetUserByEmail(string email);
    }
}
