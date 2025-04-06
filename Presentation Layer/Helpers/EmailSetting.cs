
using System.Net.Mail;

namespace Presentation_Layer.Helpers
{
    public class EmailSetting
    {
        public static bool SendEmail(Email email)
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential("ziadalimohammed812@gmail.com", "kwflxxatrzciwhcj");
                client.Send("ziadalimohammed812@gmail.com", email.To, email.Subject, email.Body);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
