using HPTA.Data.Entities;

namespace HPTA.Repositories.Contracts
{
    public interface IOTPRequestRepository
    {
        Task AddOrUpdateOTPAsync(OTPRequest otpRequest);

        Task<bool> ValidateOTP(string email, string otp);
    }
}
