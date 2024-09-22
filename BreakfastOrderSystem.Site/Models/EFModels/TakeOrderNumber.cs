namespace BreakfastOrderSystem.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TakeOrderNumber
    {
        public int Id { get; set; }

        public int CurrentNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime LastUpdated { get; set; }
    }
}
