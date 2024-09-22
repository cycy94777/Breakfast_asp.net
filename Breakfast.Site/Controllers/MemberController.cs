using Breakfast.Site.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Breakfast.Site.Controllers
{
    public class MemberController : Controller
    {
        private readonly MemberService _memberService;

        public MemberController()
        {
            _memberService = new MemberService();
        }

        // GET: Member
        public ActionResult MemberDetails()
        {
            var members = _memberService.GetMemberDetails();
            return View(members); //將view ViewModel 傳遞給View
        }

        public ActionResult BlackList()
        {
            return View();
        }

        public ActionResult PointDetail()
        {
            return View();
        }
    }
}