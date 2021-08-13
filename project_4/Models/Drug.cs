namespace project_4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Drug
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Drug()
        {
            Drug_Buying = new HashSet<Drug_Buying>();
            Has_Drug = new HashSet<Has_Drug>();
        }

        [Required]
        [StringLength(20)]
        public string DName { get; set; }

        [Column(TypeName = "date")]
        public DateTime Production_Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime Expiring_Date { get; set; }

        [Required]
        [StringLength(20)]
        public string Department { get; set; }

        [StringLength(20)]
        public string Type { get; set; }

        public int? Price { get; set; }

        [Key]
        public int DID { get; set; }

        [StringLength(200)]
        public string Drug_Desc { get; set; }

        [StringLength(20)]
        public string Dstate { get; set; }

        [StringLength(800)]
        public string DImage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Drug_Buying> Drug_Buying { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Has_Drug> Has_Drug { get; set; }
    }
}
