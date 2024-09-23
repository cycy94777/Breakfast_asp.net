using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BreakfastOrderSystem.Site.Models.ViewModels
{
    public class StoreVm
    {
        public int Id { get; set; }

        
        public string Name { get; set; }

        
        public string Account { get; set; }

        
        public string EncryptedPassword { get; set; }

        public DateTime RegistrationDate { get; set; }

        
        public string ProfilePhoto { get; set; }
    }
}
