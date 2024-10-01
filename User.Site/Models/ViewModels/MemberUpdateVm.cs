using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class MemberUpdateVm
    {
        public string Account { get; set; }
        public string OriginalAccount { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string ProfilePhoto { get; set; } 
    }
}