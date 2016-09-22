using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace WebUI.Helpers
{
    public class Services
    {
        public static void SendMessage(string subject, string message, string messageTo, string attachmentFile = "")
        {
            SmtpClient Smtp = new SmtpClient("smtp.yandex.ru", 25);
            Smtp.Credentials = new NetworkCredential("noreply@e-tiande.by", "Djkmdjc60");
            Smtp.EnableSsl = true;
            using (MailMessage Message = new MailMessage())
            {
                Message.From = new MailAddress("noreply@e-tiande.by", "e-TianDe");

                string[] listTo = messageTo.Split(',');
                foreach (string lt in listTo)
                {
                    Message.To.Add(new MailAddress(lt.Trim()));
                }

                //Message.To.Add(new MailAddress("aleksandr1.st.novikov@gmail.com"));

                Message.Subject = subject;
                Message.IsBodyHtml = true;
                Message.Body = message;

                string[] listAtt = attachmentFile.Split(',');
                foreach (string a in listAtt)
                {
                    if (a != "")
                    {
                        Attachment attach = new Attachment(a, MediaTypeNames.Application.Octet);
                        Message.Attachments.Add(attach);
                    }
                }

                //Message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                Smtp.SendAsync(Message, null);
            }
        }
    }
}