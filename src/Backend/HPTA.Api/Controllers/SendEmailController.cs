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
           await  _emailService.SendMessage();
            return Ok();
        }
    }
}
