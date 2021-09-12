using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Neodenit.DialogAssistant.Shared;
using Neodenit.DialogAssistant.Shared.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Neodenit.DialogAssistant
{
    public class EmailSender : IEmailSender
    {
        private readonly ISettings settings;

        public EmailSender(ISettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var apiKey = Environment.GetEnvironmentVariable(Constants.SendGridKey);

            return Execute(apiKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage
            {
                From = new EmailAddress(settings.EmailAddress, settings.EmailSenderName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(email));

            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
