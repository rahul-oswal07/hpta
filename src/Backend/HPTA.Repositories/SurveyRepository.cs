using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Repositories;

public class SurveyRepository : Repository<Survey>, ISurveyRepository
{
    private readonly HPTADbContext _hPTADbContext;

    public SurveyRepository(HPTADbContext hPTADbContext) : base(hPTADbContext)
    {
        _hPTADbContext = hPTADbContext;
    }

    public IQueryable<Survey> GetActiveSurveys()
    {
        return _hPTADbContext.Surveys.Where(s => s.IsActive);
    }

    public async Task<int> GetLatestSurveyId()
    {
        return await _hPTADbContext.Surveys.OrderByDescending(s => s.Id).Select(s => s.Id).FirstOrDefaultAsync();
    }
}