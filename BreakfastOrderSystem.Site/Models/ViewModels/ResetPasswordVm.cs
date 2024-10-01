using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BreakfastOrderSystem.Site.Models.ViewModels
{
    public class ResetPasswordVm
    {
        [Display(Name = "新密碼")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(50)]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}