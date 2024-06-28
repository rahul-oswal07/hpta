using HPTA.DTO;
using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HPTA.Api.Controllers;

[Route("api/[controller]")]
public class TeamController : BaseController
{
    private readonly ITeamService _teamService;
    private readonly IAnswerService _answerService;

    public TeamController(ITeamService teamService, IAnswerService answerService)
    {
        _teamService = teamService;
        this._answerService = answerService;
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

    [HttpPost("ai/{surveyId}")]
    public async Task<ActionResult> UpdateAIRecommentadions(int surveyId, [FromQuery]int? teamId, [FromQuery]string userId)
    {
        await _answerService.UpdateAIRecommendations(surveyId, teamId, userId);
        return Ok();
    }
}
