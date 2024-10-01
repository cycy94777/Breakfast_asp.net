using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using User.Site.Helpers;
using User.Site.Models.EFModels;
using User.Site.Models.ViewModels;

namespace User.Site.Controllers.Apis
{
    public class ForgotPasswordController : ApiController
    {
        private readonly EmailHelper _emailHelper;

        public ForgotPasswordController()
        {
            _emailHelper = new EmailHelper(); // 初始化您的 EmailHelper
        }

        [HttpPost]
        [Route("api/MembersApi/ForgotPassword")]
        public HttpResponseMessage ForgotPassword([FromBody] ForgotPasswordVm vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Email))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "請輸入您的電子郵件" });
            }

            using (var db = new AppDbContext())
            {
                var member = db.Members.FirstOrDefault(x => x.Account == vm.Email);
                // 不管是否找到該電子郵件，均回應已成功寄送郵件
                // 如果找到，則真正發送重置密碼的郵件
                if (member != null)
                {
                    //string resetLink = GenerateResetPasswordLink(member.Account);
                    int memberId = member.Id;
                    string resetLink = GenerateResetPasswordLink(memberId);
                    SendResetPasswordEmail(vm.Email, resetLink);
                }

                // 回應的結果保持一致
                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "郵件已發送" });

            }
            
        }

        private string GenerateResetPasswordLink(int memberId)
        {
            string baseUrl = "https://localhost:44389"; // 根據您的實際部署環境調整
            string resetPasswordLink = $"{baseUrl}/Html/resetPassword.html?memberId={memberId}";
            return resetPasswordLink;
        }

        // 發送重置密碼的電子郵件
        private void SendResetPasswordEmail(string email, string resetLink)
        {
            string subject = "重置您的密碼";
            string body = $"請點擊以下連結來重置您的密碼: <a href='{resetLink}'>重置密碼</a>";

            _emailHelper.SendFromGmail("Christine10150193@gmail.com", email, subject, body);
        }
    }

    

    
}
