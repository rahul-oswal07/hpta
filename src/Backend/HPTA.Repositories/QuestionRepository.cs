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

    public IQueryable<Question> ListWithCategories()
    {
        return _hPTADbContext.Questions.Include(q => q.SubCategory).ThenInclude(q => q.Category);
    }
}
