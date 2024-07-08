using HPTA.DTO;
using HPTA.Scheduler;
using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HPTA.Api.Controllers;

[Route("api/[controller]")]
public class TeamController : BaseController
{
    private readonly ITeamService _teamService;
    private readonly IAnswerService _answerService;
    private readonly IIdentityService identityService;

    public TeamController(ITeamService teamService, IAnswerService answerService, IIdentityService identityService)
    {
        _teamService = teamService;
        this._answerService = answerService;
        this.identityService = identityService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllTeams()
         => Ok(await _teamService.GetAllTeams());

    [HttpPost("result/{teamId?}")]
    [Authorize(AuthenticationSchemes = $"{AuthenticationSchemes.CustomJwt},{AuthenticationSchemes.AzureAD}")]
    public async Task<ActionResult> LoadChartData1(int? teamId, ChartDataRequestModel chartDataRequest)
        => Ok(await _teamService.LoadChartData(teamId, chartDataRequest));

    [HttpPost("result-category/{categoryId}/{teamId?}")]
    [Authorize(AuthenticationSchemes = $"{AuthenticationSchemes.CustomJwt},{AuthenticationSchemes.AzureAD}")]
    public async Task<ActionResult> LoadCategoryChartData(int categoryId, int? teamId, ChartDataRequestModel chartDataRequest)
            => Ok(await _teamService.LoadCategoryChartData(teamId: teamId, categoryId: categoryId, chartDataRequest));

    [HttpGet("{teamId}/members")]
    public async Task<ActionResult> ListTeamMembers(int teamId) => Ok(await _teamService.ListTeamMembers(teamId));

    [HttpGet("coreteamid")]
    public async Task<ActionResult> GetCoreTeamId() => Ok(await _teamService.GetCoreTeamId());

    [HttpPost("performance/{teamId?}")]
    public async Task<ActionResult> GetPerformanceData(int? teamId, ChartDataRequestModel chartDataRequest)
    {
        
        if (chartDataRequest.SurveyId.Any(s => AITaskManager.IsJobRunning(s, teamId, chartDataRequest.Email)))
        {
            return Ok(new { Message= "in_progress" });
        }
        var result = await _teamService.GetPerformanceData(teamId, chartDataRequest);
        bool inProgress = false;
        foreach (var item in chartDataRequest.SurveyId)
        {
            if (!result.ContainsKey(item) || result[item] == null)
            {
                if (string.IsNullOrEmpty(chartDataRequest.Email))
                    chartDataRequest.Email = identityService.GetEmail();
                AITaskManager.Enqueue(item, teamId, chartDataRequest.Email);
                inProgress = true;
            }
        }
        if (inProgress)
            return Ok(new { Message = "in_progress" });
        return Ok(result);
    }
}
