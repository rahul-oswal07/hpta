using HPTA.Data.Entities;

namespace HPTA.Repositories.Contracts;

public interface ISurveyRepository : IRepository<Survey>
{
    Task<int> GetLatestSurveyId();

    IQueryable<Survey> GetActiveSurveys();
}
