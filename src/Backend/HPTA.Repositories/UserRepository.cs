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

        public async Task<string> GetUserIdByEmailAsync(string email)
        {
            return await _hptaDbContext.Users.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefaultAsync();
        }

        public IQueryable<User> GetUserInfoWithClaims(string email)
        {
            return _hptaDbContext.Users.Include(u => u.Teams).Where(u => u.Email == email);
        }
    }
}
