using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.OrderRequest
{
    public class OrderRequest
    {
        public int MemberId { get; set; }           // 會員ID
        public int Total { get; set; }           // 訂單總金額
        public int PointsUsed { get; set; }      // 使用的點數
        public int FinalTotal { get; set; }      // 最終金額
        //public int PointsEarned { get; set; }        // 獲得的點數
        public DateTime TakeTime { get; set; }       // 取餐時間
        public List<OrderDetailRequest> OrderDetails { get; set; } // 訂單明細
    }
}