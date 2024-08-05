using EmailClient.Contracts;
using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HPTA.Api.Controllers
{
    [Route("api/[controller]")]
    public class SendEmailController(IRabbitMQProducerService emailService ) : Controller
    {
        private readonly IRabbitMQProducerService _emailService = emailService;

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail()
        {
            //var emailData = new EmailNotificationDTO { Email = request.Email, Name = request.Name , SurveyLink = "https://synergy.prowareness.nl/synergy/" };
            //await _Ra.SendAsync(emailData, "HPTA Survey", "Notification", [new EmailRecipient(request.Email, request.Name)]);
           await  _emailService.SendMessage();
            return Ok();
        }
    }
}
