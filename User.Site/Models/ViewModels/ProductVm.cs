using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class ProductVm
    {
        public int Id { get; set; }          // 商品 ID
        public string Image { get; set; }    // 商品圖片路徑
        public string Alt { get; set; }      // 商品圖片描述
        public int CategoryId { get; set; }  // 商品類別 ID
        public string Text { get; set; }     // 商品名稱
        public int Price { get; set; }       // 商品價格
        public string Currency { get; set; } // 貨幣 (例如: NT$)
        public string Category { get; set; } // 商品類別名稱 (例如: 漢堡類, 飲料類)
        public bool IsAvailable { get; set; }
        public List<ProductOptionVm> Options { get; set; } // 加選項目列表
        public int ProductAddOnDetailsId { get; set; }
    }
}