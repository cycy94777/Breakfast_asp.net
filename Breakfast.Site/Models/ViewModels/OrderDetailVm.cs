using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Breakfast.Site.Models.ViewModels
{
    public class OrderDetailVm
    {
        public int 訂單明細ID { get; set; }

        public int? 訂單ID { get; set; }

        public int? 商品ID { get; set; }

        public int? 加選資訊ID { get; set; }

        public int 數量 { get; set; }

        public decimal 單價 { get; set; }

        
        public decimal? 小計 { get; set; }
    }
}