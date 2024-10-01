using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class OrderVm
    {
        public int OrderID { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime PickupTime { get; set; }
        public decimal TotalAmount { get; set; }
        public int OrderStatus { get; set; }
        public int PointsUsed { get; set; }
        public int PointsEarned { get; set; }
        public List<OrderItemVm> Items { get; set; }  // 订单中的商品
    }
}