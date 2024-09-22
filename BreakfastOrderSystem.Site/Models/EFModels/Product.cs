namespace BreakfastOrderSystem.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ProductAddOnDetails = new HashSet<ProductAddOnDetail>();
        }

        public int ProductID { get; set; }

        public int ProductCategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        [StringLength(255)]
        public string ProductImage { get; set; }

        public bool? IsAvailable { get; set; }

        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductAddOnDetail> ProductAddOnDetails { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
    }
}
