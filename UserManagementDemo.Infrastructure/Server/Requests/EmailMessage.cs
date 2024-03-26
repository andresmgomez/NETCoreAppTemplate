using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MimeKit;

namespace UserManagementDemo.Infrastructure.Server.Requests
{
    public class EmailMessage
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public EmailMessage(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(emailAddress => MailboxAddress.Parse(emailAddress)));
            Subject = subject;
            Content = content;
        }
    }
}
