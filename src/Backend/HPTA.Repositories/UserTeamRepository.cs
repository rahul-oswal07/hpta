using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Repositories
{
    public class UserTeamRepository(HPTADbContext hptaDbContext) : Repository<UserTeam>(hptaDbContext), IUserTeamRepository
    {
        public async Task<int?> GetCoreTeamIdByUserId(string userId)
        {
            return await _hptaDbContext.UserTeams.Where(ut => ut.UserId == userId && ut.IsCoreMember && ut.StartDate <= DateTime.Today && ut.EndDate >= DateTime.Today).OrderByDescending(ut => ut.EndDate).Select(ut => ut.TeamId).FirstOrDefaultAsync();
        }
    }
}
