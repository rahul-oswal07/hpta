using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface ITeamService
{
    Task<TeamDataModel> LoadChartData(int? teamId, ChartDataRequestModel chartDataRequest);

    Task<List<TeamModel>> GetAllTeams();

    Task<TeamDataModel> LoadCategoryChartData(int? teamId, int categoryId, ChartDataRequestModel chartDataRequest);

    Task<List<TeamMemberModel>> ListTeamMembers(int teamId);
}
