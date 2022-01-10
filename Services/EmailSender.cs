using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace ClearSoundCompany.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var sendGridKey = @"";
            return Execute(sendGridKey, subject, htmlMessage, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("office@clearsoundco.org", "Clear Sound"),
                Subject = subject
            };
            msg.AddTo(new EmailAddress(email));

            //Confirm your email
            if (subject == "Confirm your email")
            {
                msg.SetTemplateId("d-d6c3c06284b243edbc8125a78bcc28f3");
                msg.SetTemplateData(new VerificationEmailData
                {
                    Weblink = message
                });
            }
            //Reset Password
            else if (subject == "Reset Password")
            {
                msg.SetTemplateId("d-5a1b9763c8fa4875824673302c42fb83");
                msg.SetTemplateData(new VerificationEmailData
                {
                    Weblink = message
                });
            }
            else
            {
                msg.From = new EmailAddress("office@clearsoundco.org", "For Clear Sound Support");
                msg.HtmlContent = message;
                msg.PlainTextContent = message;
            }
            msg.SetClickTracking(false, false);
            return client.SendEmailAsync(msg);
        }

        private class VerificationEmailData
        {
            [JsonProperty("Weblink")]
            public string Weblink { get; set; }
        }
    }
}