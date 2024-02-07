using System.ComponentModel.DataAnnotations.Schema;

namespace HPTA.Data.Entities;

/// <summary>
/// Answer base class for question
/// </summary>
public abstract partial class Answer
{
    public int Id { get; set; }

    #region Audit Properties
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    #endregion

    #region Navigation Properties
    public int SurveyId { get; set; }

    public int QuestionNumber { get; set; }

    public SurveyQuestion Question { get; set; }

    [ForeignKey(nameof(User))]
    public string UserId { get; set; }

    public User User { get; set; }
    #endregion
}
