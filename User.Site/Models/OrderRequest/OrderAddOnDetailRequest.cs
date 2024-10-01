using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.OrderRequest
{
    public class OrderAddOnDetailRequest
    {
        public int ProductAddOnDetailsId { get; set; } // 加選項ID
        public int AddOnQuantity { get; set; }         // 加選項的數量
        public int AddOnOptionPrice { get; set; }  // 加選項的價格
    }
}