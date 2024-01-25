using HPTA.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HPTA.Data.Entities;

public partial class Question
{
    public int Id { get; set; }

    [MaxLength(200)]
    public string Text { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; }

    public QuestionAnswerType AnswerType { get; set; }

    #region Audit Properties
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    #endregion

    #region Navigation Properties
    [ForeignKey(nameof(SubCategory))]
    public int SubCategoryId { get; set; }

    public SubCategory SubCategory { get; set; }

    #endregion
}
