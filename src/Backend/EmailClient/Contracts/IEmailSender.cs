namespace EmailClient.Contracts
{
    public interface IEmailSender
    {
        Task<bool> SendAsync(string subject, string body, EmailRecipient[] recipients, string[] attachments = null, bool inlineAttachments = false);

        Task<bool> SendAsync<T>(T data, string subject, string templateFile, EmailRecipient[] recipients, string[] attachments = null, bool inlineAttachments = false) where T : class;
    }
}
