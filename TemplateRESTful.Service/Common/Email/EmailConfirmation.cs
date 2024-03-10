using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Infrastructure.Server.Requests;

namespace TemplateRESTful.Service.Common.Email
{
    public static class EmailConfirmation
    {
        public static EmailMessage SetEmailContent(ApplicationUser userAccount, string accessCode)
        {
            var emailMessage = new EmailMessage(new string[]
               {
                   userAccount.Email
               },
               "RestfulAPI - Login Your Account with Two Factor Authentication",
               $@"<h4>Hello, {userAccount.Email}</h4>
                    <p>We have recently detected that you try to login to your account as an administrator.</p>
                    <p>Copy and paste the following authorization code to continue the login process.</p>
                    <h4>{accessCode}</h4>
                    <p>Otherwise, ignore this email report it to your network administrator.</p>"
            );

            return emailMessage;
        }
    }
}
