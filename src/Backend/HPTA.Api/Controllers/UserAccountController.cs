using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HPTA.Api.Controllers
{
    [Route("api/user")]
    public class PostLoginController(IUserService userService) : BaseController
    {
        private readonly IUserService _userService = userService;

        [Route("config")]
        [HttpPost]
        public async Task<IActionResult> IndexAsync()
        {
            await _userService.ImportFromDevCentral();
            return Ok();
        }
    }
}
