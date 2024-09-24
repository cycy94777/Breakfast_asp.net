namespace User.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductAddOnDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductAddOnDetail()
        {
            OrderAddOnDetails = new HashSet<OrderAddOnDetail>();
        }

        public int Id { get; set; }

        public int ProductId { get; set; }

        public int AddOnCategoryId { get; set; }

        public int AddOnOptionId { get; set; }

        [Required]
        [StringLength(50)]
        public string AddOnOptionName { get; set; }

        public virtual AddOnCategory AddOnCategory { get; set; }

        public virtual AddOnOption AddOnOption { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderAddOnDetail> OrderAddOnDetails { get; set; }

        public virtual Product Product { get; set; }
    }
}
