namespace BreakfastOrderSystem.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductAddOnDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductAddOnID { get; set; }

        public int? ProductID { get; set; }

        public int? AddOnCategoryID { get; set; }

        public int? AddOnOptionID { get; set; }

        public virtual AddOnOption AddOnOption { get; set; }

        public virtual Product Product { get; set; }
    }
}
