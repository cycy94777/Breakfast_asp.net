using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web;

namespace User.Site.Helpers
{
    public class EmailHelper
    {
        public virtual void SendFromGmail(string from, string to, string subject, string body)
        {
            var smtpAccount = from;

            var smtpPassword = "eqmp gbac rkpm kveh";

            var smtpServer = "smtp.gmail.com";
            var SmtpPort = 587;

            var mms = new MailMessage
            {
                From = new MailAddress(smtpAccount),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                SubjectEncoding = Encoding.UTF8
            };

            mms.To.Add(new MailAddress(to));

            using (var client = new SmtpClient(smtpServer, SmtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(smtpAccount, smtpPassword); // 寄信帳密
                client.Send(mms); // 寄出信件
            }
        }
    }
}