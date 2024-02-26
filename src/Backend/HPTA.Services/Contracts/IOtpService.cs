using HPTA.DTO;

namespace HPTA.Services.Contracts
{
    public interface IOtpService
    {
        Task<string> GenerateOtp(string email);
        Task<bool> ValidateOtp(OtpRequestDTO otpRequest);
    }
}
