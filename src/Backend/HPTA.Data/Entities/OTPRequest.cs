using System.ComponentModel.DataAnnotations;

namespace HPTA.Data.Entities
{
    public class OTPRequest
    {
        public int Id { get; set; }

        public string Email { get; set; }

        [MaxLength(6)]
        public string OTP { get; set; }

        public DateTime CreatedOnUTC { get; set; }

        public DateTime ExpiresOnUTC { get; set; }
    }
}
