using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class PointsDetailVm
    {
        public string OrderID { get; set; }
        public DateTime Date { get; set; }
        public int TotalAmount { get; set; }
        public int EarnedPoints { get; set; }
        public int UsedPoints { get; set; }
        public int RemainingPoints { get; set; }
    }
}