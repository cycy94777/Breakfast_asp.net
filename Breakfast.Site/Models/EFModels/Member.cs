namespace Breakfast.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Member
    {
        public int MemberID { get; set; }

        [Required]
        [StringLength(255)]
        public string MemberName { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        public int Point { get; set; }

        [Column(TypeName = "date")]
        public DateTime RegisterDate { get; set; }

        public bool IsConfirmed { get; set; }

        [StringLength(255)]
        public string ConfirmCode { get; set; }

        [StringLength(255)]
        public string EncryptedPassword { get; set; }

        public bool IsInBlacklist { get; set; }
    }
}
