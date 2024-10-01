using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreakfastOrderSystem.Site.Models.Dtos
{
    public class StoreDto
    {   public int Id { get; set; }
        public string Account { get; set; }
        public string EncryptedPassword { get; set; }
        public string Name { get; set; }
    }
}