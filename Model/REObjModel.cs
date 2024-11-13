using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNed.EF;
using WpfNed.DTO;
using WpfNed.ViewModel;
using System.Data.Entity;

namespace WpfNed.Model
{
    using RealEstateObject = WpfNed.EF.REObject;
    public class REObjModel
    {
        Model1 db = new Model1();
        public void AddObj(REObjectDTO o)
        {
            var newObject = new REObject
            {
                Rooms = o.Rooms,
                Floors = o.Floors,
                Square = o.Square,
                TypeId = o.TypeId,
                DealTypeId = o.DealTypeId,
                Street = o.Street,
                Building = o.Building,
                Number = o.Number,
                Price = o.Price,
                OwnerId = o.OwnerId,
                StatusId = o.StatusId
            };
            db.Object.Add(newObject);
            db.SaveChanges();
        }
        public void DeleteObject(RealEstateObject obj)
        {
            var existingObject = db.Object.FirstOrDefault(o => o.Id == obj.Id);
            db.Object.Remove(existingObject);
            db.SaveChanges();
        }
        public void UpdObj(REObjectDTO updatedObject)
        {
            var existingObject = db.Object.FirstOrDefault(obj => obj.Id == updatedObject.Id);

            if (existingObject != null)
            {
                existingObject.Rooms = updatedObject.Rooms;
                existingObject.Floors = updatedObject.Floors;
                existingObject.Square = updatedObject.Square;
                existingObject.TypeId = updatedObject.TypeId;
                existingObject.DealTypeId = updatedObject.DealTypeId;
                existingObject.Street = updatedObject.Street;
                existingObject.Building = updatedObject.Building;
                existingObject.Number = updatedObject.Number;
                existingObject.Price = updatedObject.Price;
                existingObject.OwnerId = updatedObject.OwnerId;
                existingObject.StatusId = updatedObject.StatusId;

                db.SaveChanges();
                db.Owner.Load();
                db.Object.Load();
            }
        }
    }
}
