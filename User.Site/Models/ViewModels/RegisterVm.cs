using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class RegisterVm
    {
        public string Name { get; set; }
        public string Account { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string ProfilePhoto { get; set; } // 可根據需求添加
        
    }
}