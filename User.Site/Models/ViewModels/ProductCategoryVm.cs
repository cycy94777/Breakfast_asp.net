using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class ProductCategoryVm
    {
        public int Id { get; set; }


        public string Name { get; set; }

        public int DisplayOrder { get; set; }


        public string Image { get; set; }
        public List<ProductVm> Products { get; internal set; }
    }
}