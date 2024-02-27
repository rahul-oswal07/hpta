using HPTA.Data.Entities;

namespace HPTA.Repositories.Contracts
{
    public interface IOTPRequestRepository
    {
        Task AddOrUpdateOTPAsync(OTPRequest otpRequest);

        Task<string> GetOTPHashByEmail(string email);
    }
}
