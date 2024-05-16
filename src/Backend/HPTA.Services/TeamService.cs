using AutoMapper;
using AutoMapper.QueryableExtensions;
using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace HPTA.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IOpenAIService _openAIService;
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly IAIResponseRepository _aIResponseRepository;
    private readonly ISurveyRepository _surveyRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TeamService> Logger;

    public TeamService(ITeamRepository teamRepository, IMapper mapper, IOpenAIService openAIService, IIdentityService identityService, IUserRepository userRepository,
IAIResponseRepository aIResponseRepository,
ISurveyRepository surveyRepository,
ILogger<TeamService> logger)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
        _openAIService = openAIService;
        _identityService = identityService;
        _userRepository = userRepository;
        _aIResponseRepository = aIResponseRepository;
        _surveyRepository = surveyRepository;
        Logger = logger;
    }

    public async Task<List<TeamModel>> GetAllTeams()
    {
        //ToDo: These values should be resolved using user id. Once custom claims are enabled in azure, this needs to be modified accordingly.
        var email = _identityService.GetEmail();
        var myRole = await _userRepository.GetRoleByUser(email);
        IQueryable<Team> teams;
#if DEBUG
        teams = _teamRepository.GetBy(t => t.IsActive);
#else
        if (myRole >= Common.Roles.CDL)
            teams = _teamRepository.GetBy(t => t.IsActive);
        else
            teams = _teamRepository.ListByUser(email);
#endif
        return await teams.ProjectTo<TeamModel>(_mapper.ConfigurationProvider).OrderBy(team => team.Name).ToListAsync();
    }

    public async Task<TeamDataModel> LoadChartData(int? teamId, int? surveyId)
    {
        List<UspTeamDataReturnModel> chartData = null;
        var email = _identityService.GetEmail();
        TeamPerformanceDTO performance = null;
        if (!surveyId.HasValue)
            surveyId = await _surveyRepository.GetLatestSurveyId();
        if (teamId.HasValue) // Devon user
        {
            //ToDo: These values should be resolved using user id. Once custom claims are enabled in azure, this needs to be modified accordingly.
#if !DEBUG
            if (!await _userRepository.ValidateTeamId(teamId, email))
            {
                throw new Exception("Invalid team or the user does not have access to the team.");
            } 
#endif
            chartData = await _teamRepository.LoadChartData(teamId.Value, surveyId.Value);
            if (chartData?.Count > 0)
            {
                var teamPerformance = await _aIResponseRepository.GetResponseDataForTeam(teamId.Value);
                if (teamPerformance != null)
                    performance = _mapper.Map<TeamPerformanceDTO>(teamPerformance);
            }
        }
        else // Anonymous user
        {
            chartData = await _teamRepository.LoadUserChartData(email, surveyId.Value);
            if (chartData?.Count > 0)
            {
                var userPerformance = await _aIResponseRepository.GetResponseDataForUser(email);
                if (userPerformance != null)
                    performance = _mapper.Map<TeamPerformanceDTO>(userPerformance);
            }
        }

        if (chartData == null || chartData.Count == 0)
        {
            return new TeamDataModel();
        }

        var teamData = _mapper.Map<TeamDataModel>(chartData);
        var categoryScores = teamData.Scores.ToDictionary(x => x.CategoryName, x => x.Average);
        try
        {
            performance ??= await GetAIResponse(teamId, email, categoryScores);
            teamData.TeamPerformance = performance;
        }
        catch (Exception ex)
        {
            // TO:DO : Find a better way to handle this exception
            Logger.LogError(ex, "Error while getting prompt response");
        }

        return teamData;
    }

    public async Task<TeamDataModel> LoadCategoryChartData(int? teamId, int categoryId, int? surveyId)
    {
        List<UspTeamDataReturnModel> chartData = null;
        var email = _identityService.GetEmail();
        if (!surveyId.HasValue)
            surveyId = await _surveyRepository.GetLatestSurveyId();
        if (teamId.HasValue) // Devon user
        {
            //ToDo: These values should be resolved using user id. Once custom claims are enabled in azure, this needs to be modified accordingly.
#if !DEBUG
            if (!await _userRepository.ValidateTeamId(teamId, email))
            {
                throw new Exception("Invalid team or the user does not have access to the team.");
            } 
#endif
            chartData = await _teamRepository.LoadCategoryChartData(teamId.Value, categoryId, surveyId.Value);
        }
        else // Anonymous user
        {
            chartData = await _teamRepository.LoadCategoryChartDataForUser(email, categoryId, surveyId.Value);
        }

        if (chartData == null || chartData.Count == 0)
        {
            return new TeamDataModel();
        }
        var teamData = _mapper.Map<TeamDataModel>(chartData);

        return teamData;
    }

    public async Task<List<string>> ListTeamMembers(int teamId)
    {
        return await _userRepository.GetByTeamId(teamId).Select(u => u.Name).OrderBy(name => name).ToListAsync();
    }

    private async Task<TeamPerformanceDTO> GetAIResponse(int? teamId, string userEmail, Dictionary<string, double> categoryScores)
    {
        var result = JsonSerializer.Deserialize<TeamPerformanceDTO>(await _openAIService.GetPromptResponse(categoryScores), new JsonSerializerOptions { PropertyNameCaseInsensitive = true, NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString });
        var modelData = _mapper.Map<AIResponseData>(result);
        try
        {
            if (teamId.HasValue)
            {
                await _aIResponseRepository.AddOrUpdateResponseDataForTeam(teamId.Value, modelData);
            }
            else
            {
                var userId = await _userRepository.GetUserIdByEmailAsync(userEmail);
                await _aIResponseRepository.AddOrUpdateResponseDataForUser(userId, modelData);
            }
        }
        catch
        {
        }
        return result;
    }
}
