namespace HPTA.DTO
{
    public class AIRequestCategoryDTO
    {
        public string CategoryName { get; set; }

        public List<AIRequestSubCategoryDTO> Scores { get; set; }
    }

    public class AIRequestSubCategoryDTO
    {
        public string SubCategoryName { get; set; }

        public double Score { get; set; }
    }
}
