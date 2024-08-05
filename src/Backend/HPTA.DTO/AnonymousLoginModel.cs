namespace HPTA.DTO
{
    public class EmailRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }

    }

    public class OtpRequestDTO : EmailRequest
    {
        public string Otp { get; set; }
    }

    public class EmailNotificationDTO : EmailRequest
    {
        public string SurveyLink { get; set; }

    }
}
