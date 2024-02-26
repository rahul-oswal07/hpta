namespace HPTA.DTO
{
    public class EmailRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class OtpRequestDTO : EmailRequest
    {
        public string Otp { get; set; }
    }
}
