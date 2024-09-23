using BreakfastOrderSystem.Site.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BreakfastOrderSystem.Site.Controllers
{
    public class PointController : Controller
    {
        private readonly PointService _pointService;


        public PointController()
        {
            _pointService = new PointService();
        }

        // GET: Point
        public ActionResult PointDetails()
        {
            // 從 Service 層獲取轉換後的 ViewModel 資料
            var pointDetails = _pointService.GetPointDetail();
            return View(pointDetails);
        }


        //test
        public ActionResult PointDetailsTest()
        {
            // 從 Service 層獲取轉換後的 ViewModel 資料
            var pointDetails = _pointService.GetPointDetail();
            return View(pointDetails);
    }


}
}