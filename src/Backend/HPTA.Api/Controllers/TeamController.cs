using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HPTA.Api.Controllers;

[Route("api/[controller]")]
public class TeamController : BaseController
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet("team-chart/{teamId}")]
    public async Task<ActionResult> LoadTeamChartData(int teamId)
        => Ok(await _teamService.LoadChartData(teamId: teamId, userId: string.Empty));

    [HttpGet("user-chart/{userId}")]
    public async Task<ActionResult> LoadUserChartData(string userId)
        => Ok(await _teamService.LoadChartData(teamId: 0, userId: userId));

    [HttpGet]
    public async Task<ActionResult> GetAllTeams()
         => Ok(await _teamService.GetAllTeams());

    [HttpGet("category-chart/{teamId}/{categoryId}")]
    public async Task<ActionResult> LoadCategoryChartData(int teamId, int categoryId)
        => Ok(await _teamService.LoadCategoryChartData(teamId: teamId, categoryId: categoryId, userId: string.Empty));

    [HttpGet("result/{teamId?}")]
    [Authorize(AuthenticationSchemes = $"{AuthenticationSchemes.CustomJwt},{AuthenticationSchemes.AzureAD}")]
    public async Task<ActionResult> LoadChartData(int? teamId)
        => Ok(await _teamService.LoadChartData(teamId));


    [HttpGet("result-category/{teamId?}/{categoryId}")]
    public async Task<ActionResult> LoadCategoryChartData(int? teamId, int categoryId)
            => Ok(await _teamService.LoadCategoryChartData(teamId: teamId, categoryId: categoryId));
}
