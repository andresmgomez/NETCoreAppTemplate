using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Infrastructure.Server.Requests;

namespace TemplateRESTful.Service.Common.Email
{
    public class EmailService : IEmailService
    {
        public readonly EmailSettingsDto _emailSettings;
        public ILogger<EmailService> _logEmailRequest;

        public EmailService(IOptions<EmailSettingsDto> emailSettings,
            ILogger<EmailService> logEmailRequest)
        {
            _emailSettings = emailSettings.Value;
            _logEmailRequest = logEmailRequest;
        }

        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            var createMessage = CreateEmailMessage(emailMessage);
            await SendAsync(createMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessage emailMessage)
        {
            var onlineMessage = new MimeMessage();

            #pragma warning disable CS0618 // Type or member is obsolete
            onlineMessage.From.Add(new MailboxAddress(_emailSettings.From));
            onlineMessage.To.AddRange(emailMessage.To);
            onlineMessage.Subject = emailMessage.Subject;

            var buildEmailContent = new BodyBuilder();
            buildEmailContent.HtmlBody = emailMessage.Content;
            onlineMessage.Body = buildEmailContent.ToMessageBody();


            return onlineMessage;
        }

        private async Task SendAsync(MimeMessage emailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                    await client.SendAsync(emailMessage);
                }
                catch (Exception exception)
                {
                    _logEmailRequest.LogError(exception.Message, exception);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}