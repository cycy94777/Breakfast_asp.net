using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using User.Site.Models.EFModels;
using User.Site.Models.ViewModels;

namespace User.Site.Controllers.Apis
{
    public class OrderlistApiController : ApiController
    {
        [HttpGet]
        [Route("api/orders/all")]
        public IHttpActionResult Get(int memberId)
        {
            var db = new AppDbContext();

            // 查询指定订单 ID 的订单详情
            var order = db.Orders
                .Where(o => o.MemberID == memberId)
                .Select(o => new OrderVm
                {
                    OrderID = o.Id,
                    OrderTime = o.OrderTime,
                    PickupTime = o.TakeTime,
                    TotalAmount = o.FinalTotal,
                    OrderStatus = o.OrderStatus,

                    // PointsEarned 从 PointDetails 中提取
                    PointsEarned = db.PointDetails
                        .Where(pd => pd.OrderId == o.Id)
                        .Select(pd => pd.Earned)
                        .FirstOrDefault(),

                    PointsUsed = o.PointsUsed,

                    // 查询 OrderDetails 的数据
                    Items = db.OrderDetails
                        .Where(od => od.OrderID == o.Id)
                        .Select(od => new OrderItemVm
                        {
                            Name = od.Product.Name,

                            // Description 从 OrderAddOnDetails 和 ProductAddOnDetails 提取
                            Description = db.OrderAddOnDetails
                                .Where(aod => aod.OrderDetailID == od.Id)
                                .Select(aod => db.ProductAddOnDetails
                                    .Where(pad => pad.Id == aod.ProductAddOnDetailsID)
                                    .Select(pad => pad.AddOnOptionName)
                                    .FirstOrDefault())
                                .FirstOrDefault(),

                            // 从 Products 表提取 Image
                            Image = db.Products
                                .Where(p => p.Name == od.Product.Name)
                                .Select(p => p.Image)
                                .FirstOrDefault(),

                            Price = od.ProductPrice,
                            Quantity = od.ProductQuantity
                        }).ToList()
                });

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }



        [HttpPost]
        [Route("api/orders/cancel/{orderId}")]
        public IHttpActionResult CancelOrder(int orderId)
        {
            using (var db = new AppDbContext())
            {
                // 找到對應的訂單
                var order = db.Orders.FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    // 如果找不到訂單，返回 404 Not Found
                    return NotFound();
                }

                // 檢查訂單狀態是否允許取消
                if (order.OrderStatus != 1)
                {
                    // 如果不是 "未取餐" 狀態，則不能取消訂單，返回 400 Bad Request
                    return BadRequest("訂單已經被處理，無法取消。");
                }

                // 更新訂單狀態為 "已取消" (3)
                order.OrderStatus = 3;

                // 儲存變更到資料庫
                db.SaveChanges();

                // 返回成功訊息
                return Ok(new { message = "訂單已取消", orderId = orderId });
            }
        }
    }
}
