using System.ComponentModel.DataAnnotations;

namespace HPTA.Data.Entities;

/// <summary>
/// Answer derived class in case of question is free-text based.
/// </summary>
public class FreeTextAnswer : Answer
{
    [MaxLength(100)]
    public string Text { get; set; }
}