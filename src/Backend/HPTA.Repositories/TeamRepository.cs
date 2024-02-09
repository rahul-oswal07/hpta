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
        public async Task<List<UspTeamDataReturnModel>> LoadChartData(int teamId)
        {
            var teamIdParam = new SqlParameter("@p0", teamId);
            var teamData = await hptaDbContext.UspTeamDataReturnModels.FromSqlRaw("exec Usp_GetTeamWiseData @p0", teamIdParam).ToListAsync();
            return teamData;
        }
    }
}
