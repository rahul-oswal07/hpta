namespace HPTA.Data.Entities
{
    public partial class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        #region Audit Properties
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        #endregion

        #region Navigation Properties
        public ICollection<UserTeam> TeamUsers { get; set; }
        #endregion
    }
}
