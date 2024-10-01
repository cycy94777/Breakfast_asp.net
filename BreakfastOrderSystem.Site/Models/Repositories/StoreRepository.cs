using BreakfastOrderSystem.Site.Models.Dtos;
using BreakfastOrderSystem.Site.Models.EFModels;
using BreakfastOrderSystem.Site.Models.ViewModels;
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

        public StoreDto Get(string account)
        {
            var store = _db.Stores
                        .AsNoTracking()
                        .FirstOrDefault(m => m.Account == account);
            if (store == null) return null;

            return new StoreDto
            {   
                Id = store.Id,
                Account = store.Account,
                EncryptedPassword = store.EncryptedPassword,
                Name = store.Name
            };
        }
        public StoreDto Get(int id)
        {
            var store = _db.Stores
                        .AsNoTracking()
                        .FirstOrDefault(m => m.Id == id);
            if (store == null) return null;

            return new StoreDto
            {
                Id = store.Id,
                Account = store.Account,
                EncryptedPassword = store.EncryptedPassword,
                Name = store.Name
            };
        }

    }
}