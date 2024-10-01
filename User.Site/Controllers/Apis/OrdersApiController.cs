using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using User.Site.Models.EFModels;
using User.Site.Models.OrderRequest;
using User.Site.Models.ViewModels;

namespace User.Site.Controllers.Apis
{
    public class OrdersApiController : ApiController
    {
        [HttpGet]
        [Route("api/ordersapi/latest")]
        public IHttpActionResult GetLatestOrderByMemberId(int memberId)
        {
            var db = new AppDbContext();

            // 查詢該會員的最新訂單（依照 OrderTime 降序）
            var latestOrder = db.Orders
                .Where(o => o.MemberID == memberId)
                .OrderByDescending(o => o.OrderTime)
                .FirstOrDefault();

            if (latestOrder == null)
            {
                return NotFound();  // 如果找不到訂單，返回 404
            }

            var orderDetails = db.OrderDetails
                .Where(od => od.OrderID == latestOrder.Id)
                .Select(od => new OrderDetailVm
                {
                    ProductId = od.ProductID,
                    Quantity = od.ProductQuantity,
                    Options = db.OrderAddOnDetails
                        .Where(aod => aod.OrderDetailID == od.Id)  // 根據 ProductId 篩選正確的加選選項
                        .Select(aod => new OrderAddOnDetailVm
                        {
                            Price = aod.AddOnOptionPrice,
                            Name = aod.ProductAddOnDetail.AddOnOptionName
                        })
                        .ToList()
                })
                .ToList();

            // 建立返回的 ViewModel
            var orderVm = new OrderAgainVm
            {
                Id = latestOrder.Id,
                Items = orderDetails,
                PickupTime = latestOrder.TakeTime.ToString("HH:mm"),  // 轉成時間格式
                Points = latestOrder.PointsUsed,
                TotalAmount = latestOrder.FinalTotal,
                PointsEarned = (int)(latestOrder.FinalTotal / 60)  // 假設 1 點數等於 60 元
            };

            return Ok(orderVm);
        }

        [HttpGet]
        [Route("api/ordersapi/reorder")]
        public IHttpActionResult Reorder(int orderId)
        {
            using (var db = new AppDbContext())
            {
                // 1. 查找历史订单
                var order = db.Orders
                    .Include(o => o.OrderDetails.Select(od => od.Product))
                    .Include(o => o.OrderDetails.Select(od => od.OrderAddOnDetails.Select(oad => oad.ProductAddOnDetail)))
                    .FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    return NotFound();
                }

                int newTotal = 0;

                // 2. 遍历订单的每个项目
                foreach (var orderDetail in order.OrderDetails)
                {
                    // 根据 ProductId 查找最新的商品信息
                    var product = db.Products
                        .Include(p => p.ProductAddOnDetails)
                        .FirstOrDefault(p => p.Id == orderDetail.ProductID);

                    if (product != null)
                    {
                        // 更新商品价格
                        orderDetail.ProductPrice = product.Price;

                        // 计算加选的价格
                        int addOnTotal = 0;
                        foreach (var addOn in orderDetail.OrderAddOnDetails)
                        {
                            // 根据 ProductAddOnDetailsID 查找最新的加选价格
                            var addOnDetail = db.ProductAddOnDetails
                                .FirstOrDefault(pad => pad.Id == addOn.ProductAddOnDetailsID);
                            if (addOnDetail != null)
                            {
                                // 更新加选项的价格
                                addOn.AddOnOptionPrice = addOnDetail.AddOnOption.Price;
                                addOnTotal += addOn.AddOnOptionPrice * addOn.AddOnQuantity;
                            }
                        }

                        // 更新订单项的总价
                        orderDetail.SubTotal = (orderDetail.ProductPrice + addOnTotal) * orderDetail.ProductQuantity;

                        // 累加总价
                        newTotal += orderDetail.SubTotal;
                    }
                }

                // 3. 更新订单总价
                order.Total = newTotal;
                db.SaveChanges();

                // 4. 返回更新后的订单信息
                var result = new
                {
                    OrderId = order.Id,
                    Total = order.Total,
                    Items = order.OrderDetails.Select(od => new
                    {
                        ProductId = od.ProductID,
                        ProductName = od.ProductName,
                        Quantity = od.ProductQuantity,
                        Price = od.ProductPrice,
                        SubTotal = od.SubTotal,
                        AddOns = od.OrderAddOnDetails.Select(a => new
                        {
                            AddOnOptionName = a.ProductAddOnDetail.AddOnOptionName,
                            AddOnOptionPrice = a.AddOnOptionPrice,
                            Quantity = a.AddOnQuantity
                        })
                    }).ToList()
                };

                return Ok(result);
            }
        }


        [HttpPost]
        [Route("api/ordersapi/create")]
        public async Task<IHttpActionResult> CreateOrder([FromBody] OrderRequest orderRequest)
        {
            using (var db = new AppDbContext()) // 使用 AppDbContext
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. 檢查取餐號碼表，看看是否需要重置號碼
                        var today = DateTime.Today;
                        var takeOrderNumberRecord = db.TakeOrderNumbers.FirstOrDefault();

                        int takeOrderNumber;
                        if (takeOrderNumberRecord == null || takeOrderNumberRecord.LastUpdated < today)
                        {
                            // 如果是新的一天，重置取餐號碼
                            takeOrderNumber = 1;
                            if (takeOrderNumberRecord == null)
                            {
                                takeOrderNumberRecord = new TakeOrderNumber
                                {
                                    CurrentNumber = takeOrderNumber,
                                    LastUpdated = today
                                };
                                db.TakeOrderNumbers.Add(takeOrderNumberRecord);
                            }
                            else
                            {
                                takeOrderNumberRecord.CurrentNumber = takeOrderNumber;
                                takeOrderNumberRecord.LastUpdated = today;
                                db.Entry(takeOrderNumberRecord).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            // 否則遞增取餐號碼
                            takeOrderNumber = takeOrderNumberRecord.CurrentNumber + 1;
                            takeOrderNumberRecord.CurrentNumber = takeOrderNumber;
                            db.Entry(takeOrderNumberRecord).State = EntityState.Modified;
                        }

                        // 保存變量表的變更
                        await db.SaveChangesAsync();

                        // 2. 建立訂單
                        var order = new Order
                        {
                            MemberID = orderRequest.MemberId,
                            Total = orderRequest.Total,
                            PointsUsed = orderRequest.PointsUsed,
                            FinalTotal = orderRequest.FinalTotal,
                            //PointsEarned = orderRequest.PointsEarned,
                            OrderTime = DateTime.Now,
                            TakeTime = orderRequest.TakeTime,
                            TakeOrderNumber = takeOrderNumber,   // 生成的取餐號碼
                            OrderStatus = 1                     // 預設為未取餐
                        };

                        db.Orders.Add(order);
                        await db.SaveChangesAsync(); // 先保存訂單以生成訂單 ID

                        // 3. 保存訂單明細和加選項目
                        foreach (var item in orderRequest.OrderDetails)
                        {
                            var orderDetail = new OrderDetail
                            {
                                OrderID = order.Id, // 關聯訂單 ID
                                ProductID = item.ProductId,
                                ProductName = item.ProductName,
                                ProductPrice = item.ProductPrice,
                                ProductQuantity = item.ProductQuantity,
                                SubTotal = item.SubTotal  // 使用 subtotal
                            };

                            db.OrderDetails.Add(orderDetail);
                            await db.SaveChangesAsync(); // 保存訂單明細以生成訂單明細 ID

                            // 處理加選項目
                            foreach (var addOn in item.OrderAddOnDetails)
                            {
                                var orderAddOnDetail = new OrderAddOnDetail
                                {
                                    OrderDetailID = orderDetail.Id, // 關聯訂單明細 ID
                                    ProductAddOnDetailsID = addOn.ProductAddOnDetailsId,
                                    AddOnQuantity = addOn.AddOnQuantity,
                                    AddOnOptionPrice = addOn.AddOnOptionPrice
                                };

                                db.OrderAddOnDetails.Add(orderAddOnDetail);
                            }
                        }

                        // 保存所有變更
                        await db.SaveChangesAsync();

                        // 提交交易
                        transaction.Commit();

                        // 構建 ViewModel 返回
                        var takeOrderNumberVM = new TakeOrderNumberVm
                        {
                            TakeOrderNumber = takeOrderNumber,
                            LastUpdated = takeOrderNumberRecord.LastUpdated
                        };

                        return Ok(new { orderId = order.Id, takeOrderNumber = takeOrderNumberVM });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BadRequest("訂單創建失敗，請確認輸入的資料是否正確");
                    }
                }
            }
        }
    }
}
