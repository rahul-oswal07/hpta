using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;

namespace HPTA.Repositories
{
    public class UserTeamRepository(HPTADbContext hptaDbContext) : Repository<UserTeam>(hptaDbContext), IUserTeamRepository
    {
    }
}
