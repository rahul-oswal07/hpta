using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface IAnswerService
{
    Task AddAnswers(List<SurveyAnswerModel> answers);

    Task UpdateAIRecommendations(int surveyId, int? teamId, string userId);
}
