using HPTA.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace HPTA.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController(ILogger<AuthController> logger, IUserService userService) : ControllerBase
    {

        private readonly ILogger<AuthController> _logger = logger;
        private readonly IUserService _userService = userService;

        [HttpPost("claims")]
        public async Task<IActionResult> Post(TokenIssuanceStartPayload data)
        {
            _logger.LogInformation("Azure AD requesting custom claims.");

            //string requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
            //_logger.Log(LogLevel.Information, requestBody);
            //var data = JsonSerializer.Deserialize<TokenIssuanceStartPayload>(requestBody);
            string correlationId = "NA";
            try
            {
                // Read the correlation ID from the request
                correlationId = data?.Data.AuthenticationContext.CorrelationId;

            }
            catch (Exception)
            {

            }
            var r = new ResponseContent();
            // Claims to return
            var customClaims = await _userService.GetCustomClaims(data.Data.AuthenticationContext.User.Mail);
            r.Data.Actions[0].Claims = customClaims;

            return Ok(r);
        }

        public class TokenIssuanceStartPayload
        {
            public Data Data { get; set; }
            public string Source { get; set; }
            public string Type { get; set; }
        }

        public class Data
        {
            [JsonPropertyName("@odata.type")]
            public string OdataType { get; set; }
            public AuthenticationContext AuthenticationContext { get; set; }
            public string AuthenticationEventListenerId { get; set; }
            public string CustomAuthenticationExtensionId { get; set; }
            public string TenantId { get; set; }
        }

        public class AuthenticationContext
        {
            public Client Client { get; set; }
            public ServicePrincipal ClientServicePrincipal { get; set; }
            public string CorrelationId { get; set; }
            public string Protocol { get; set; }
            public ServicePrincipal ResourceServicePrincipal { get; set; }
            public RequestingUser User { get; set; }
        }

        public class Client
        {
            public string Ip { get; set; }
            public string Locale { get; set; }
            public string Market { get; set; }
        }

        public class ServicePrincipal
        {
            public string AppDisplayName { get; set; }
            public string AppId { get; set; }
            public string DisplayName { get; set; }
            public string Id { get; set; }
        }

        public class RequestingUser
        {
            public string CompanyName { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public string DisplayName { get; set; }
            public string GivenName { get; set; }
            public string Id { get; set; }
            public string Mail { get; set; }
            public string PreferredLanguage { get; set; }
            public string Surname { get; set; }
            public string UserPrincipalName { get; set; }
            public string UserType { get; set; }
        }
    }

    public class ResponseContent
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
        public ResponseContent()
        {
            Data = new Data();
        }
    }

    public class Data
    {
        [JsonPropertyName("@odata.type")]
        public string OdataType { get; set; }
        public List<Action> Actions { get; set; }
        public Data()
        {
            OdataType = "microsoft.graph.onTokenIssuanceStartResponseData";
            Actions = [new Action()];
        }
    }

    public class Action
    {
        [JsonPropertyName("@odata.type")]
        public string OdataType { get; set; }
        public CustomClaimsDTO Claims { get; set; }
        public Action()
        {
            OdataType = "microsoft.graph.tokenIssuanceStart.provideClaimsForToken";
            Claims = new CustomClaimsDTO();
        }
    }
}
