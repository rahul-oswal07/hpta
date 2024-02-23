using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Repositories
{
    public class TeamRepository(HPTADbContext hptaDbContext) : Repository<Team>(hptaDbContext), ITeamRepository
    {
        public IQueryable<Team> ListByUser(string email)
        {
            return _hptaDbContext.UserTeams.AsNoTracking().Include(u => u.Team).Where(t => t.User.IsActive && t.User.Email == email && t.IsCoreMember && t.StartDate <= DateTime.Today && t.EndDate >= DateTime.Today).Select(ut => ut.Team);
        }

        public async Task<List<UspTeamDataReturnModel>> LoadChartData(int teamId)
        {
            var teamIdParam = new SqlParameter("@p0", teamId);
            var teamData = await hptaDbContext.UspTeamDataReturnModels.FromSqlRaw("exec Usp_GetTeamWiseData @p0", teamIdParam).ToListAsync();
            return teamData;
        }
    }
}
