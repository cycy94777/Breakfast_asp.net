namespace BreakfastOrderSystem.Site.Models.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AddOnCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AddOnCategory()
        {
            AddOnOptions = new HashSet<AddOnOption>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AddOnCategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string AddOnCategoryName { get; set; }

        public bool IsSingleChoice { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AddOnOption> AddOnOptions { get; set; }
    }
}
