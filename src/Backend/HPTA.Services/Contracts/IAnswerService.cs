using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface IAnswerService
{
    Task<(int, int?, string)> AddAnswers(List<SurveyAnswerModel> answers);

    Task UpdateAIResponse(int? teamId, string email, int surveyId, string jobId);
}
