using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using User.Site.Models.EFModels;
using User.Site.Models.ViewModels;
using System.Data.Entity;

namespace User.Site.Controllers.Apis
{
    public class CategoriesApiController : ApiController
    {
        [HttpGet]
        [Route("api/categoriesapi/all")]
        public IHttpActionResult GetAllCategories()
        {
            var db = new AppDbContext();

            var data = db.ProductCategories
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new ProductCategoryVm
                {
                    Id = x.Id,
                    Name = x.Name,
                    DisplayOrder = (int)x.DisplayOrder,
                    Image = x.Image
                })
                .ToList();
            return Ok(data);

        }

        [HttpGet]
        [Route("api/categoriesapi/products")]
        public IHttpActionResult GetAllProducts(int categoryId)
        {
            var db = new AppDbContext();

            var data = db.Products
                .Where(p => p.ProductCategoryId == categoryId && p.IsAvailable)
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
                                Price = o.AddOnOption.Price, // 假設這裡有個加選價格
                                ProductAddOnDetailsId = o.Id
                            }).ToList()
                        }).ToList()
                }).ToList();

            return Ok(data);
        }
    }
}
