using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class TakeOrderNumberVm
    {
        public int TakeOrderNumber { get; set; }  // 取餐號碼
        public DateTime LastUpdated { get; set; } // 上次更新日期
    }
}