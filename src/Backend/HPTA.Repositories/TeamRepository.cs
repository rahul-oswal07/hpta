using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;

namespace HPTA.Repositories
{
    public class TeamRepository(HPTADbContext hptaDbContext) : Repository<Team>(hptaDbContext), ITeamRepository
    {
    }
}
