using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface ITeamService
{
    Task<SurveyResultDataModel> LoadChartData(int? teamId);

    Task<List<TeamModel>> GetAllTeams();
}
