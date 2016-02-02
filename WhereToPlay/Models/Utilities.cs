using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace WhereToPlay.Models
{
    public static class Utilities
    {
        public static void  SmsSend (String smsTo,String smsBody)
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

        public static void EmailSend(String emailTo, String emailSubject, String emailBody)
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

        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static LoggedStatus GetLoggedStatus(string userName)
        {
            LoggedStatus status = new LoggedStatus();
            WhereToPlay.Models.WhereToPlayDb db = new WhereToPlay.Models.WhereToPlayDb();
            WhereToPlay.Models.DB.User loggedUser = db.Users.Where(u => u.UserName == HttpContext.Current.User.Identity.Name).FirstOrDefault();
            if (loggedUser == null)
            {
                status = LoggedStatus.NotLogged;
            }
            else
            {
                WhereToPlay.Models.DB.UserGroup UsrGroup = db.UserGroups.Where(u => u.IDUserGroup == loggedUser.UserGroupID).FirstOrDefault();
                switch (UsrGroup.UserGroupName)
                {
                    case "Proprietar":
                        status = LoggedStatus.LoggedOwner;
                        break;
                    case "Administrator":
                        status = LoggedStatus.LoggedAdmin;
                        break;
                    case "Jucator":
                        status = LoggedStatus.LoggedPlayer;
                        break;
                    default:
                        status = LoggedStatus.NotLogged;
                        break;
                }
            }
            return status;
        }
    }


    public enum LoggedStatus
    {
        NotLogged = 1,
        LoggedAdmin = 2,
        LoggedOwner = 3,
        LoggedPlayer = 4
    }
}