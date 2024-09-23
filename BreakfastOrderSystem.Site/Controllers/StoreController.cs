using BreakfastOrderSystem.Site.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BreakfastOrderSystem.Site.Controllers
{
    public class StoreController : Controller
    {
        private readonly StoreService _storeService;

        public StoreController()
        {
            _storeService = new StoreService();
        }


        // GET: Store
        public ActionResult Index()
        {
            var stores = _storeService.getStoreInfo();
            return View(stores);
        }
    }
}