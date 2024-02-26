using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Repositories.Contracts
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<List<UspTeamDataReturnModel>> LoadChartData(int teamId);

        IQueryable<Team> ListByUser(string email);
    }
}
