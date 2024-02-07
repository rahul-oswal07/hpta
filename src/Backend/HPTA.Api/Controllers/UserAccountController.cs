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
            await Task.FromResult(0);
            //if (string.IsNullOrEmpty(code))
            //{
            //    // Handle the error scenario where the code is not present.
            //    return View("Error");
            //}
            //            var tokenResponse = await new HttpClient().PostAsync("https://login.microsoftonline.com/organizations/oauth2/v2.0/token", new FormUrlEncodedContent(new Dictionary<string, string>
            //{
            //    {"client_id", _configuration["AzureAd:ClientId"]},
            //    {"scope", "user.read openid profile offline_access"},
            //    {"code", code},
            //    {"redirect_uri", "/"},
            //    {"grant_type", "authorization_code"},
            //    {"client_secret", _configuration["AzureAd:ClientSecret"]},
            //}));
            //            if (tokenResponse.IsSuccessStatusCode)
            //            {
            //                var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
            //                var tokenData = JsonConvert.DeserializeObject<dynamic>(tokenJson);

            //                var accessToken = tokenData.access_token;

            //                // Validate the access token.

            //                // Optionally, get user info from Microsoft Graph.

            //                // Create a session or cookie for the user.
            //            }
            //            else
            //            {
            //                // Handle token acquisition failure.
            //                return View("Error");
            //            }
            return Ok();
        }
    }
}
