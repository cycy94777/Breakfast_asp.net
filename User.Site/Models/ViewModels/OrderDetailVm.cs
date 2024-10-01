using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class OrderDetailVm
    {
        public int ProductId { get; set; }  // 商品 ID
        public int Quantity { get; set; }  // 商品數量
        public List<OrderAddOnDetailVm> Options { get; set; }  // 加選項目
    }
}