using System.ComponentModel.DataAnnotations.Schema;

namespace HPTA.Data.Entities
{
    public partial class UserTeam
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCoreMember { get; set; }
        public bool IsBillable { get; set; }
        public int RoleId { get; set; }

        #region Navigation Properties
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public User User { get; set; }

        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; }

        public Team Team { get; set; }
        #endregion

        #region Audit Properties
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        #endregion

    }
}
