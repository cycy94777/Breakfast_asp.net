namespace BreakfastOrderSystem.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderAddOnDetail
    {
        public int Id { get; set; }

        public int OrderDetailID { get; set; }

        public int ProductAddOnDetailsID { get; set; }

        public int AddOnQuantity { get; set; }

        public int AddOnOptionPrice { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }

        public virtual ProductAddOnDetail ProductAddOnDetail { get; set; }
    }
}
