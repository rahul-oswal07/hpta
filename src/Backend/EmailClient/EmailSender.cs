using EmailClient.Contracts;
using HPTA.Common.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace EmailClient
{
    public class EmailSender : IEmailSender
    {
        private readonly ILiquidTemplateService _liquidTemplateService;
        private readonly EmailClientConfig _clientConfig;

        public EmailSender(ILiquidTemplateService liquidTemplateService, EmailClientConfig clientConfig)
        {
            _liquidTemplateService = liquidTemplateService;
            _clientConfig = clientConfig;
        }
        public async Task<bool> SendAsync(string subject, string body, EmailRecipient[] recipients, string[] attachments = null, bool inlineAttachments = false)
        {
            var builder = BuildBody(body, attachments, inlineAttachments);
            return await SendAsync(subject, recipients, builder);
        }

        public async Task<bool> SendAsync<T>(T data, string subject, string templateFile, EmailRecipient[] recipients, string[] attachments = null, bool inlineAttachments = false) where T : class
        {
            var body = await _liquidTemplateService.RegisterType(typeof(T)).RenderAsync(templateFile, data);
            return await SendAsync(subject, body, recipients, attachments, inlineAttachments);
        }

        private static BodyBuilder BuildBody(string htmlBody, string[] attachments, bool inlineAttachments = false)
        {
            var builder = new BodyBuilder
            {
                TextBody = Regex.Replace(htmlBody, "<[^>]+?>", string.Empty)
            };
            if (attachments != null)
            {
                if (inlineAttachments)
                {
                    foreach (var item in attachments)
                    {
                        var image = builder.LinkedResources.Add(item);
                        image.ContentId = Path.GetFileName(item);
                    }
                }
                else
                {
                    foreach (var item in attachments)
                    {
                        _ = builder.Attachments.Add(item);
                    }
                }
            }
            builder.HtmlBody = htmlBody;
            return builder;
        }

        private async Task<bool> SendAsync(string subject, EmailRecipient[] recipients, BodyBuilder builder)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_clientConfig.SenderName, _clientConfig.SenderEmailAddress));
            message.Subject = subject;
            foreach (var item in recipients)
            {
                message.To.Add(new MailboxAddress(item.Name, item.Email));
            }
            message.Body = builder.ToMessageBody();
            return await SendAsync(message);
        }

        private async Task<bool> SendAsync(MimeMessage message)
        {
            using var client = new SmtpClient();
            client.Authenticated += Client_Authenticated;
            client.Connected += Client_Connected;
            client.Disconnected += Client_Disconnected;
            client.MessageSent += Client_MessageSent;
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
             _clientConfig.UserName = "forevermufc92@gmail.com";
            _clientConfig.Password = "trzb csne rcoj twar";
            _clientConfig.SMTPServer = "smtp.gmail.com";
            _clientConfig.SMTPPort = 587;
            await client.ConnectAsync(_clientConfig.SMTPServer, _clientConfig.SMTPPort, SecureSocketOptions.StartTls);
            //await client.ConnectAsync(_clientConfig.SMTPServer, _clientConfig.SMTPPort, string.IsNullOrEmpty(_clientConfig.SecureSocketOptions) ?
            //    SecureSocketOptions.StartTlsWhenAvailable : (SecureSocketOptions)Enum.Parse(typeof(SecureSocketOptions), _clientConfig.SecureSocketOptions));
            if (_clientConfig.SMTPServer != "127.0.0.1" && _clientConfig.UserName != null && _clientConfig.Password != null)
            {
                await client.AuthenticateAsync(_clientConfig.UserName, _clientConfig.Password);
            }

            _ = await client.SendAsync(message);

            client.Disconnect(true);
            return true;
        }

        private void Client_MessageSent(object sender, MailKit.MessageSentEventArgs e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.Response);
        }

        private void Client_Disconnected(object sender, MailKit.DisconnectedEventArgs e)
        {
            Console.WriteLine($"Disconnected from {e.Host} at {e.Port} with {e.Options}");
        }

        private void Client_Connected(object sender, MailKit.ConnectedEventArgs e)
        {
            Console.WriteLine($"Connected to {e.Host} at {e.Port} with {e.Options}");
        }

        private void Client_Authenticated(object sender, MailKit.AuthenticatedEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        // A simple call back function:
        private void OnMailSent(object sender, AsyncCompletedEventArgs e)
        {
            if (e.UserState != null)
            {
                Console.WriteLine(e.UserState.ToString());
            }

            if (e.Error != null)
            {
                Console.WriteLine(e.Error);
            }
            else if (!e.Cancelled)
            {
                Console.WriteLine("Send successfull!");
            }
        }
    }
}
