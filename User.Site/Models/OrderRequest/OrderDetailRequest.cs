using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.OrderRequest
{
    public class OrderDetailRequest
    {
        public int ProductId { get; set; }           // 商品ID
        public string ProductName { get; set; }      // 商品名稱
        public int ProductPrice { get; set; }    // 單價
        public int ProductQuantity { get; set; }     // 數量
        public int SubTotal { get; set; }        // 小計
        public List<OrderAddOnDetailRequest> OrderAddOnDetails { get; set; } // 加選項目
    }
}