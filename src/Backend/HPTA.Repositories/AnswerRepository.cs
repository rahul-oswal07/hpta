using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Repositories;

public class AnswerRepository : Repository<Answer>, IAnswerRepository
{
    public AnswerRepository(HPTADbContext hPTADbContext) : base(hPTADbContext)
    {
    
    }

    public IQueryable<RatingAnswer> ListAnswersByUser(string email)
    {
        return _hptaDbContext.Answers.OfType<RatingAnswer>().AsNoTracking().Include(a => a.Question).ThenInclude(q => q.Question).ThenInclude(q => q.SubCategory).ThenInclude(s => s.Category).Where(a => a.User.Email == email);
    }

    public IQueryable<RatingAnswer> ListAnswersByTeamWithCategories(int surveyId, int? teamId)
    {
        var query = _hptaDbContext.Answers.OfType<RatingAnswer>().AsNoTracking().Include(r => r.Question).ThenInclude(q => q.Question).ThenInclude(q => q.SubCategory).ThenInclude(q => q.Category).Where(a => a.SurveyId == surveyId);
        if (teamId.HasValue)
            query = query.Where(a => a.User.Teams.Any(t => t.TeamId == teamId));
        return query;
    }
}
