using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreakfastOrderSystem.Site.Models.ViewModels
{
    public class PointDetailVm
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public string MemberName { get; set; } // 來自 Members 表的 Name 欄位

        public int OrderId { get; set; }

        public int Original { get; set; }

        public int Used { get; set; }

        public int Earned { get; set; }

        public DateTime Date { get; set; }

        public int Remaining { get; set; }
    }
}