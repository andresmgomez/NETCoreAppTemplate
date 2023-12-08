using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Entities.DTOs.Mail;

namespace TemplateRESTful.Infrastructure.Server.Requests.IRepository
{
    public interface IEmailRequest
    {
        Task SendEmailAsync(EmailMessage emailMessage);
    }
}
