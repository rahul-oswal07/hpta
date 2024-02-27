using System.ComponentModel.DataAnnotations;

namespace HPTA.Data.Entities
{
    public class OTPRequest
    {
        public int Id { get; set; }

        public string Email { get; set; }

        [MaxLength(256)]
        public string OTPHash { get; set; }

        public DateTime CreatedOnUTC { get; set; }

        public DateTime ExpiresOnUTC { get; set; }
    }
}
