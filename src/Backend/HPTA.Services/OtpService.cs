using HPTA.Common.Configurations;
using HPTA.Common.Models;
using HPTA.Data.Entities;
using HPTA.DTO;
using HPTA.Repositories.Contracts;
using HPTA.Services.Contracts;

namespace HPTA.Services
{
    public class OtpService(IOTPRequestRepository oTPRequestRepository, IUserRepository userRepository, IHashingService hashingService, int validity) : IOtpService
    {
        private readonly IOTPRequestRepository _oTPRequestRepository = oTPRequestRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<string> GenerateOtp(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            var request = new OTPRequest
            {
                Email = email,
                CreatedOnUTC = DateTime.UtcNow,
                ExpiresOnUTC = DateTime.UtcNow.AddMinutes(validity),
                OTPHash =hashingService.GenerateHash(otp)
            };
            await _oTPRequestRepository.AddOrUpdateOTPAsync(request);
            return otp;
        }

        public async Task<bool> ValidateOtp(OtpRequestDTO otpRequest)
        {
            var hash = await _oTPRequestRepository.GetOTPHashByEmail(otpRequest.Email);
            var isValid = hash != null && hashingService.VerifyHash(otpRequest.Otp, hash);
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
