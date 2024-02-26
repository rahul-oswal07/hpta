namespace EmailClient
{
    public class EmailRecipient
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public EmailRecipient()
        {

        }

        public EmailRecipient(string email)
        {
            Email = email;
        }

        public EmailRecipient(string email, string name) : this(email)
        {
            Name = name;
        }
    }
}
