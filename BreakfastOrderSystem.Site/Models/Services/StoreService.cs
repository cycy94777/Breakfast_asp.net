using BreakfastOrderSystem.Site.Models.Repositories;
using BreakfastOrderSystem.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreakfastOrderSystem.Site.Models.Services
{
    public class StoreService
    {
        private readonly StoreRepository _storeRepository;

        public StoreService()
        {
            _storeRepository = new StoreRepository();
        }

      
        public List<StoreVm> getStoreInfo()
        {
            var stores = _storeRepository.getStoreInfo();
            return stores.Select(m => new StoreVm
            {
                Id = m.Id,
                Name = m.Name,
                Account = m.Account,
                EncryptedPassword = m.EncryptedPassword,
                RegistrationDate = m.RegistrationDate,
                ProfilePhoto = m.ProfilePhoto,
            }).ToList();
        }

        
    }
}