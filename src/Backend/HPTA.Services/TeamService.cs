﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories;
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
    private readonly IUserTeamRepository _userTeamRepository;
    private readonly IAnswerRepository answerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TeamService> Logger;

    public TeamService(ITeamRepository teamRepository, IMapper mapper, IOpenAIService openAIService, IIdentityService identityService, IUserRepository userRepository,
IAIResponseRepository aIResponseRepository,
ISurveyRepository surveyRepository,
IUserTeamRepository userTeamRepository,
IAnswerRepository answerRepository,
ILogger<TeamService> logger)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
        _openAIService = openAIService;
        _identityService = identityService;
        _userRepository = userRepository;
        _aIResponseRepository = aIResponseRepository;
        _surveyRepository = surveyRepository;
        this._userTeamRepository = userTeamRepository;
        this.answerRepository = answerRepository;
        Logger = logger;
    }

    public async Task<List<TeamModel>> GetAllTeams()
    {
        //ToDo: These values should be resolved using user id. Once custom claims are enabled in azure, this needs to be modified accordingly.
        var email = _identityService.GetEmail();
        var myRole = await _userRepository.GetRoleByUser(email);
        var user = await _userRepository.GetUserByEmail(email);

        IQueryable<Team> teams;
        if (myRole >= Common.Roles.CDL || user.HasSpecialPrivilege.GetValueOrDefault())
            teams = _teamRepository.GetBy(t => t.IsActive);
        else
            teams = _teamRepository.ListByUser(email);
        return await teams.ProjectTo<TeamModel>(_mapper.ConfigurationProvider).OrderBy(team => team.Name).ToListAsync();
    }

    public async Task<TeamDataModel> LoadChartData(int? teamId, ChartDataRequestModel chartDataRequest)
    {
        List<UspTeamDataReturnModel> chartData = null;
        if (chartDataRequest.SurveyId == null || !chartDataRequest.SurveyId.Any())
            chartDataRequest.SurveyId = [await _surveyRepository.GetLatestSurveyId()];
        var userEmail = _identityService.GetEmail();
        var user = await _userRepository.GetUserByEmail(userEmail);

        if (teamId.HasValue) // Devon user
        {
            //ToDo: These values should be resolved using user id. Once custom claims are enabled in azure, this needs to be modified accordingly.
            if (!user.HasSpecialPrivilege.GetValueOrDefault() && !await _userRepository.ValidateTeamId(teamId, userEmail))
            {
                throw new Exception("Invalid team or the user does not have access to the team.");
            }

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
        return teamData;
    }

    public async Task<TeamDataModel> LoadCategoryChartData(int? teamId, int categoryId, ChartDataRequestModel chartDataRequest)
    {
        var userEmail = _identityService.GetEmail();
        var user = await _userRepository.GetUserByEmail(userEmail);

        List<UspTeamDataReturnModel> chartData = null;
        if (chartDataRequest.SurveyId == null || !chartDataRequest.SurveyId.Any())
            chartDataRequest.SurveyId = [await _surveyRepository.GetLatestSurveyId()];

        if (teamId.HasValue) // Devon user
        {
            //ToDo: These values should be resolved using user id. Once custom claims are enabled in azure, this needs to be modified accordingly.
            if (!user.HasSpecialPrivilege.GetValueOrDefault() && !await _userRepository.ValidateTeamId(teamId, _identityService.GetEmail()))
            {
                throw new Exception("Invalid team or the user does not have access to the team.");
            }

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
        var user = await _userRepository.GetUserByEmail(email);

        if (role >= Common.Roles.ScrumMaster || user.HasSpecialPrivilege.GetValueOrDefault())
            return await _userRepository.GetByTeamId(teamId).Select(u => new TeamMemberModel
            {
                Email = u.Email,
                Name = u.Name
            }).OrderBy(x => x.Name).ToListAsync();
        return new List<TeamMemberModel>();
    }

    public async Task<Dictionary<int, TeamPerformanceDTO>> GetPerformanceData(int? teamId, ChartDataRequestModel chartDataRequest)
    {
        var result = new Dictionary<int, TeamPerformanceDTO>();
        var email = _identityService.GetEmail();
        var user = await _userRepository.GetUserByEmail(email);

        if (teamId.HasValue && !user.HasSpecialPrivilege.GetValueOrDefault() && !await _userRepository.ValidateTeamId(teamId, email))
        {
            throw new Exception("Invalid team or the user does not have access to the team.");
        }

        foreach (var surveyId in chartDataRequest.SurveyId)
        {
            if (teamId.HasValue) // Devon user
            {
                var teamPerformance = await _aIResponseRepository.GetResponseDataForTeam(teamId.Value, surveyId);
                var lastUpdatedDateTime = await answerRepository.GetLastUpdatedDateTime(surveyId, teamId, null);
                var mappedTeamPerformance = teamPerformance == null ? new TeamPerformanceDTO() : _mapper.Map<TeamPerformanceDTO>(teamPerformance);
                mappedTeamPerformance.AssessmentDateTime = lastUpdatedDateTime;
                result.Add(surveyId, mappedTeamPerformance);
            }
            else // Anonymous user
            {
                var userPerformance = await _aIResponseRepository.GetResponseDataForUser(email, surveyId);
                var lastUpdatedDateTime = await answerRepository.GetLastUpdatedDateTime(surveyId, teamId, null);
                var mappedUserPerformance = userPerformance == null ? new TeamPerformanceDTO() : _mapper.Map<TeamPerformanceDTO>(userPerformance);
                mappedUserPerformance.AssessmentDateTime = lastUpdatedDateTime;
                result.Add(surveyId, mappedUserPerformance);

            }
        }
        return result;
    }

    public async Task<int?> GetCoreTeamId()
    {
        var email = _identityService.GetEmail();
        var userId = await _userRepository.GetUserIdByEmailAsync(email);
        return await _userTeamRepository.GetCoreTeamIdByUserId(userId);
    }
}
