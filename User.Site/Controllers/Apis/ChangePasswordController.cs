using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using User.Site.Models.EFModels;
using User.Site.Models.ViewModels;

namespace User.Site.Controllers.Apis
{
    public class ChangePasswordController : ApiController
    {

        private const string AuthCookieName = "auth_token";
        private bool IsAuthenticated()
        {
            var cookieHeader = Request.Headers.GetCookies(AuthCookieName).FirstOrDefault();
            return cookieHeader != null && cookieHeader[AuthCookieName] != null;
        }


        [HttpPut]
        [Route("api/MembersApi/ChangePassword")]
        public HttpResponseMessage ChangePassword(ChangePasswordVm vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Account))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "帳號不能為空");
            }

            if(string.IsNullOrWhiteSpace(vm.OldPassword) || string.IsNullOrWhiteSpace(vm.NewPassword))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "密碼欄位尚未填寫" });
            }

            // Check if the user is authenticated
            if (!IsAuthenticated())
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { success = false, message = "您的登入已過期，請重新登入以繼續操作。" });
            }

            using (var db = new AppDbContext())
            {
                var member = db.Members.FirstOrDefault(x => x.Account == vm.Account);
                if (member == null) return Request.CreateResponse(HttpStatusCode.NotFound, new { success = false, message = "找不到該會員" });

                //比較舊密碼是否正確
                string encryptedOldPassword = EncryptPassword(vm.OldPassword);
                if (encryptedOldPassword != member.EncryptedPassword)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "舊密碼輸入有誤" });
                }

                //加密新密碼
                string encryptedNewPassword = EncryptPassword(vm.NewPassword);

                //將新密碼存到資料庫
                member.EncryptedPassword = encryptedNewPassword;

                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "更新密碼成功" });

            }

        }


        private string EncryptPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
