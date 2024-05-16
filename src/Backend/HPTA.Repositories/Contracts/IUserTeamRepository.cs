using HPTA.Data.Entities;

namespace HPTA.Repositories.Contracts
{
    public interface IUserTeamRepository : IRepository<UserTeam>
    {
        Task<int?> GetCoreTeamIdByUserId(string userId);
    }
}
