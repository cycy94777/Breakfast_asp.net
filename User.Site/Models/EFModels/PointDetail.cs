namespace User.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PointDetail
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public int OrderId { get; set; }

        public int Original { get; set; }

        public int Used { get; set; }

        public int Earned { get; set; }

        public DateTime Date { get; set; }

        public int Remaining { get; set; }

        public virtual Member Member { get; set; }

        public virtual Order Order { get; set; }
    }
}
