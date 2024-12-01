using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using WpfNed.EF;
using WpfNed.View;
using System.Windows.Media;

namespace WpfNed.DTO
{
    public class REObjectDTO
    {
        public REObjectDTO() { }
        public REObjectDTO(REObject o)
        {
            Id = o.Id;
            Rooms = o.Rooms;
            Floors = o.Floors;
            Square = o.Square;
            TypeId = o.TypeId;
            DealTypeId = o.DealTypeId;
            Street = o.Street;
            Building = o.Building;
            Number = o.Number;
            Price = o.Price;
            OwnerId = o.OwnerId;
            StatusId = o.StatusId;
            DealType = o.DealType;
            Owner = o.Owner;
            Status = o.Status;
            ObjectType = o.ObjectType;
        }
        public REObjectDTO(REObjectDTO other)
        {
            if (other != null)
            {
                Id = other.Id;
                Rooms = other.Rooms;
                Floors = other.Floors;
                Square = other.Square;
                TypeId = other.TypeId;
                DealTypeId = other.DealTypeId;
                Street = other.Street;
                Building = other.Building;
                Number = other.Number;
                Price = other.Price;
                OwnerId = other.OwnerId;
                StatusId = other.StatusId;
                DealType = other.DealType;
                Owner = other.Owner;
                Status = other.Status;
                ObjectType = other.ObjectType;
                other.TypeDisplay = other.TypeDisplay;
                other.DealTypeDisplay = other.DealTypeDisplay;
                other.StatusDisplay = other.StatusDisplay;
                other.OwnerDisplay = other.OwnerDisplay;
            }
        }
        public int Id { get; set; }

        public int Rooms { get; set; }

        public int Floors { get; set; }

        public int Square { get; set; }

        public int TypeId { get; set; }
        public string TypeDisplay { get; set; }

        public int DealTypeId { get; set; }
        public string DealTypeDisplay { get; set; }

        [Required]
        [StringLength(50)]
        public string Street { get; set; }

        public int Building { get; set; }

        public int? Number { get; set; }

        public int Price { get; set; }

        public int OwnerId { get; set; }
        public string OwnerDisplay { get; set; }

        public int StatusId { get; set; }
        public string StatusDisplay { get; set; }

        public virtual DealType DealType { get; set; }

        public virtual Owner Owner { get; set; }

        public virtual Status Status { get; set; }

        public virtual ObjectType ObjectType { get; set; }

        [NotMapped]
        public List<ImageSource> Images { get; set; }
    }
}
