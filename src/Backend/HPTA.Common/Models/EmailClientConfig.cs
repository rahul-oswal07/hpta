namespace HPTA.Common.Models
{
    public class EmailClientConfig
    {
        public string SMTPServer { get; set; }

        public string SenderEmailAddress { get; set; }

        public string SenderName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int SMTPPort { get; set; }

        public bool EnableSSL { get; set; }

        public string SecureSocketOptions { get; set; }
    }
}
