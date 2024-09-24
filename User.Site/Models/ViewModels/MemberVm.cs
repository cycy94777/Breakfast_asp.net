using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace User.Site.Models.ViewModels
{
    public class MemberVm
    {
        public int Id { get; set; }

        
        public string Name { get; set; }

       
        public string Account { get; set; }

        
        public string EncryptedPassword { get; set; }

        
        public string Phone { get; set; }

        
        public string ProfilePhoto { get; set; }

        public int Points { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool BlackList { get; set; }

        public bool IsConfirmed { get; set; }
    }
}