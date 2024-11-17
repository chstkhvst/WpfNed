using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNed.DTO;
using WpfNed.EF;

namespace WpfNed.Model
{
    public class ContractModel
    {
        Model1 db = new Model1();
        public void AddObj(ContractDTO o)
        {
            var newContract = new Contract
            {
                ReservationId = o.ReservationId,
                UserId = o.UserId,
                SignDate = o.SignDate,
                Total = o.Total
            };
            db.Contract.Add(newContract);
            var reservation = db.Reservation.FirstOrDefault(r => r.Id == newContract.ReservationId);

            if (reservation != null)
            {
                reservation.ResStatusId = 2;
                db.SaveChanges();
            }
            db.SaveChanges();
        }
        public void DeleteObject(ContractDTO obj)
        {
            var existingObject = db.Contract.Find(obj.Id);
            db.Contract.Remove(existingObject);
            db.SaveChanges();
        }
        public void UpdObj(ContractDTO updatedObject)
        {
            var existingObject = db.Contract.FirstOrDefault(obj => obj.Id == updatedObject.Id);

            if (existingObject != null)
            {
                existingObject.UserId = updatedObject.UserId;
                existingObject.Total = updatedObject.Total;

                db.SaveChanges();
                //db.Owner.Load();
                //db.Object.Load();
            }
        }
    }
}
