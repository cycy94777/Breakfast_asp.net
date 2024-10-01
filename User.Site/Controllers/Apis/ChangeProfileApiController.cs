using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using User.Site.Models.EFModels;
using User.Site.Models.ViewModels;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Web.Services.Description;


namespace User.Site.Controllers.Apis
{
    public class ChangeProfileApiController : ApiController
    {
        private const string AuthCookieName = "auth_token";
        private bool IsAuthenticated()
        {
            var cookieHeader = Request.Headers.GetCookies(AuthCookieName).FirstOrDefault();
            return cookieHeader != null && cookieHeader[AuthCookieName] != null;
        }


        [HttpPost]
        [Route("api/MembersApi/UpdateProfilePhoto")]
        public async Task<HttpResponseMessage> UpdateProfilePhoto()
        {
            var httpRequest = HttpContext.Current.Request;


            if (string.IsNullOrWhiteSpace(httpRequest.Form["account"]))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "帳號不能為空");
            }



            var account = httpRequest.Form["account"];

            if (!IsAuthenticated())
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "User is not authenticated.");
            }

            var postedFile = httpRequest.Files["profilePhoto"];

            if (postedFile == null || postedFile.ContentLength <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "未提供有效的圖片文件");
            }

            // 檢查文件的類型，確保是圖片
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(postedFile.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "不支持的圖片格式");
            }

            string relativePath = null; // 保存相對路徑


            try
            {
                // 設定儲存檔案的資料夾路徑
                var profilePhotoDirectory = HttpContext.Current.Server.MapPath("~/Content/Images/profile/");
                if (!Directory.Exists(profilePhotoDirectory))
                {
                    Directory.CreateDirectory(profilePhotoDirectory); // 如果資料夾不存在，則建立
                }

                // 生成新的唯一文件名，避免文件名衝突
                var fileName = Guid.NewGuid().ToString() + fileExtension;

                // 組合完整的檔案路徑（絕對路徑）
                var filePath = Path.Combine(profilePhotoDirectory, fileName);

                // 保存檔案到指定路徑
                postedFile.SaveAs(filePath);

                // 設定相對路徑，這樣可以在前端顯示圖片
                relativePath = "/Content/Images/profile/" + fileName;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "上傳圖片時發生錯誤：" + ex.Message);
            }

            using (var db = new AppDbContext())
            {
                // 查找使用 account 的會員
                var member = await db.Members.FirstOrDefaultAsync(m => m.Account == account);
                if (member == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "用戶不存在");
                }

                // 更新頭像路徑
                member.ProfilePhoto = relativePath;
                await db.SaveChangesAsync();
            }

            return Request.CreateResponse(HttpStatusCode.OK, "頭像更新成功");
        }


        [HttpPost]
        [Route("api/MembersApi/DeleteProfilePhoto")]
        public async Task<HttpResponseMessage> DeleteProfilePhoto()
        {
            var httpRequest = HttpContext.Current.Request;

            var account = httpRequest.Form["account"];

            // 驗證用戶是否已經通過身份驗證
            if (!IsAuthenticated())
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { success = false, message = "用戶未通過身份驗證" });
            }


            // 驗證帳號
            if (string.IsNullOrWhiteSpace(account))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "帳號不能為空");
            }



            

            try
            {
                using (var db = new AppDbContext())
                {
                    // 根據帳號查詢會員
                    var member = await db.Members.FirstOrDefaultAsync(m => m.Account == account);
                    if (member == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, new { success = false, message = "用戶不存在" } );
                    }

                    // 檢查會員是否已有頭像
                    if (!string.IsNullOrEmpty(member.ProfilePhoto))
                    {
                        // 刪除伺服器上已存在的頭像檔案
                        var profilePhotoPath = HttpContext.Current.Server.MapPath(member.ProfilePhoto);
                        if (File.Exists(profilePhotoPath))
                        {
                            File.Delete(profilePhotoPath);
                        }

                        // 將會員的頭像重置為空
                        member.ProfilePhoto = null;

                        // 保存更改到資料庫
                        await db.SaveChangesAsync();

                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest,  new { success = false, message = "該用戶未設置頭像" });
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK,   new { success = true, message = "頭像已成功刪除" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,  new { success = false, message = "刪除頭像時發生錯誤:" + ex.Message });
            }
                
            
        }
    }
}
