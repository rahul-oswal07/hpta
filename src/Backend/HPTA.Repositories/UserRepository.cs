using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using HPTA.Repositories.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Repositories
{
    public class UserRepository(HPTADbContext hptaDbContext) : Repository<User>(hptaDbContext), IUserRepository
    {
        public async Task<User> AddAnonymousUserIfNotExists(string name, string email)
        {
            var user = await _hptaDbContext.Users.AsNoTracking().Where(u => u.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                user = new User { Id = Guid.NewGuid().ToString("N"), Email = email, Name = name, IsActive = true };
                await _hptaDbContext.Users.AddAsync(user);
                await _hptaDbContext.SaveChangesAsync();
            }
            return user;
        }

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
