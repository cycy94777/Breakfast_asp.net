using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using User.Site.Models.EFModels;

namespace User.Site.Models.ViewModels
{
    public class OrderAgainVm
    {
        public int Id { get; set; }  // 訂單 ID
        public List<OrderDetailVm> Items { get; set; }  // 訂單商品明細
        public string PickupTime { get; set; }  // 取餐時間
        public int Points { get; set; }  // 使用的點數
        public int TotalAmount { get; set; }  // 總金額
        public int PointsEarned { get; set; }  // 獲得的點數
    }
}