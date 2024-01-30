using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;

namespace HPTA.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly HPTADbContext _hPTADbContext;
    public CategoryRepository(HPTADbContext hPTADbContext) : base(hPTADbContext)
    {
        _hPTADbContext = hPTADbContext;
    }
}
