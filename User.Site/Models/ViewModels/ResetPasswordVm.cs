using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class ResetPasswordVm
    {
        public string NewPassword  { get; set; }
        public int MemberId {  get; set; }
    }
}