using HPTA.Data.Entities;

namespace HPTA.Repositories.Contracts;

public interface IAnswerRepository : IRepository<Answer>
{
    IQueryable<RatingAnswer> ListAnswersByUser(string userId);

    IQueryable<RatingAnswer> ListAnswersByTeamWithCategories(int surveyId, int? teamId);

    Task<DateTime?> GetLastUpdatedDateTime(int surveyId, int? teamId, int? userId);
}
