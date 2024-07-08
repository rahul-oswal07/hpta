using HPTA.Common.Configurations;
using HPTA.DTO;
using HPTA.Services.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace HPTA.Services;

public class OpenAIService : IOpenAIService
{
    private readonly ApplicationSettings _connectionStrings;
    public OpenAIService(ApplicationSettings connectionStrings)
    {
        _connectionStrings = connectionStrings;
    }

    public async Task<string> GetPromptResponse(IEnumerable<AIRequestCategoryDTO> scores)
    {
        var prompt = GetPrompt(scores);
        string json = JsonConvert.SerializeObject(new PromptModel { UserInput = prompt });
        string aiResponse = await GetAIResponse(json);
        string response = ReplaceHtmlTags(aiResponse);

        return response;
    }

    static string GetPrompt(IEnumerable<AIRequestCategoryDTO> scores)
    {
        StringBuilder promptBuilder = new StringBuilder(@"I am conducting a survey on high-performing agile teams. I will provide the scores divided into categories and subcategories in a JSON format. I would like the response to be in the format below, with explanations for each field provided.");

        foreach (var category in scores)
        {
            promptBuilder.Append($"\n{category.CategoryName}:");
            foreach (var score in category.Scores)
            {
                promptBuilder.Append($" [{score.SubCategoryName} : {score.Score}]");
            }
        }

        promptBuilder.Append("\n" + GetStringFromJSonFile());

        return promptBuilder.ToString();
    }

    private async Task<string> GetAIResponse(string json)
    {
        try
        {
            using var client = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_connectionStrings.OpenAIUrl, content);
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static string GetStringFromJSonFile()
    {
        // TODO : Find a better way to create a prompt
        var path = File.ReadAllText("prompt.json");
        return path;
    }

    private static string ReplaceHtmlTags(string input)
    {
        return input.Replace("<br>", "\r\n").Replace("</br>", "\r\n");
    }
}
