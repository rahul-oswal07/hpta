using System.ComponentModel.DataAnnotations;

namespace HPTA.Data.Entities;

public partial class User
{
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string EmployeeCode { get; set; }

    [Required]
    public string Email { get; set; }

    public bool IsActive { get; set; }
    public bool IsTemporary { get; set; }

    public DateTime? DoB { get; set; }
    public DateTime? DateOfJoin { get; set; }
    public DateTime? DateOfSeperation { get; set; }

    public string AzureAdUserId { get; set; }

    public bool? HasSpecialPrivilege { get; set; }

    #region Audit Properties
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    #endregion

    #region Navigation Properties
    public ICollection<UserTeam> Teams { get; set; } = new List<UserTeam>();
    #endregion
}
