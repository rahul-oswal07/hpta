using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;

namespace HPTA.Repositories
{
    internal class SubCategoryRepository : Repository<SubCategory>, ISubCategoryRepository
    {
        private readonly HPTADbContext _hPTADbContext;
        public SubCategoryRepository(HPTADbContext hPTADbContext) : base(hPTADbContext)
        {
            _hPTADbContext = hPTADbContext;
        }
    }
}