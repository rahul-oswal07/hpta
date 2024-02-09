using HPTA.Common;
using System.ComponentModel.DataAnnotations;

namespace HPTA.DTO
{
    public class QuestionModel
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Text { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public QuestionAnswerType AnswerType { get; set; }

        public string SubCategoryName { get; set; }

        public string CategoryName { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class QuestionEditModel
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Text { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public QuestionAnswerType AnswerType { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Must select sub-category.")]
        public int SubCategoryId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Must select category.")]
        public int CategoryId { get; set; }
    }
}
