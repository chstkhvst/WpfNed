namespace WpfNed.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Contract = new HashSet<Contract>();
            Reservation = new HashSet<Reservation>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        public int RoleId { get; set; }

        [StringLength(10)]
        public string Passport { get; set; }

        [StringLength(11)]
        public string Phone { get; set; }

        [Required]
        [StringLength(25)]
        public string Login { get; set; }

        [Required]
        [StringLength(10)]
        public string Password { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contract> Contract { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservation { get; set; }

        public virtual UserRole UserRole { get; set; }
    }
}
