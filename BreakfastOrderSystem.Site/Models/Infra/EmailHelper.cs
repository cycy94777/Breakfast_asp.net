using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web;

namespace BreakfastOrderSystem.Site.Models.Infra
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

        internal string GenerateResetPasswordLink(int memberId)
        {
            string baseUrl = "https://localhost:44374"; // 根據您的實際部署環境調整
            string resetPasswordLink = $"{baseUrl}/Login/ResetPassword?memberId={memberId}";
            return resetPasswordLink;
        }

        // 發送重置密碼的電子郵件
        internal void SendResetPasswordEmail(string email, string resetLink)
        {
            string subject = "重置您的密碼";
            string body = $"請點擊以下連結來重置您的密碼: <a href='{resetLink}'>重置密碼</a>";

            SendFromGmail("Christine10150193@gmail.com", email, subject, body);
        }



    }
}