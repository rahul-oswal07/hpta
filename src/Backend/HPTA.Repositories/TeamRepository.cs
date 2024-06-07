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

        public async Task<List<UspTeamDataReturnModel>> LoadChartData(int teamId, int surveyId)
        {
            var teamIdParam = new SqlParameter("@p0", teamId);
            var surveyIdParam = new SqlParameter("@p1", surveyId);
            var teamData = await _hptaDbContext.UspTeamDataReturnModels.FromSqlRaw("exec Usp_GetTeamWiseData @p0, @p1", teamIdParam, surveyIdParam).ToListAsync();
            return teamData;
        }

        public async Task<List<UspTeamDataReturnModel>> LoadCategoryChartData(int teamId, int categoryId, int surveyId)
        {
            var teamIdParam = new SqlParameter("@p0", teamId);
            var categoryIdParam = new SqlParameter("@p1", categoryId);
            var surveyIdParam = new SqlParameter("@p2", surveyId);
            var teamData = await _hptaDbContext.UspTeamDataReturnModels.FromSqlRaw("exec Usp_GetCategoryWiseData @p0,@p1, @p2", teamIdParam, categoryIdParam, surveyIdParam).ToListAsync();
            return teamData;
        }

        public async Task<List<UspTeamDataReturnModel>> LoadUserChartData(string email, int surveyId)
        {
            var userIdParam = new SqlParameter("@p0", email);
            var surveyIdParam = new SqlParameter("@p1", surveyId);
            var teamData = await _hptaDbContext.UspTeamDataReturnModels.FromSqlRaw("exec Usp_GetUserChartData @p0, @p1", userIdParam, surveyIdParam).ToListAsync();
            return teamData;
        }

        public async Task<List<UspTeamDataReturnModel>> LoadCategoryChartDataForUser(string email, int categoryId, int surveyId)
        {
            var userIdParam = new SqlParameter("@p0", email);
            var categoryIdParam = new SqlParameter("@p1", categoryId);
            var surveyIdParam = new SqlParameter("@p2", surveyId);
            var teamData = await _hptaDbContext.UspTeamDataReturnModels.FromSqlRaw("exec Usp_GetCategoryWiseDataForUser @p0,@p1, @p2", userIdParam, categoryIdParam, surveyIdParam).ToListAsync();
            return teamData;
        }

        public async Task<List<UspTeamDataReturnModel>> LoadTeamMemberChartData(string email, int teamId, int surveyId)
        {
            var emailParam = new SqlParameter("@p0", email);
            var teamIdParam = new SqlParameter("@p1", teamId);
            var surveyIdParam = new SqlParameter("@p2", surveyId);
            var teamData = await _hptaDbContext.UspTeamDataReturnModels.FromSqlRaw("exec Usp_GetTeamMemberChartData @p0, @p1, @p2", emailParam, teamIdParam, surveyIdParam).ToListAsync();
            return teamData;
        }

        public async Task<List<UspTeamDataReturnModel>> LoadTeamMemberCategoryChartData(string email, int teamId, int categoryId, int surveyId)
        {
            var teamIdParam = new SqlParameter("@p0", teamId);
            var categoryIdParam = new SqlParameter("@p1", categoryId);
            var surveyIdParam = new SqlParameter("@p2", surveyId);
            var emailParam = new SqlParameter("@p3", email);
            var teamData = await _hptaDbContext.UspTeamDataReturnModels.FromSqlRaw("exec Usp_GetTeamMemberCategoryWiseData @p0, @p1, @p2, @p3", teamIdParam, categoryIdParam, surveyIdParam, emailParam).ToListAsync();
            return teamData;
        }
    }
}
