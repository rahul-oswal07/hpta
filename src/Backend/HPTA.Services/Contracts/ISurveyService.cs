using HPTA.Common;
using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface ISurveyService
{
    Task<List<SurveyQuestionModel>> GetSurveyQuestions();

    Task<List<ListItem>> ListSurveys();

    Task CreateSurvey();
}
