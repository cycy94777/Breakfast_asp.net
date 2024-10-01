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
    public class ResetPasswordController : ApiController
    {
        [HttpPost]
        [Route("api/MembersApi/ResetPassword")]
        public HttpResponseMessage ResetPassword([FromBody] ResetPasswordVm vm)
        {
            if (string.IsNullOrWhiteSpace(vm.NewPassword))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "密碼不能為空值" });
            }

            using (var db = new AppDbContext())
            {
                var member = db.Members.FirstOrDefault(x => x.Id == vm.MemberId);
                if (member == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "無效的會員帳號" });
                }


                // 更新密碼 (建議進行加密後存儲)
                string encryptedNewPassword = EncryptPassword(vm.NewPassword); //密碼在這裡進行加密處理
                member.EncryptedPassword = encryptedNewPassword;  
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "密碼已成功重置" });
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
