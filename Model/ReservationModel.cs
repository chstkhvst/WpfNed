using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfNed.DTO;
using WpfNed.EF;

namespace WpfNed.Model
{
    public class ReservationModel
    {
        Model1 db = new Model1();
        public void DeleteRes(Reservation obj)
        {
            var existingObject = db.Reservation.Find(obj.Id);
            var resObject = db.Object.Find(obj.ObjectId);
            if (existingObject != null)
            {
                resObject.StatusId = 1;
                db.Reservation.Remove(existingObject);
                db.SaveChanges();
            }
        }
        public void AddRes(ReservationDTO r)
        {
            var newObject = new Reservation()
            {
                UserId = r.UserId,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                ObjectId = r.ObjectId,
                ResStatusId = 1
            };
            db.Reservation.Add(newObject);
            var resObj = db.Object.FirstOrDefault(u => u.Id == r.ObjectId);
            if (resObj != null) resObj.StatusId = 2;
            db.SaveChanges();
        }
        public int FindUser(string fio)
        {
            var existingUser = db.User.FirstOrDefault(u => u.FullName == fio);
            if (existingUser == null) return 0;
            return existingUser.Id;
        }
    }
}
