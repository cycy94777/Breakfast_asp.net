using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.PointRequest
{
    public class PointDetailRequest
    {
        public int MemberId { get; set; }
        public int OrderId { get; set; }
        public int Original { get; set; }
        public int Used { get; set; }
        public int Earned { get; set; }
        public int Remaining { get; set; }
        public DateTime Date { get; set; }
    }
}