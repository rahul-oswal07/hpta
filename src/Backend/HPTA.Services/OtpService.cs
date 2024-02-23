using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;

namespace HPTA.Services
{
    public class OtpService : IOtpService
    {
        private readonly IOTPRequestRepository _oTPRequestRepository;
        private readonly IUserRepository _userRepository;

        public OtpService(IOTPRequestRepository oTPRequestRepository, IUserRepository userRepository)
        {
            _oTPRequestRepository = oTPRequestRepository;
            _userRepository = userRepository;
        }

        public async Task<string> GenerateOtp(string email)
        {
            var request = new OTPRequest
            {
                Email = email,
                CreatedOnUTC = DateTime.UtcNow,
                ExpiresOnUTC = DateTime.UtcNow.AddMinutes(10),
                OTP = new Random().Next(100000, 999999).ToString() // Simple 6 digit OTP
            };
            await _oTPRequestRepository.AddOrUpdateOTPAsync(request);
            return request.OTP;
        }

        public async Task<bool> ValidateOtp(OtpRequestDTO otpRequest)
        {
            var isValid = await _oTPRequestRepository.ValidateOTP(otpRequest.Email, otpRequest.Otp);
            if (isValid)
            {
                var user = await _userRepository.AddAnonymousUserIfNotExists(otpRequest.Name, otpRequest.Email);
                if (user.EmployeeCode != null)
                    throw new Exception("Devon user cannot be logged in as anonymous user.");
            }
            return isValid;
        }
    }
}
