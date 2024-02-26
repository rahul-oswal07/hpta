using System.Text.Json.Serialization;

namespace HPTA.Api.Controllers
{
    public class CustomClaimsDTO
    {
        [JsonPropertyName("employeeCode")]
        public string EmployeeCode { get; set; }

        [JsonPropertyName("hptaUserId")]
        public string HPTAUserId { get; set; }

        public int? CoreTeamId { get; set; }

        public bool IsSuperUser { get; set; }
    }
}
