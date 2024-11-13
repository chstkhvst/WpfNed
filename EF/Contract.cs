namespace WpfNed.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contract")]
    public partial class Contract
    {
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime SignDate { get; set; }

        public int ReservationId { get; set; }

        public int UserId { get; set; }

        public int Total { get; set; }

        public virtual Reservation Reservation { get; set; }

        public virtual User User { get; set; }
    }
}
