using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class AddOnVm
    {
        public string Name { get; set; }   // 加选项名称
        public decimal Price { get; set; } // 加选项价格
    }
}