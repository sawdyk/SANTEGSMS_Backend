using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Services.Email
{
    public class EmailMessage
    {
        public MailboxAddress To { get; set; }
        //public string Subject { get; set; }
        public string Content { get; set; }

        public EmailMessage(string to, string content)
        {
            //To = new List<MailboxAddress>();
            //To.AddRange(to.Select(x => new MailboxAddress(x)));
            To = new MailboxAddress(to);
            //Subject = subject;
            Content = content;
        }
    }
}
