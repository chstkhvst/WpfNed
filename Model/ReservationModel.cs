using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNed.EF;

namespace WpfNed.Model
{
    public class ReservationModel
    {
        Model1 db = new Model1();
        public void DeleteRes(Reservation obj)
        {
            var existingObject = db.Reservation.FirstOrDefault(o => o.Id == obj.Id);
            db.Reservation.Remove(obj);
            db.SaveChanges();
        }
    }
}
