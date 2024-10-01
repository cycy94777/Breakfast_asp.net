using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using User.Site.Models.EFModels;
using User.Site.Models.ViewModels;

namespace User.Site.Controllers.Apis
{
    public class ProductsApiController : ApiController
    {
        [HttpGet]
        [Route("api/productsapi/all")]
        public IHttpActionResult GetAllProducts()
        {
            var db = new AppDbContext();

            // 查询所有可用的商品
            var data = db.Products
                .Where(p => p.IsAvailable)  // 仅返回上架的商品
                .Include(p => p.ProductCategory)  // 关联类别表
                .Select(p => new ProductVm
                {
                    Id = p.Id,
                    Image = p.Image,
                    Alt = p.Name, // 商品名称作为图片描述
                    CategoryId = p.ProductCategoryId,
                    Text = p.Name, // 商品名称
                    Price = p.Price, // 商品价格
                    Currency = "NT$", // 假设使用固定的货币
                    Category = p.ProductCategory.Name, // 获取类别名称
                    IsAvailable = p.IsAvailable,
                    Options = db.ProductAddOnDetails
                        .Where(a => a.ProductId == p.Id)
                        .GroupBy(a => new { a.AddOnCategory.Name, a.AddOnCategory.IsSingleChoice })  // 根据加选项类别分组
                        .Select(g => new ProductOptionVm
                        {
                            Type = g.Key.IsSingleChoice ? "radio" : "checkbox", // 判断是否是单选
                            Category = g.Key.Name,
                            Items = g.Select(o => new ProductAddOnItemVm
                            {
                                Label = o.AddOnOptionName, // 加选项名称
                                Name = o.AddOnOptionName,
                                Price = o.AddOnOption.Price, // 加选项的价格
                                ProductAddOnDetailsId = o.Id
                            }).ToList()
                        }).ToList()  // 加选项
                }).ToList();  // 获取结果列表

            return Ok(data);  // 返回结果

        }


        [HttpGet]
        [Route("api/productsapi/product")]  // 更改路徑
        public IHttpActionResult GetProductById(int productId)
        {
            var db = new AppDbContext();

            var data = db.Products
                .Where(p => p.Id == productId && p.IsAvailable)
                .Include(p => p.ProductCategory)
                .Select(p => new ProductVm
                {
                    Id = p.Id,
                    Image = p.Image,
                    Alt = p.Name, // 假設商品名稱也可以用作圖片描述    
                    CategoryId = p.ProductCategoryId,
                    Text = p.Name,
                    Price = p.Price,
                    Currency = "NT$", // 假設使用固定的貨幣
                    Category = p.ProductCategory.Name, // 從關聯的 ProductCategories 表中取得類別名稱
                    IsAvailable = p.IsAvailable,
                    Options = db.ProductAddOnDetails
                        .Where(a => a.ProductId == p.Id)
                        .GroupBy(a => new { a.AddOnCategory.Name, a.AddOnCategory.IsSingleChoice }) // 根據加選類別分組
                        .Select(g => new ProductOptionVm
                        {
                            Type = g.Key.IsSingleChoice ? "radio" : "checkbox", // 根據是否單選來決定類型
                            Category = g.Key.Name,
                            Items = g.Select(o => new ProductAddOnItemVm
                            {
                                Label = o.AddOnOptionName,
                                Name = o.AddOnOptionName,
                                Price = o.AddOnOption.Price // 假設這裡有個加選價格
                            }).ToList()
                        }).ToList()
                }).ToList();

            return Ok(data);
        }
    }
}
