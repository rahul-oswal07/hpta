using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Repositories.Contracts
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<List<UspTeamDataReturnModel>> LoadChartData(int teamId);
        IQueryable<Team> ListByUser(string email);
        Task<List<UspTeamDataReturnModel>> LoadCategoryChartData(int teamId, int categoryId);
        Task<List<UspTeamDataReturnModel>> LoadUserChartData(string userId);
        Task<List<UspTeamDataReturnModel>> LoadCategoryChartDataForUser(string userId, int categoryId);
    }
}
