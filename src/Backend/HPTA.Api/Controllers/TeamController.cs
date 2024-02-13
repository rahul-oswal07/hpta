using HPTA.DTO;
using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HPTA.Api.Controllers;

[Route("api/[controller]")]
public class TeamController : BaseController
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet("{teamId}")]
    public async Task<ActionResult> LoadChartData(int teamId)
        => Ok(await _teamService.LoadChartData(teamId));

    [HttpGet]
    public async Task<ActionResult> GetAllTeams()
         => Ok(await _teamService.GetAllTeams());

    [HttpGet]
    [Route("GetAIResult/{teamId}")]
    public async Task<ActionResult> GetHPTAResult(int teamId)
    {
        List<UspTeamDataReturnModel> result = await _teamService.LoadChartData(teamId);
        var categoryScores = result.ToDictionary(x => x.CategoryName, x => x.Average);
        var prompt = GetPrompt(categoryScores);
        string json = JsonConvert.SerializeObject(new PromptModel { UserInput = prompt });
        string aiResponse = await MakePostRequest("https://azureopenaidevon.azurewebsites.net/api/ai/completion", json);

        return Ok(await _teamService.LoadChartData(teamId));
    }

    static string GetPrompt(Dictionary<string, double> scores)
    {
        // Append the loaded scores to the prompt
        string prompt = @"
At Devon, we kindly request our team members to complete the High-Performing Team Assessment Survey. The team has diligently submitted their responses, revealing the following scores per category on a scale of 1 to 5:";

        foreach (var kvp in scores)
        {
            prompt += $"\n{kvp.Key}: {kvp.Value}";
        }

        prompt += "\n\nAs an AI Agile Coach, your insights are invaluable. Please provide guidance on the areas the team should focus on in the upcoming months.";

        return prompt;
    }

    // create a method that will make http post request to a url and return the result in string
    public static async Task<string> MakePostRequest(string url, string json)
    {
        using var client = new HttpClient();
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);
        return await response.Content.ReadAsStringAsync();
    }
}
