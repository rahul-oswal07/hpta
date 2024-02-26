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
                existing.OTP = otpRequest.OTP;
                existing.CreatedOnUTC = otpRequest.CreatedOnUTC;
                existing.ExpiresOnUTC = otpRequest.ExpiresOnUTC;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<OTPRequest> GetOTPByEmail(string email)
        {
            return await _dbContext.OTPRequests.AsNoTracking().Where(r => r.Email == email).FirstOrDefaultAsync();
        }

        public async Task<bool> ValidateOTP(string email, string otp)
        {
            return await _dbContext.OTPRequests.AnyAsync(o => o.Email == email && o.OTP == otp && o.CreatedOnUTC < DateTime.UtcNow && o.ExpiresOnUTC > DateTime.UtcNow);
        }
    }
}
