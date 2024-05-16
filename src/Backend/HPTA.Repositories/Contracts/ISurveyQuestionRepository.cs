using HPTA.Data.Entities;

namespace HPTA.Repositories.Contracts
{
    public interface ISurveyQuestionRepository
    {
        IQueryable<SurveyQuestion> ListQuestionsBySurveyId(int surveyId);
        IQueryable<SurveyQuestion> ListQuestionsForActiveSurvey();

        Task AddRangeAsync(List<SurveyQuestion> surveyQuestions);
    }
}
