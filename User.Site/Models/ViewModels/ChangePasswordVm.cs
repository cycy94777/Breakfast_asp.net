using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class ChangePasswordVm
    {
        public string Account { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}