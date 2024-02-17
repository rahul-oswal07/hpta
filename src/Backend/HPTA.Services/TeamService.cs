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

    public async Task<TeamDataModel> LoadChartData(int teamId)
    {
        var chartData = await _teamRepository.LoadChartData(teamId);

        if(chartData == null)
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
        var teams = await _teamRepository.GetByAsync(x => x.IsActive);
        return _mapper.Map<List<TeamModel>>(teams);
    }

}
