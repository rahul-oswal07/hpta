using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;

namespace HPTA.Repositories;

public class QuestionRepository : Repository<Question>, IQuestionRepository
{
    private readonly HPTADbContext _hPTADbContext;
    public QuestionRepository(HPTADbContext hPTADbContext) : base(hPTADbContext)
    {
        _hPTADbContext = hPTADbContext;
    }
}
