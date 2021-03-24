using MailKit.Net.Smtp;
using MimeKit;
using SANTEGSMS.IRepos;
using SANTEGSMS.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Repos
{
    public class EmailRepo : IEmailRepo
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailRepo(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            try
            {

                //var bodyBuilder = new BodyBuilder();
                //bodyBuilder.HtmlBody = "<b>This is some html text</b>";
                //bodyBuilder.TextBody = "This is some plain text";

                //message.Body = bodyBuilder.ToMessageBody();

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<h2>SANTEG</h2>";
                bodyBuilder.TextBody = message.Content;

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
                emailMessage.To.Add(message.To);
                emailMessage.Subject = "SANTEG Registration";
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

                return emailMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch(Exception ex)
                {
                    //log an error message or throw an exception or both.
                    throw ex;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
