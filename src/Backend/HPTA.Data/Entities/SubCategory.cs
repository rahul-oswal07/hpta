using System.ComponentModel.DataAnnotations.Schema;

namespace HPTA.Data.Entities;

public partial class SubCategory
{
    public int Id { get; set; }
    public string Name { get; set; }

    #region Audit Properties
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    #endregion

    #region Navigation Properties
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }

    public Category Category { get; set; }
    public ICollection<Question> Questions { get; set; }
    #endregion
}
