using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace WhereToPlay.Models
{
    internal static class Utilities
    {
        internal static void  SmsSend (String smsTo,String smsBody)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add("online.programare@gmail.com");
            mail.From = new MailAddress("online.programare@gmail.com");
            mail.Subject = smsTo;
            mail.Body = smsBody;
            mail.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            ("online.programare", "parolaprogramare1");// Enter seders User name and password
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        internal static void EmailSend(String emailTo, String emailSubject, String emailBody)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(emailTo);
            mail.From = new MailAddress("online.programare@gmail.com");
            mail.Subject = emailSubject;
            mail.Body = emailBody;
            mail.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            ("online.programare", "parolaprogramare1");// Enter seders User name and password
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}