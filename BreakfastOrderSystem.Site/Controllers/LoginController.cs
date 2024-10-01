using BreakfastOrderSystem.Site.Models.Infra;
using BreakfastOrderSystem.Site.Models.Services;
using BreakfastOrderSystem.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BreakfastOrderSystem.Site.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _loginService;
        private readonly EmailHelper _emailHelper;

        public LoginController()
        {
            _loginService = new LoginService();
            _emailHelper = new EmailHelper();
        }


        // GET: Login
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public JsonResult Login(LoginVm vm)
        {
            if (ModelState.IsValid)
            {
                Result result = _loginService.ValidateLogin(vm);
                if (result.IsSuccess)
                {   
                    string name = _loginService.GetStoreName(vm);
                    //ViewBag.StoreName = name;
                    ViewBag.Name = name;
                    TempData["UserName"] = name;
                    Session["UserName"] = name;
                    (string url, HttpCookie cookie) = ProcessLogin(vm.Account);
                    Response.Cookies.Add(cookie);
                    return Json(new { success = true, url = Url.Action("Index", "Home") });
                }

                //ModelState.AddModelError(
                //    string.Empty,
                //    result.ErrorMessage);

                //ViewBag.LoginError = "帳號或密碼有錯誤"; // 將錯誤訊息傳遞給前端

                //vm.Account = string.Empty;
                //vm.Password = string.Empty;
                return Json(new { success = false, message = "帳號或密碼有錯誤" });

            }
            //return View(vm);
            return Json(new { success = false, message = "請確認輸入是否正確" });
        }


        private (string url, HttpCookie cookie) ProcessLogin(string account)
        {
            var roles = string.Empty; // 在本範例，沒有用到角色權限
            // 建立一張認證票
            var ticket =
                new FormsAuthenticationTicket(
                    1, // 版本別
                    account, // 
                    DateTime.Now, // 發行日
                    DateTime.Now.AddDays(2), //到期日
                    false, //是否續存
                    roles, //userdata
                    "/" //cookie位置
                );

            // 將它加密
            var value = FormsAuthentication.Encrypt(ticket);
            // 存入cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);
            // 取得return url , web config defaultUrl="/Members/Index
            var url = FormsAuthentication.GetRedirectUrl(account, true);
            return (url, cookie);
        }


        //登出
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            // 獲取 cookie 名稱
            string cookieName = FormsAuthentication.FormsCookieName;

            // 清除 cookie
            if (Request.Cookies[cookieName] != null)
            {
                var cookie = new HttpCookie(cookieName)
                {
                    Expires = DateTime.Now.AddDays(-1) // 將到期日設置為過去的日期以刪除 cookie
                };
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Login", "Login");
        }

        // 忘記密碼
        [HttpPost]
        public JsonResult ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "請輸入有效的信箱" });
            }

            var member = _loginService.Get(email);
            // 不管是否找到該電子郵件，均回應已成功寄送郵件
            // 如果找到，則真正發送重置密碼的郵件
            if (member != null)
            {
                try
                {
                    string resetLink = _emailHelper.GenerateResetPasswordLink(member.Id);
                    _emailHelper.SendResetPasswordEmail(member.Account, resetLink);
                }
                catch { return Json(new { success = false, message = "寄信有誤" }); }
            }
            return Json(new { success = true, message= "請至信箱點擊連結以重設密碼" });
            }




        public ActionResult ResetPassword(int memberId)
        {
            ViewBag.MemberId = memberId;
            return View(new ResetPasswordVm());
        }

        [HttpPost]
        public ActionResult ResetPassword(int memberId, ResetPasswordVm vm)
        {
            if (ModelState.IsValid == false) return View(vm);

            var result = _loginService.ProcessChangePassword(memberId, vm.Password);

            if (result.IsSuccess == false)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(vm);
            }
            return View("ResetPasswordConfirm");
        }

    }
}