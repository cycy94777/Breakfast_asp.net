using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Breakfast.Site.Models.ViewModels
{
    public class MemberDetailVm
    {
        public int MemberID { get; set; }

        
        public string MemberName { get; set; }

        
        public string Email { get; set; }

        
        public string Phone { get; set; }

        public int Point { get; set; }

        
        public DateTime RegisterDate { get; set; }




    }
}