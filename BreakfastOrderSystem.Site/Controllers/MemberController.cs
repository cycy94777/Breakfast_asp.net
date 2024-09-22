using BreakfastOrderSystem.Site.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BreakfastOrderSystem.Site.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member
        private readonly MemberService _memberService;

        public MemberController()
        {
            _memberService = new MemberService();
        }

        // GET: Member
        // 會員明細
        public ActionResult MemberDetails()
        {
            var members = _memberService.GetMemberDetails();
            return View(members); //將view ViewModel 傳遞給View
        }

        // 黑名單
        public ActionResult BlackList()
        {
            var blacklistMembers = _memberService.GetBlacklist();
            return View(blacklistMembers);
        }


        // 黑名單
        public ActionResult test()
        {
            var blacklistMembers = _memberService.GetBlacklist();
            return View(blacklistMembers);
        }

        // 處理解除封鎖請求
        [HttpPost]
        public ActionResult Unblock(string email)
        {
            _memberService.UnblockMember(email);
            return RedirectToAction(nameof(BlackList)); // 重新導向到黑名單頁面
        }

        public ActionResult PointDetail()
        {
            return View();
        }
    }
}