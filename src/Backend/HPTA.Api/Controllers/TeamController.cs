using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HPTA.Api.Controllers;

[Route("api/[controller]")]
public class TeamController : BaseController
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllTeams()
         => Ok(await _teamService.GetAllTeams());




    [HttpGet("result/{teamId?}")]
    [Authorize(AuthenticationSchemes = $"{AuthenticationSchemes.CustomJwt},{AuthenticationSchemes.AzureAD}")]
    public async Task<ActionResult> LoadChartData(int? teamId)
        => Ok(await _teamService.LoadChartData(teamId));

    [HttpGet("result-category/{teamId?}/{categoryId}")]
    public async Task<ActionResult> LoadCategoryChartData(int? teamId, int categoryId)
            => Ok(await _teamService.LoadCategoryChartData(teamId: teamId, categoryId: categoryId));

}
