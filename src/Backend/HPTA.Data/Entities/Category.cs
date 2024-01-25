namespace HPTA.Data.Entities
{
    public partial class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        #region Audit Properties
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        #endregion

        #region Navigation Properties
        public ICollection<SubCategory> SubCategories { get; set; }
        #endregion
    }
}
