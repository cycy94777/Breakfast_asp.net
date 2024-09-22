namespace BreakfastOrderSystem.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AddOnOption
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AddOnOption()
        {
            ProductAddOnDetails = new HashSet<ProductAddOnDetail>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AddOnOptionID { get; set; }

        [StringLength(100)]
        public string AddOnOptionName { get; set; }

        public int? AddOnCategoryID { get; set; }

        public decimal? AddOnOptionPrice { get; set; }

        public virtual AddOnCategory AddOnCategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductAddOnDetail> ProductAddOnDetails { get; set; }
    }
}
