using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;

namespace HPTA.Repositories;

public class AnswerRepository : Repository<Answer>, IAnswerRepository
{
    private readonly HPTADbContext _hPTADbContext;
    public AnswerRepository(HPTADbContext hPTADbContext) : base(hPTADbContext)
    {
        _hPTADbContext = hPTADbContext;
    }
}
