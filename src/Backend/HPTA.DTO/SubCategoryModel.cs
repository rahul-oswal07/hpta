using System.ComponentModel.DataAnnotations;

namespace HPTA.DTO
{
    public class SubCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class SubCategoryEditModel
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Must select category.")]
        public int CategoryId { get; set; }
    }
}
