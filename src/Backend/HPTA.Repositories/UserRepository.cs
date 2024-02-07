using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Repositories
{
    public class UserRepository(HPTADbContext hptaDbContext) : Repository<User>(hptaDbContext), IUserRepository
    {
        public async Task<string> GetUserIdByAzureAdUserIdAsync(string azureAdUserId)
        {
            return await _hptaDbContext.Users.Where(u => u.AzureAdUserId == azureAdUserId).Select(u => u.Id).FirstOrDefaultAsync();
        }
    }
}
