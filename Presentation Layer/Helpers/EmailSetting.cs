
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;

namespace Presentation_Layer.Helpers
{
    public class EmailSetting : IMailService
    {
        private readonly MailSettings _options;
        public EmailSetting(IOptions<MailSettings> options)
        {
            _options = options.Value;
        }
       
        public bool SendEmail(Email email)
        {
            try
            {
                var mail = new MimeMessage();

                mail.Subject = email.Subject;

                mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));

                mail.To.Add(MailboxAddress.Parse(email.To));

                var bodyBuilder = new BodyBuilder(); // can hold any data ( text-video-audio-etc)

                bodyBuilder.TextBody = email.Body;
                mail.Body = bodyBuilder.ToMessageBody();

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(_options.Host, _options.Port, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_options.Email, _options.Password);

                smtp.Send(mail);

                return true;
            } catch(Exception ex)
            {
                return false;
            }
        }
    }
}
