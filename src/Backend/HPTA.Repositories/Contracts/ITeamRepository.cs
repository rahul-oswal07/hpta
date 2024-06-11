using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Repositories.Contracts
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<List<UspTeamDataReturnModel>> LoadChartData(int teamId, int surveyId);
        IQueryable<Team> ListByUser(string email);
        Task<List<UspTeamDataReturnModel>> LoadCategoryChartData(int teamId, int categoryId, int surveyId);
        Task<List<UspTeamDataReturnModel>> LoadUserChartData(string email, int surveyId);
        Task<List<UspTeamDataReturnModel>> LoadCategoryChartDataForUser(string email, int categoryId, int surveyId);
        Task<List<UspTeamDataReturnModel>> LoadTeamMemberChartData(string email, int teamId, int surveyId);
        Task<List<UspTeamDataReturnModel>> LoadTeamMemberCategoryChartData(string email, int teamId, int categoryId, int surveyId);
    }
}
