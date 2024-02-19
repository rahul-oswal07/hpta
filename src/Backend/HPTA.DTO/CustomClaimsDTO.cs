using HPTA.Common;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HPTA.Api.Controllers
{
    public class CustomClaimsDTO
    {
        private List<TeamRoles> _teamRoles;
        private string _customRoles;

        public string EmployeeCode { get; set; }

        [JsonIgnore]
        public List<TeamRoles> TeamRoles
        {
            get => _teamRoles; set
            {
                _teamRoles = value;
                string json = JsonSerializer.Serialize(value);
                _customRoles = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            }
        }

        public string CustomRoles => _customRoles;
        public CustomClaimsDTO()
        {
            TeamRoles = [];
        }
    }

    public class TeamRoles
    {
        public int TeamId { get; set; }
        public List<Roles> Roles { get; set; }
    }
}
