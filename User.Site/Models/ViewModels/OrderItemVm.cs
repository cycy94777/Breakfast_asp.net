using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class OrderItemVm
    {
        public string Name { get; set; }          // 商品名称
        public string Description { get; set; }   // 商品描述
        public decimal Price { get; set; }        // 商品价格
        public int Quantity { get; set; }         // 商品数量
        public string Image { get; set; }         // 商品图片路径
        public List<AddOnVm> Extras { get; set; } // 商品的加选项
    }
}