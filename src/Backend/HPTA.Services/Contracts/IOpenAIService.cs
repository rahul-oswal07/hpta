using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface IOpenAIService
{
    Task<string> GetPromptResponse(IEnumerable<AIRequestCategoryDTO> scores);
}