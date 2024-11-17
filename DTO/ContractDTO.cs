using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNed.EF;

namespace WpfNed.DTO
{
    public class ContractDTO
    {
        public ContractDTO() { }
        public ContractDTO(Contract c)
        {
            Id = c.Id;
            SignDate = c.SignDate;
            ReservationId = c.ReservationId;
            UserId = c.UserId;
            Total = c.Total;
            Reservation = c.Reservation;
            User = c.User;
        }
        public ContractDTO(ContractDTO other)
        {
            if (other == null)
            {
                Id = other.Id;
                SignDate = other.SignDate;
                ReservationId = other.ReservationId;
                UserId = other.UserId;
                Total = other.Total;
                Reservation= other.Reservation;
                User = other.User;
                other.DisplayReservationUs = other.DisplayReservationUs;
                other.DisplayReservationAd = other.DisplayReservationAd;
                other.DisplayReservationOw = other.DisplayReservationOw;
                other.DisplayUser = other.DisplayUser;
            }
        }
        public int Id { get; set; }

        public DateTime SignDate { get; set; }

        public int ReservationId { get; set; }
        public string DisplayReservationUs {  get; set; }
        public string DisplayReservationAd {  get; set; }
        public string DisplayReservationOw { get; set; }

        public int UserId { get; set; }
        public string DisplayUser { get; set; }

        public int Total { get; set; }

        public virtual Reservation Reservation { get; set; }

        public virtual User User { get; set; }
    }
}
