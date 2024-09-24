using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using User.Site.Helpers;
using User.Site.Models.EFModels;

namespace User.Site.Controllers.Apis
{
    public class RegisterApiController : ApiController
    {
        [HttpPost]
        [Route("api/MembersApi/Register")]
        public async Task<HttpResponseMessage> Register()
        {
            var httpRequest = HttpContext.Current.Request;

            // 取得檔案
            var postedFile = httpRequest.Files["profilePhoto"];
            string relativePath = null; // 保存相對路徑

            if (postedFile != null && postedFile.ContentLength > 0)
            {
                // 設定儲存檔案的資料夾路徑
                var profilePhotoDirectory = HttpContext.Current.Server.MapPath("~/Content/Images/profile/");
                if (!Directory.Exists(profilePhotoDirectory))
                {
                    Directory.CreateDirectory(profilePhotoDirectory); // 如果資料夾不存在，則建立
                }

                // 生成新的唯一文件名，避免文件名衝突
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(postedFile.FileName);

                // 組合完整的檔案路徑 (絕對路徑)
                var filePath = Path.Combine(profilePhotoDirectory, fileName);

                // 保存檔案到指定路徑
                postedFile.SaveAs(filePath);

                // 設定相對路徑，這樣可以在前端顯示圖片
                relativePath = "/Content/Images/profile/" + fileName;
            }



            // 創建新的會員對象，並將相對路徑存儲到 ProfilePhoto 欄位中
            var newMember = new Member
            {
                Name = httpRequest.Form["name"],
                Account = httpRequest.Form["account"],
                Phone = httpRequest.Form["phone"],
                EncryptedPassword = EncryptPassword(httpRequest.Form["password"]),
                ProfilePhoto = relativePath, // 保存相對路徑
                RegistrationDate = DateTime.Now, // 設定當前時間
                IsConfirmed = false // 默認會員未驗證
            };

            // 生成確認碼
            string confirmCode = Guid.NewGuid().ToString("N");

            // 將確認碼存入資料庫
            newMember.ConfirmCode = confirmCode;


            try
            {
                using (var db = new AppDbContext())
                {
                    // 檢查帳號是否已存在
                    var existingMember = db.Members.FirstOrDefault(x => x.Account == newMember.Account);
                    if (existingMember != null)
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "帳號已存在");

                    db.Members.Add(newMember);
                    await db.SaveChangesAsync();
                }


                // 假設您的應用程式運行在 localhost，並且使用 https
                string baseUrl = "https://localhost:44389"; // 根據您的實際域名或地址進行調整
                string activationLink = $"{baseUrl}/Html/Activation.html?memberId={newMember.Id}&confirmCode={confirmCode}";

                // 構建郵件內容
                string subject = "註冊成功!";
                string body = $"歡迎加入會員！請點擊以下連結以確認您的帳戶：<br/>" +
                              $"<a href='{activationLink}'>請點擊這裡</a>";

                // 發送郵件
                EmailHelper emailHelper = new EmailHelper();
                emailHelper.SendFromGmail("christine10150193@gmail.com", newMember.Account, subject, body);


                return Request.CreateResponse(HttpStatusCode.OK, "註冊成功");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "註冊失敗: " + ex.Message);
            }
        }


        // 啟動帳號
        [HttpGet]
        [Route("api/MembersApi/ActiveRegister")]
        public async Task<HttpResponseMessage> ActiveRegister(int memberId, string confirmCode)
        {
            // 檢查參數是否有效
            if (string.IsNullOrEmpty(confirmCode))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "無效的確認連結");
            }

            using (var db = new AppDbContext())
            {
                // 根據 memberId 查找會員
                var member = await db.Members.FirstOrDefaultAsync(m => m.Id == memberId);
                if (member == null || member.ConfirmCode != confirmCode)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "無效的確認連結");
                }

                // 更新會員的 IsConfirmed 欄位
                member.IsConfirmed = true;
                member.ConfirmCode = null; // 清除確認碼
                await db.SaveChangesAsync();

                return Request.CreateResponse(HttpStatusCode.OK, "帳號已成功啟用");
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
