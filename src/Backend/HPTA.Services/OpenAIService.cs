using HPTA.Common.Configurations;
using HPTA.DTO;
using HPTA.Services.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace HPTA.Services;

public class OpenAIService : IOpenAIService
{
    private readonly ConnectionStrings _connectionStrings;
    public OpenAIService(ConnectionStrings connectionStrings)
    {
        _connectionStrings = connectionStrings;
    }

    public async Task<string> GetPromptResponse(Dictionary<string, double> scores)
    {
        var prompt = GetPrompt(scores);
        string json = JsonConvert.SerializeObject(new PromptModel { UserInput = prompt });
        string aiResponse = await GetAIResponse(json);
        return aiResponse;
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

    private async Task<string> GetAIResponse(string json)
    {
        using var client = new HttpClient();
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(_connectionStrings.OpenAIUrl, content);
        return await response.Content.ReadAsStringAsync();
    }
}
