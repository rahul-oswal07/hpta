using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;

namespace HPTA.Repositories;

public class SurveyRepository : Repository<Survey>, ISurveyRepository
{
    private readonly HPTADbContext _hPTADbContext;

    public SurveyRepository(HPTADbContext hPTADbContext) : base(hPTADbContext)
    {
        _hPTADbContext = hPTADbContext;
    }
}