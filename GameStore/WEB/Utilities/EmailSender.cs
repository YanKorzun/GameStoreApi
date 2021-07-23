using System.Net;
using System.Net.Mail;

namespace GameStore.WEB.Utilities
{
    public class EmailSender
    {
        public static void SendConfirmMessage(int id, string token, string adress, string email)
        {
            //place here your own login and password
            MailAddress from = new MailAddress("mail", "Tom");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Тест";
            m.Body = @$"<h2>Для потверждения учетной записи перейдите по ссылке<br><h1><a href='{adress}?id={id}&token={token}'>Тык</a></h1></h2>";
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            //place here your own login and password
            smtp.Credentials = new NetworkCredential("mail", "password");
            smtp.Send(m);
        }
    }
}