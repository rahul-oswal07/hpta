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

    public async Task<TeamDataModel> LoadChartData(int? teamId, ChartDataRequestModel chartDataRequest)
    {
        List<UspTeamDataReturnModel> chartData = null;
        if (chartDataRequest.SurveyId == null || !chartDataRequest.SurveyId.Any())
            chartDataRequest.SurveyId = [await _surveyRepository.GetLatestSurveyId()];
        var userEmail = _identityService.GetEmail();
        if (teamId.HasValue) // Devon user
        {
            //ToDo: These values should be resolved using user id. Once custom claims are enabled in azure, this needs to be modified accordingly.
#if !DEBUG
            if (!await _userRepository.ValidateTeamId(teamId, userEmail))
            {
                throw new Exception("Invalid team or the user does not have access to the team.");
            }
#endif

            if (!string.IsNullOrWhiteSpace(chartDataRequest.Email))
            {
                chartData = await _teamRepository.LoadChartDataForTeamMember(chartDataRequest.Email, teamId.Value, chartDataRequest.SurveyId);
            }
            else
            {
                chartData = await _teamRepository.LoadChartDataForTeam(teamId.Value, chartDataRequest.SurveyId);
            }
        }
        else // Anonymous user
        {
            chartData = await _teamRepository.LoadChartDataForAnonymousUser(userEmail, chartDataRequest.SurveyId.FirstOrDefault());
        }

        if (chartData == null || chartData.Count == 0)
        {
            return new TeamDataModel();
        }

        var teamData = _mapper.Map<TeamDataModel>(chartData);
        //var categoryScores = teamData.Scores.ToDictionary(x => x.CategoryName, x => x.Average);
        try
        {
            await SetPerformanceData(teamId, userEmail, chartDataRequest, teamData);
        }
        catch (Exception ex)
        {
            // TO:DO : Find a better way to handle this exception
            Logger.LogError(ex, "Error while getting prompt response");
        }

        return teamData;
    }

    public async Task<TeamDataModel> LoadCategoryChartData(int? teamId, int categoryId, ChartDataRequestModel chartDataRequest)
    {
        List<UspTeamDataReturnModel> chartData = null;
        if (chartDataRequest.SurveyId == null || !chartDataRequest.SurveyId.Any())
            chartDataRequest.SurveyId = [await _surveyRepository.GetLatestSurveyId()];

        if (teamId.HasValue) // Devon user
        {
            //ToDo: These values should be resolved using user id. Once custom claims are enabled in azure, this needs to be modified accordingly.
#if !DEBUG
            if (!await _userRepository.ValidateTeamId(teamId, string.IsNullOrEmpty(chartDataRequest.Email) ? _identityService.GetEmail() : chartDataRequest.Email))
            {
                throw new Exception("Invalid team or the user does not have access to the team.");
            }
#endif
            if (!string.IsNullOrWhiteSpace(chartDataRequest.Email))
            {
                chartData = await _teamRepository.LoadCategoryChartDataForTeamMember(chartDataRequest.Email, teamId.Value, categoryId, chartDataRequest.SurveyId);
            }
            else
            {
                chartData = await _teamRepository.LoadCategoryChartDataForTeam(teamId.Value, categoryId, chartDataRequest.SurveyId);
            }
        }
        else // Anonymous user
        {
            chartDataRequest.Email = _identityService.GetEmail();
            chartData = await _teamRepository.LoadCategoryChartDataForAnonymousUser(chartDataRequest.Email, categoryId, chartDataRequest.SurveyId.FirstOrDefault());
        }

        if (chartData == null || chartData.Count == 0)
        {
            return new TeamDataModel();
        }
        var teamData = _mapper.Map<TeamDataModel>(chartData);

        return teamData;
    }

    public async Task<List<TeamMemberModel>> ListTeamMembers(int teamId)
    {
        var email = _identityService.GetEmail();
        var role = await _userRepository.GetRoleByUser(email);

        if (role >= Common.Roles.ScrumMaster)
            return await _userRepository.GetByTeamId(teamId).Select(u => new TeamMemberModel
            {
                Email = u.Email,
                Name = u.Name
            }).OrderBy(x => x.Name).ToListAsync();
        return new List<TeamMemberModel>();
    }

    private async Task<TeamPerformanceDTO> GetAIResponse(int surveyId, int? teamId, string userEmail, Dictionary<string, double> categoryScores)
    {
        var result = JsonSerializer.Deserialize<TeamPerformanceDTO>(await _openAIService.GetPromptResponse(categoryScores), new JsonSerializerOptions { PropertyNameCaseInsensitive = true, NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString });
        var modelData = _mapper.Map<AIResponseData>(result);
        try
        {
            if (teamId.HasValue)
            {
                await _aIResponseRepository.AddOrUpdateResponseDataForTeam(teamId.Value, surveyId, modelData);
            }
            else
            {
                var userId = await _userRepository.GetUserIdByEmailAsync(userEmail);
                await _aIResponseRepository.AddOrUpdateResponseDataForUser(userId, surveyId, modelData);
            }
        }
        catch
        {
        }
        return result;
    }

    private async Task SetPerformanceData(int? teamId, string userEmail, ChartDataRequestModel chartDataRequest, TeamDataModel teamData)
    {
        foreach (var survey in teamData.SurveyResults)
        {
            if (teamId.HasValue) // Devon user
            {
                if (!string.IsNullOrWhiteSpace(chartDataRequest.Email))
                {
                    var userPerformance = await _aIResponseRepository.GetResponseDataForUser(chartDataRequest.Email, survey.SurveyId, teamId);
                    if (userPerformance != null)
                        survey.TeamPerformance = _mapper.Map<TeamPerformanceDTO>(userPerformance);
                }
                else
                {
                    var teamPerformance = await _aIResponseRepository.GetResponseDataForTeam(teamId.Value, survey.SurveyId);
                    if (teamPerformance != null)
                        survey.TeamPerformance = _mapper.Map<TeamPerformanceDTO>(teamPerformance);
                }
            }
            else // Anonymous user
            {
                var userPerformance = await _aIResponseRepository.GetResponseDataForUser(userEmail, survey.SurveyId);
                if (userPerformance != null)
                    survey.TeamPerformance = _mapper.Map<TeamPerformanceDTO>(userPerformance);
            }
        }
    }
}
