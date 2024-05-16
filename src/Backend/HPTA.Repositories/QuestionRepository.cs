using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Repositories;

public class QuestionRepository : Repository<Question>, IQuestionRepository
{
    private readonly HPTADbContext _hPTADbContext;
    public QuestionRepository(HPTADbContext hPTADbContext) : base(hPTADbContext)
    {
        _hPTADbContext = hPTADbContext;
    }

    public async Task<List<int>> ListQuestionIds()
    {
        return await _hPTADbContext.Questions.AsNoTracking().Select(q => q.Id).ToListAsync();
    }

    public IQueryable<Question> ListWithCategories()
    {
        return _hPTADbContext.Questions.Include(q => q.SubCategory).ThenInclude(q => q.Category);
    }
}
