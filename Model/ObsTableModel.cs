using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using WpfNed.EF;

namespace WpfNed.Model
{
    using RealEstateObject = WpfNed.EF.REObject;
    using RealEstateTypeObject = WpfNed.EF.ObjectType;
    public class ObsTableModel
    {
        Model1 db = new Model1();

        public ObservableCollection<RealEstateObject> GetObjects()
        {
            db.Set<RealEstateObject>().Load();
            return new ObservableCollection<RealEstateObject>(db.Set<RealEstateObject>().Local);
        }

        public ObservableCollection<RealEstateTypeObject> GetObjectTypes()
        {
            db.Set<RealEstateTypeObject>().Load();
            return new ObservableCollection<RealEstateTypeObject>(db.Set<RealEstateTypeObject>().Local);
        }

        public ObservableCollection<ResStatus> GetResStatuses()
        {
            db.Set<ResStatus>().Load();
            return new ObservableCollection<ResStatus>(db.Set<ResStatus>().Local);
        }

        public ObservableCollection<Status> GetStatuses()
        {
            db.Set<Status>().Load();
            return new ObservableCollection<Status>(db.Set<Status>().Local);
        }

        public ObservableCollection<Owner> GetOwners()
        {
            db.Set<Owner>().Load();
            return new ObservableCollection<Owner>(db.Set<Owner>().Local);
        }

        public ObservableCollection<DealType> GetDealTypes()
        {
            db.DealType.Load();
            return new ObservableCollection<DealType>(db.DealType.Local);
        }

        public ObservableCollection<User> GetUsers()
        {
            db.User.Load();
            return new ObservableCollection<User>(db.User.Local);
        }

        public ObservableCollection<Reservation> GetReservations()
        {
            db.Reservation.Load();
            return new ObservableCollection<Reservation>(db.Reservation.Local);
        }

        public ObservableCollection<Contract> GetContracts()
        {
            db.Contract.Load();
            return new ObservableCollection<Contract>(db.Contract.Local);
        }

        public User ValidateUser(string login, string password)
        {
            return db.User.FirstOrDefault(u => u.Login == login && u.Password == password);
        }
    }
}
