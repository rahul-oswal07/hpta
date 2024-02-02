using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface ISurveyService
{
    Task<List<SurveyQuestionModel>> GetSurveyQuestions();
}
