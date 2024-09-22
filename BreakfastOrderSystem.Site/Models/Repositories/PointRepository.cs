using BreakfastOrderSystem.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreakfastOrderSystem.Site.Models.Repositories
{
    public class PointRepository
    {
        private AppDbContext _db;

        public PointRepository()
        {
            _db = new AppDbContext();
        }


        public IEnumerable<PointDetail> GetPointDetail()
        {
            return _db.PointDetails
              .Include("Member")
              .Include("Order")
              .ToList();
        }
    }
}