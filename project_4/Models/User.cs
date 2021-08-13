namespace project_4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Drug_Buying = new HashSet<Drug_Buying>();
            Has_Drug = new HashSet<Has_Drug>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"(2|3)[0-9][1-9][0-1][1-9][0-3][1-9](01|02|03|04|11|12|13|14|15|16|17|18|19|21|22|23|24|25|26|27|28|29|31|32|33|34|35|88)\d\d\d\d\d", ErrorMessage = "invalid national ID")]

        public long SSN { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(20)]
        public string UName { get; set; }

        [StringLength(50)]
        public string Street { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(20)]
        public string City { get; set; }

        [StringLength(20)]
        public string Governorate { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Email")]
        //[Remote("checkemail", "User", ErrorMessage = "Email already existed")]
        [RegularExpression(@"^[\w-.]+@([\w-]+.)+[\w-]{2,4}$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50)]
        //[DataType(DataType.Password)]

        public string Password { get; set; }
        [Required(ErrorMessage = "*")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password not match")]
        [NotMapped]
        //[DataType(DataType.Password)]

        public string confirm_Password { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Birth_Day { get; set; }

        //[Required(ErrorMessage = "*")]
        [StringLength(int.MaxValue)]

        public string UImage { get; set; }
        [Range(18, 100, ErrorMessage = ("Age must be between 18 and 100"))]

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? Age { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression("(01)[0-9]{9}", ErrorMessage = "invalid phone")]
        public string Phone { get; set; }
        [StringLength(10)]
        public string userRole { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Drug_Buying> Drug_Buying { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Has_Drug> Has_Drug { get; set; }
    }
}
