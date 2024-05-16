using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface IAnswerService
{
    Task AddAnswers(List<SurveyAnswerModel> answers);
}
