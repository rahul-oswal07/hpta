using System.Text.Json.Serialization;

namespace HPTA.DTO
{
    public class DevCentralTeamsResponse
    {
        public EmployeeInfo Employee { get; set; }
        public TeamInfo Team { get; set; }
        public int Id { get; set; }
        public string EmpId { get; set; }
        public int TeamId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCoreMember { get; set; }
        public bool IsBillable { get; set; }
        public int RoleId { get; set; }

        public class EmployeeInfo
        {
            [JsonPropertyName("EmpId")]
            public string EmployeeCode { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public int LocationId { get; set; }
            public bool IsActive { get; set; }
            public bool IsTemporary { get; set; }
            public string Location { get; set; }
            public string CockpitUserId { get; set; }
            public string CockpitRegionId { get; set; }
            public string PhoneNumber { get; set; }
            public DateTime DoB { get; set; }
            public DateTime DateOfJoin { get; set; }
            public DateTime? DateOfSeperation { get; set; }
            public string AreaOfResidence { get; set; }
            public string EmergencyContactNumber { get; set; }
            public string ExternalEmpID { get; set; }
            public int Gender { get; set; }
        }

        public class TeamInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
