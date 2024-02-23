using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Repositories;

public class AnswerRepository : Repository<Answer>, IAnswerRepository
{
    private readonly HPTADbContext _hPTADbContext;
    public AnswerRepository(HPTADbContext hPTADbContext) : base(hPTADbContext)
    {
        _hPTADbContext = hPTADbContext;
    }

    public IQueryable<RatingAnswer> ListAnswersByUser(string email)
    {
        return _hPTADbContext.Answers.OfType<RatingAnswer>().AsNoTracking().Include(a => a.Question).ThenInclude(q => q.Question).ThenInclude(q => q.SubCategory).ThenInclude(s => s.Category).Where(a => a.User.Email == email);
    }
}
