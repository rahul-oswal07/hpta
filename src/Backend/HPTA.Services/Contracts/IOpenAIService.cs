namespace HPTA.Services.Contracts;

public interface IOpenAIService
{
    Task<string> GetPromptResponse(Dictionary<string, double> scores);
}