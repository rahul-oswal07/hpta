using HPTA.Data.Entities;
using HPTA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HPTA.Repositories
{
    public class OTPRequestRepository : IOTPRequestRepository
    {
        private readonly HPTADbContext _dbContext;

        public OTPRequestRepository(HPTADbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddOrUpdateOTPAsync(OTPRequest otpRequest)
        {
            var existing = await _dbContext.OTPRequests.Where(r => r.Email == otpRequest.Email).FirstOrDefaultAsync();
            if (existing == null)
                await _dbContext.OTPRequests.AddAsync(otpRequest);
            else
            {
                existing.OTPHash = otpRequest.OTPHash;
                existing.CreatedOnUTC = otpRequest.CreatedOnUTC;
                existing.ExpiresOnUTC = otpRequest.ExpiresOnUTC;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GetOTPHashByEmail(string email)
        {
            return await _dbContext.OTPRequests.Where(o => o.Email == email && o.CreatedOnUTC < DateTime.UtcNow && o.ExpiresOnUTC > DateTime.UtcNow).Select(o=>o.OTPHash).FirstOrDefaultAsync();
        }
    }
}
