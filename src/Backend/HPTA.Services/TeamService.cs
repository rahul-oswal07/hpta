using AutoMapper;
using AutoMapper.QueryableExtensions;
using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IOpenAIService _openAIService;
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public TeamService(ITeamRepository teamRepository, IMapper mapper, IOpenAIService openAIService, IIdentityService identityService, IUserRepository userRepository)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
        _openAIService = openAIService;
        _identityService = identityService;
        _userRepository = userRepository;
    }

    public async Task<List<TeamModel>> GetAllTeams()
    {
        //ToDo: These values should be resolved using user id. Once custom claims are enabled in azure, this needs to be modified accordingly.
        var email = _identityService.GetEmail();
        var myRole = await _userRepository.GetRoleByUser(email);
        IQueryable<Team> teams = _teamRepository.GetBy(x => x.IsActive);
        return await teams.ProjectTo<TeamModel>(_mapper.ConfigurationProvider).OrderBy(team => team.Name).ToListAsync();
    }

    public async Task<TeamDataModel> LoadChartData(int? teamId)
    {
        List<UspTeamDataReturnModel> chartData = null;
        var email = _identityService.GetEmail();
        if (teamId.HasValue) // Devon user
        {
            //ToDo: These values should be resolved using user id. Once custom claims are enabled in azure, this needs to be modified accordingly.
            if (!await _userRepository.ValidateTeamId(teamId, email))
            {
                throw new Exception("Invalid team or the user does not have access to the team.");
            }
            chartData = await _teamRepository.LoadChartData(teamId.Value);
        }
        else // Anonymous user
        {
            chartData = await _teamRepository.LoadUserChartData(email);
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

    public async Task<TeamDataModel> LoadCategoryChartData(int? teamId, int categoryId)
    {
        List<UspTeamDataReturnModel> chartData = null;
        var email = _identityService.GetEmail();

        if (teamId.HasValue) // Devon user
        {
            //ToDo: These values should be resolved using user id. Once custom claims are enabled in azure, this needs to be modified accordingly.
            if (!await _userRepository.ValidateTeamId(teamId, email))
            {
                throw new Exception("Invalid team or the user does not have access to the team.");
            }
            chartData = await _teamRepository.LoadCategoryChartData(teamId.Value, categoryId);
        }
        else // Anonymous user
        {
            chartData = await _teamRepository.LoadCategoryChartDataForUser(email, categoryId);
        }

        if (chartData == null || chartData.Count == 0)
        {
            return new TeamDataModel();
        }
        var teamData = _mapper.Map<TeamDataModel>(chartData);

        return teamData;
    }
}
