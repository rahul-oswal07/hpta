using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface IAnswerService
{
    Task AddAnswers(int surveyId, List<SurveyAnswerModel> answers);
}
