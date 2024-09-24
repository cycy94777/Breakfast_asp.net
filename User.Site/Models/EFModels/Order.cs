namespace User.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            PointDetails = new HashSet<PointDetail>();
        }

        public int Id { get; set; }

        public int TakeOrderNumber { get; set; }

        public DateTime OrderTime { get; set; }

        public DateTime TakeTime { get; set; }

        public int? MemberID { get; set; }

        public int Total { get; set; }

        public int PointsUsed { get; set; }

        public int FinalTotal { get; set; }

        public int OrderStatus { get; set; }

        public virtual Member Member { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PointDetail> PointDetails { get; set; }
    }
}
