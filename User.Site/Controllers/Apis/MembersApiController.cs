using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using User.Site.Models.EFModels;
using User.Site.Models.ViewModels;

namespace User.Site.Controllers.Apis
{
    public class MembersApiController : ApiController
    {
        [HttpPost]
        [Route("api/MembersApi/Login")]
        public HttpResponseMessage Login(LoginVm vm)
        {
            using (var db = new AppDbContext()) // 確保使用你的 DbContext
            {
                var member = db.Members.FirstOrDefault(x => x.Account == vm.Account);
                if (member == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "帳號或密碼錯誤");

                //若為黑名單
                if (member.BlackList)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "帳戶異常,請聯絡客服");
                }

                if (!member.IsConfirmed)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "尚未開通，請至信箱點擊連結");
                }

                // 驗證密碼
                if (!VerifyPassword(vm.Password, member.EncryptedPassword))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "帳號或密碼錯誤");

                // 取出必要的使用者資料
                var userData = new
                {
                    account = member.Account,
                    name = member.Name,
                    profilePhoto = member.ProfilePhoto,
                    points = member.Points
                };

                // 設定 Cookies
                var response = Request.CreateResponse(HttpStatusCode.OK, userData);

                var cookie = new CookieHeaderValue("auth_token", GenerateToken(member.Account))
                {
                    HttpOnly = true,  // 防止 JavaScript 存取 Cookie，增加安全性
                    Expires = DateTimeOffset.Now.AddHours(2),  // 設定過期時間
                    Domain = Request.RequestUri.Host,  // 設定適用的域名
                    Path = "/"  // 設定適用的路徑
                };

                response.Headers.AddCookies(new CookieHeaderValue[] { cookie });

                return response;
            }
        }


        [HttpPost]
        [Route("api/MembersApi/Logout")]
        public HttpResponseMessage Logout()
        {
            // 清除 auth_token cookie
            var cookie = new CookieHeaderValue("auth_token", "")
            {
                Expires = DateTimeOffset.Now.AddDays(-1), // 將過期時間設為過去，強制瀏覽器清除 cookie
                Path = "/"
            };

            var response = Request.CreateResponse(HttpStatusCode.OK, "成功登出");
            response.Headers.AddCookies(new[] { cookie });

            return response;
        }

        private bool VerifyPassword(string password, string encryptedPassword)
        {
            // 將輸入的密碼進行 SHA 加密
            using (var sha256 = SHA256.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(inputBytes);
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashString == encryptedPassword;
            }
        }

        // 生成身份驗證的 Token，這裡使用簡單的範例，可以改為更複雜的邏輯
        private string GenerateToken(string account)
        {
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{account}:{DateTime.UtcNow}"));
            return token;
        }

    }
}