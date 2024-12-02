using PdfSharp.Pdf.Content.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using WpfNed.EF;

namespace WpfNed.DTO
{
    public class ReservationDTO
    {
        public ReservationDTO() { }
        public ReservationDTO(Reservation r) 
        {
            Id = r.Id;
            ObjectId = r.ObjectId;
            UserId = r.UserId;
            StartDate = r.StartDate;
            EndDate = r.EndDate;
            ResStatusId = r.ResStatusId;
            Object = r.Object;
            ResStatus = r.ResStatus;
            User = r.User;
        }
        public ReservationDTO(ReservationDTO other)
        {
            Id = other.Id;
            ObjectId = other.ObjectId;
            UserId = other.UserId;
            StartDate = other.StartDate;
            EndDate = other.EndDate;
            ResStatusId = other.ResStatusId;
            Object = other.Object;
            ResStatus = other.ResStatus;
            User = other.User;
            other.ObjectDisplay = other.ObjectDisplay;
            other.UserDisplay = other.UserDisplay;
            other.ResStatusDisplay = other.ResStatusDisplay;
        }
        public int Id { get; set; }

        public int ObjectId { get; set; }
        public string ObjectDisplay { get; set; }

        public int UserId { get; set; }
        public string UserDisplay { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int ResStatusId { get; set; }
        public string ResStatusDisplay { get; set; }

        public virtual REObject Object { get; set; }

        public virtual ResStatus ResStatus { get; set; }

        public virtual User User { get; set; }
    }
}
