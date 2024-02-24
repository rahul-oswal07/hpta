using AutoMapper;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;

namespace HPTA.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IOpenAIService _openAIService;
    private readonly IMapper _mapper;

    public TeamService(ITeamRepository teamRepository, IMapper mapper, IOpenAIService openAIService)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
        _openAIService = openAIService;
    }

    public async Task<TeamDataModel> LoadChartData(int teamId, string userId)
    {
        List<UspTeamDataReturnModel> chartData = null;

        if (teamId > 0)
        {
            chartData = await _teamRepository.LoadChartData(teamId);
        }
        else if (!string.IsNullOrWhiteSpace(userId))
        {
            chartData = await _teamRepository.LoadUserChartData(userId);
        }

        if (chartData == null || chartData.Count == 0)
        {
            return new TeamDataModel();
        }
        var teamData = _mapper.Map<TeamDataModel>(chartData);

        var categoryScores = teamData.Scores.ToDictionary(x => x.CategoryName, x => x.Average);

        teamData.PromptData = await _openAIService.GetPromptResponse(categoryScores);

        return teamData;
    }

    public async Task<List<TeamModel>> GetAllTeams()
    {
        var teams = (await _teamRepository.GetByAsync(x => x.IsActive)).OrderBy(x => x.Name);
        return _mapper.Map<List<TeamModel>>(teams);
    }

    public async Task<TeamDataModel> LoadCategoryChartData(int teamId, int categoryId, string userId)
    {
        List<UspTeamDataReturnModel> chartData = null;

        if (teamId > 0)
        {
            chartData = await _teamRepository.LoadCategoryChartData(teamId, categoryId);
        }
        else if (!string.IsNullOrWhiteSpace(userId))
        {
            chartData = await _teamRepository.LoadCategoryChartDataForUser(userId, categoryId);
        }

        if (chartData == null || chartData.Count == 0)
        {
            return new TeamDataModel();
        }
        var teamData = _mapper.Map<TeamDataModel>(chartData);

        return teamData;
    }
}
