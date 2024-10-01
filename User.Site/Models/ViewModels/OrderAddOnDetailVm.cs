using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class OrderAddOnDetailVm
    {
        public int Price { get; set; }  // 加選價格
        public string Name { get; set; }  // 加選名稱
    }
}