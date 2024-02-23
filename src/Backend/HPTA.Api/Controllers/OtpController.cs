using EmailClient;
using EmailClient.Contracts;
using HPTA.DTO;
using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HPTA.Api.Controllers
{
    [Route("api/anonymous/[controller]")]
    [ApiController]
    public class OtpController(IEmailSender emailService, IOtpService otpService, IJwtTokenService jwtTokenService) : ControllerBase
    {
        private readonly IEmailSender _emailService = emailService;
        private readonly IOtpService _otpService = otpService;
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

        [HttpPost("send")]
        public async Task<IActionResult> SendOtp([FromBody] EmailRequest request)
        {
            var otp = await _otpService.GenerateOtp(request.Email);
            var emailData = new OtpRequestDTO { Email = request.Email, Name = request.Name, Otp = otp };
            await _emailService.SendAsync(emailData, "HPTA Credentials", "OTPEmail", [new EmailRecipient(request.Email, request.Name)]);
            return Ok();
        }

        [HttpPost("verify")]
        public async Task<IActionResult> ValidateOtp([FromBody] OtpRequestDTO request)
        {
            var isValid = await _otpService.ValidateOtp(request);
            if (isValid)
            {
                var token = _jwtTokenService.GenerateToken(request.Email);
                return Ok(new { Token = token });
            }
            else
            {
                return BadRequest("Invalid OTP.");
            }
        }
    }
}
