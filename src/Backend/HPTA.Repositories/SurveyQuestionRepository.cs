using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Repositories
{
    public class SurveyQuestionRepository(HPTADbContext dbContext) : ISurveyQuestionRepository
    {
        private readonly HPTADbContext _dbContext = dbContext;

        public async Task AddRangeAsync(List<SurveyQuestion> surveyQuestions)
        {
            await _dbContext.AddRangeAsync(surveyQuestions);
        }

        public IQueryable<SurveyQuestion> ListQuestionsBySurveyId(int surveyId)
        {
            return _dbContext.SurveyQuestions.Include(q => q.Question).Where(q => q.SurveyId == surveyId);
        }

        public IQueryable<SurveyQuestion> ListQuestionsForActiveSurvey()
        {
            return _dbContext.SurveyQuestions.Include(q => q.Question).Where(q => q.Survey.IsActive);
        }
    }
}
