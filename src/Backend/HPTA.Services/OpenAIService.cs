using Azure.AI.OpenAI;
using Azure;
using HPTA.Common.Configurations;
using HPTA.DTO;
using HPTA.Services.Contracts;
using Newtonsoft.Json;
using OpenAI.Chat;
using System.Text;
using System.ClientModel;
using Azure.Identity;
using static System.Environment;

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
        string aiResponse = await GetAIResponse(prompt);
        string response = ReplaceHtmlTags(aiResponse);

        return response;
    }

    static string GetPrompt(IEnumerable<AIRequestCategoryDTO> scores)
    {
        var prompt = @"You are an agile coach for a company. A team has submitted an High performing team assessment which will indicate the current status of the team. Each question in the assessment are divided into categories. You will be provided with the score for each categories and again divided into sub-categories. The score for each categories and their sub-categories are as follows:";

        foreach (var category in scores)
        {
            prompt += $"\n{category.CategoryName}-[{string.Join(",", category.Scores.Select(s => $"{s.SubCategoryName} - {s.Score:N2}"))}]";
        }

        prompt += "\n You need to provide detail explanation of the team is doing with respect to each categories in below JSON format:";
        prompt += "\n" + GetStringFromJSonFile();

        return prompt;
    }


    private async Task<string> GetAIResponse(string prompt)
    {
        try
        {
            string endpoint = "https://devongpt.openai.azure.com/"; // TO:DO - Move this to configuration
            string key = "3b15a30215b34919bc0c298a82c0ea57";        // TO:DO move this to configularation  
            ApiKeyCredential credential = new ApiKeyCredential(key);
            AzureOpenAIClient azureClient = new(new Uri(endpoint), credential);
            ChatClient chatClient = azureClient.GetChatClient("DevOnGPT");

            ChatCompletion completion = await chatClient.CompleteChatAsync(
              new ChatMessage[] {
              new SystemChatMessage(prompt),
              },
              new ChatCompletionOptions()
              {
                  MaxOutputTokenCount = 800,
                  Temperature = (float)0.7,
                  FrequencyPenalty = (float)0,
                  PresencePenalty = (float)0,
              }
            );

            return completion.Content[0].Text;
        }
        catch (Exception ex)
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
        var start = input.IndexOf('{');
        return input[start..(input.LastIndexOf('}') + 1)].Replace("<br>", "\r\n").Replace("</br>", "\r\n");
    }
}
