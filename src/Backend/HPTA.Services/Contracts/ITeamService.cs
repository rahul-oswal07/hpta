using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface ITeamService
{
    Task<TeamDataModel> LoadChartData(int? teamId, int? surveyId, string email);

    Task<List<TeamModel>> GetAllTeams();

    Task<TeamDataModel> LoadCategoryChartData(int? teamId, int categoryId, int? surveyId, string email);

    Task<List<TeamMemberModel>> ListTeamMembers(int teamId);
}
