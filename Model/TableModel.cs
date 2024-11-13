using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using WpfNed.EF;

namespace WpfNed.Model
{
    using RealEstateObject = WpfNed.EF.REObject;
    using RealEstateTypeObject = WpfNed.EF.ObjectType;
    public class TableModel
    {
        Model1 db = new Model1();
        public List<RealEstateObject> GetObjects()
        {
            db.Set<RealEstateObject>().Load();
            return db.Set<RealEstateObject>().ToList();
        }
       
        public List<RealEstateTypeObject> GetObjectTypes()
        {
            db.Set<RealEstateTypeObject>().Load();
            return db.Set<RealEstateTypeObject>().ToList();
        }
        public List<ResStatus> GetResStatuses()
        {
            db.Set<ResStatus>().Load();
            return db.Set<ResStatus>().ToList();
        }
        public List<Status> GetStatuses()
        {
            db.Set<Status>().Load();
            return db.Set<Status>().ToList();
        }
        public List<Owner> GetOwners()
        {
            db.Set<Owner>().Load();
            return db.Set<Owner>().ToList();
        }
        public List<DealType> GetDealTypes()
        {
            db.DealType.Load();
            return db.DealType.ToList();
        }
        public List<User> GetUsers()
        {
            db.User.Load();
            return db.User.ToList();
        }
        public List<Reservation> GetReservations()
        {
            db.Reservation.Load();
            return db.Reservation.ToList();
        }
        public List<Contract> GetContracts()
        {
            db.Contract.Load();
            return db.Contract.ToList();
        }
        public User ValidateUser(string login, string password)
        {
            return db.User.FirstOrDefault(u => u.Login == login && u.Password == password);
        }
    }
}
