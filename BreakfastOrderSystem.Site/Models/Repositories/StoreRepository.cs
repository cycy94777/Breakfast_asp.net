using BreakfastOrderSystem.Site.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreakfastOrderSystem.Site.Models.Repositories
{
    public class StoreRepository
    {
        private AppDbContext _db;

        public StoreRepository()
        {
            _db = new AppDbContext();
        }

        public IEnumerable<Store> getStoreInfo()
        {
            var storeData = _db.Stores;
            return storeData.ToList();
        }


        public Store getStoreInfo(string storeName)
        {
            return _db.Stores.FirstOrDefault(m => m.Name == storeName);
        }
    }
}