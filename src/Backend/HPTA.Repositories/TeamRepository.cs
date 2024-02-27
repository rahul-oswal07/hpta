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
            var teamData = await _hptaDbContext.UspTeamDataReturnModels.FromSqlRaw("exec Usp_GetTeamWiseData @p0", teamIdParam).ToListAsync();
            return teamData;
        }

        public async Task<List<UspTeamDataReturnModel>> LoadCategoryChartData(int teamId, int categoryId)
        {
            var teamIdParam = new SqlParameter("@p0", teamId);
            var categoryIdParam = new SqlParameter("@p1", categoryId);
            var teamData = await _hptaDbContext.UspTeamDataReturnModels.FromSqlRaw("exec Usp_GetCategoryWiseData @p0,@p1", teamIdParam, categoryIdParam).ToListAsync();
            return teamData;
        }

        public async Task<List<UspTeamDataReturnModel>> LoadUserChartData(string userId)
        {
            var userIdParam = new SqlParameter("@p0", userId);
            var teamData = await _hptaDbContext.UspTeamDataReturnModels.FromSqlRaw("exec Usp_GetUserChartData @p0", userIdParam).ToListAsync();
            return teamData;
        }

        public async Task<List<UspTeamDataReturnModel>> LoadCategoryChartDataForUser(string userId, int categoryId)
        {
            var userIdParam = new SqlParameter("@p0", userId);
            var categoryIdParam = new SqlParameter("@p1", categoryId);
            var teamData = await _hptaDbContext.UspTeamDataReturnModels.FromSqlRaw("exec Usp_GetCategoryWiseDataForUser @p0,@p1", userIdParam, categoryIdParam).ToListAsync();
            return teamData;
        }
    }
}
