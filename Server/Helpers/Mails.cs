using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Apoyo.Server.Helpers
{
    public class Mails
    {
        public static async Task<bool> sendMensaje(string titulo, string destinatario, string contenido)
        {
            bool x = false;
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.To.Add(destinatario);
                    mail.From = new MailAddress("omarcabral1@gmail.com");
                    mail.Subject = titulo;
                    mail.Body = contenido;
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential("omarcabral1@gmail.com", "Omar2306*");
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(mail);
                        x = true;
                    }
                }
            }
            catch(Exception e)
            {
                x = false;
            }
            return x;
        }
    }
}
