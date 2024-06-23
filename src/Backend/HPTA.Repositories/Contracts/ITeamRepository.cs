using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Repositories.Contracts
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<List<UspTeamDataReturnModel>> LoadChartDataForTeam(int teamId, List<int> surveyId);
        IQueryable<Team> ListByUser(string email);
        Task<List<UspTeamDataReturnModel>> LoadCategoryChartDataForTeam(int teamId, int categoryId, List<int> surveyId);
        Task<List<UspTeamDataReturnModel>> LoadChartDataForTeamMember(string email, int teamId, List<int> surveyId);
        Task<List<UspTeamDataReturnModel>> LoadCategoryChartDataForTeamMember(string email, int teamId, int categoryId, List<int> surveyId);
        Task<List<UspTeamDataReturnModel>> LoadChartDataForAnonymousUser(string email, int surveyId);
        Task<List<UspTeamDataReturnModel>> LoadCategoryChartDataForAnonymousUser(string email, int categoryId, int surveyId);
    }
}
