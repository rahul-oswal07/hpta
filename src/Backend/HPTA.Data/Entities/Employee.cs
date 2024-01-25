using System.ComponentModel.DataAnnotations;

namespace HPTA.Data.Entities;

public partial class Employee
{
    [MaxLength(128)]
    public string Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    #region Audit Properties
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    #endregion

}
