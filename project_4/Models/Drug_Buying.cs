namespace project_4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Drug_Buying
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SSN { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Buying_Date { get; set; }

        public virtual Drug Drug { get; set; }

        public virtual User User { get; set; }
    }
}
