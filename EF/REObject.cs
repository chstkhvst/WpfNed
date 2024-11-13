namespace WpfNed.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Object")]
    public partial class REObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public REObject()
        {
            ObjectImage = new HashSet<ObjectImage>();
            Reservation = new HashSet<Reservation>();
        }

        public int Id { get; set; }

        public int Rooms { get; set; }

        public int Floors { get; set; }

        public int Square { get; set; }

        public int TypeId { get; set; }

        public int DealTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Street { get; set; }

        public int Building { get; set; }

        public int? Number { get; set; }

        public int Price { get; set; }

        public int OwnerId { get; set; }

        public int StatusId { get; set; }

        public virtual DealType DealType { get; set; }

        public virtual Owner Owner { get; set; }

        public virtual Status Status { get; set; }

        public virtual ObjectType ObjectType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ObjectImage> ObjectImage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservation { get; set; }
    }
}
