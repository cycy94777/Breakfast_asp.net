using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreakfastOrderSystem.Site.Models.ViewModels
{
    public class MemberDetailVm
    {
        public int MemberID { get; set; }


        public string MemberName { get; set; }


        public string Email { get; set; }


        public string Phone { get; set; }

        public int Point { get; set; }


        public DateTime RegisterDate { get; set; }

    }
}