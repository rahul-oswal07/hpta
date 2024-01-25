using System.ComponentModel.DataAnnotations;

namespace HPTA.Data.Entities;

public partial class Survey
{
    public int Id { get; set; }

    [MaxLength(200)]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; }

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    #region Audit Properties
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    #endregion

    #region Navigation Properties
    public ICollection<SurveyQuestion> Questions { get; set; }

    public ICollection<Answer> Answers { get; set; }
    #endregion
}
