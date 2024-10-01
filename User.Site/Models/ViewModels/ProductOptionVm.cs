using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class ProductOptionVm
    {
        public string Type { get; set; }     // 加選類型 (例如: checkbox, radio)
        public string Category { get; set; } // 加選類別名稱 (例如: 餐點加料, 飲料溫度)
        public List<ProductAddOnItemVm> Items { get; set; } // 加選項目列表
        public int ProductAddOnDetailsId { get; set; }
    }
}