using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class ProductAddOnItemVm
    {
        public string Label { get; set; }    // 加選名稱 (例如: 加起司, 加蛋)
        public string Name { get; set; }     // 加選名稱
        public int Price { get; set; }       // 加選價格
        public int ProductAddOnDetailsId { get; set; }
    }
}