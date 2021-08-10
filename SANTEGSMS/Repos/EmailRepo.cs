using MailKit.Net.Smtp;
using MimeKit;
using SANTEGSMS.IRepos;
using SANTEGSMS.Services.Email;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SANTEGSMS.DatabaseContext;
using SANTEGSMS.Utilities;
using MailKit;
using SANTEGSMS.Entities;

namespace SANTEGSMS.Repos
{
    public class EmailRepo : IEmailRepo
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly AppDbContext _context;

        public EmailRepo(EmailConfiguration emailConfig, AppDbContext context)
        {
            _emailConfig = emailConfig;
            _context = context;
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
                //bodyBuilder.HtmlBody = "<h2>SANTEG SCHOOL MANAGEMENT SYSTEM</h2>";
                bodyBuilder.TextBody = message.Content;

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
                emailMessage.To.Add(message.To);
                emailMessage.Subject = "SANTEG SCHOOL MANAGEMENT SYSTEM";
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

                return emailMessage;
            }
            catch (Exception ex)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(ex);
                _context.ErrorLog.AddAsync(logError);
                _context.SaveChangesAsync();
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

                    if (client.IsConnected && client.IsAuthenticated && client.IsSecure)
                    {
                        //activityLog
                        var activitylog = new ActivityLogs()
                        {
                            UserId = "Mail Client Sender",
                            FirstName = "IsConnected: "+ client.IsConnected.ToString() +"; IsAuthenticated: "+ client.IsAuthenticated.ToString() + "; IsSecured: " + client.IsSecure.ToString(),
                            LastName = "TimeOut: "+ client.Timeout.ToString(),
                            Action = "Mail Client Sender",
                            Message = "Secured, Connected and Authenticated",
                            Description = "Mail Sent Successfully",
                            ActionDate = DateTime.Now,
                        };

                        _context.ActivityLogs.AddAsync(activitylog);
                        _context.SaveChangesAsync();
                    }
                    else
                    {
                        //activityLog
                        var activitylog = new ActivityLogs()
                        {
                            UserId = "Mail Client Sender",
                            FirstName = "IsConnected: " + client.IsConnected.ToString() + "; IsAuthenticated: " + client.IsAuthenticated.ToString() + "; IsSecured: " + client.IsSecure.ToString(),
                            LastName = "TimeOut: " + client.Timeout.ToString(),
                            Action = "Mail Client Sender",
                            Message = "Not Secured or Not Connected or not Authenticated",
                            Description = "Mail Failed to Send",
                            ActionDate = DateTime.Now,
                        };

                        _context.ActivityLogs.AddAsync(activitylog);
                        _context.SaveChangesAsync();
                    }
                }
                catch (ServiceNotConnectedException ex)
                {
                    //log an error message or throw an exception or both.
                    ErrorLogger err = new ErrorLogger();
                    var logError = err.logError(ex);
                    _context.ErrorLog.AddAsync(logError);
                    _context.SaveChangesAsync();
                }
                catch (ServiceNotAuthenticatedException ex)
                {
                    //log an error message or throw an exception or both.
                    ErrorLogger err = new ErrorLogger();
                    var logError = err.logError(ex);
                    _context.ErrorLog.AddAsync(logError);
                    _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    //log an error message or throw an exception or both.
                    ErrorLogger err = new ErrorLogger();
                    var logError = err.logError(ex);
                    _context.ErrorLog.AddAsync(logError);
                    _context.SaveChangesAsync();
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
