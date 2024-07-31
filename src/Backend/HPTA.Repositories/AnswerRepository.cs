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

    public IQueryable<RatingAnswer> ListAnswersByUser(string userId)
    {
        return _hptaDbContext.Answers.OfType<RatingAnswer>().AsNoTracking().Include(a => a.Question).ThenInclude(q => q.Question).ThenInclude(q => q.SubCategory).ThenInclude(s => s.Category).Where(a => a.UserId == userId);
    }

    public IQueryable<RatingAnswer> ListAnswersByTeamWithCategories(int surveyId, int? teamId)
    {
        var query = _hptaDbContext.Answers.OfType<RatingAnswer>().AsNoTracking().Include(r => r.Question).ThenInclude(q => q.Question).ThenInclude(q => q.SubCategory).ThenInclude(q => q.Category).Where(a => a.SurveyId == surveyId);
        if (teamId.HasValue)
            query = query.Where(a => a.User.Teams.Any(t => t.TeamId == teamId));
        return query;
    }

    public async Task<DateTime?> GetLastUpdatedDateTime(int surveyId, int? teamId, int? userId)
    {
        var query = _hptaDbContext.Answers.Where(a => a.SurveyId == surveyId);
        if(teamId.HasValue)
            query = query.Where(q=> q.User.Teams.Any(t=>t.TeamId == teamId));
        return await query.OrderByDescending(q=> q.CreatedOn).Select(q=>q.CreatedOn).FirstOrDefaultAsync();
    }
}
