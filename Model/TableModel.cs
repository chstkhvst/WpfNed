using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfNed.DTO;
using WpfNed.EF;

namespace WpfNed.Model
{
    using RealEstateObject = WpfNed.EF.REObject;
    using RealEstateTypeObject = WpfNed.EF.ObjectType;
    public class TableModel
    {
        Model1 db = new Model1();
        //public List<RealEstateObject> GetObjects()
        //{
        //    db.Set<RealEstateObject>().Load();
        //    return db.Set<RealEstateObject>().ToList();
        //}
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
        public List<UserRole> GetUserRoles()
        {
            db.Set<UserRole>().Load();
            return db.Set<UserRole>().ToList();
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
        //public List<User> GetUsers()
        //{
        //    //db.User.Load();
        //    //return db.User.ToList();
        //    return db.User.Include(u => u.UserRole).ToList();
        //    //return db.User.AsNoTracking().Include(u => u.UserRole).ToList();
        //}
        public List<RealEstateObject> GetObjects()
        {
            return db.Object.Include(o => o.Owner) // Загружаем владельцев
                            .Include(o => o.ObjectType)
                            .Include(o => o.DealType)
                            .Include(o => o.Status)
                            .ToList();
        }
        public List<UserDTO> GetUsersDTO()
        {
            using (var db = new Model1())
            {
                db.User.Load();
                List<UserDTO> r = db.User.ToList().Select(i => new UserDTO(i)).ToList();
                for (int i = 0; i < r.Count; i++)
                {
                    r[i].DisplayRole = db.UserRole.Find(r[i].RoleId).RoleName;
                }
                return r;
            }
        }
        public List<REObjectDTO> GetObjectsDTO()
        {
            using (var db = new Model1())
            {
                db.Object.Load();
                List<REObjectDTO> r = db.Object.ToList().Select(i => new REObjectDTO(i)).ToList();
                for (int i = 0; i < r.Count; i++)
                {
                    r[i].DealTypeDisplay = db.DealType.Find(r[i].DealTypeId).DealName;
                    r[i].StatusDisplay = db.Status.Find(r[i].StatusId).StatusName;
                    r[i].OwnerDisplay = db.Owner.Find(r[i].OwnerId).FullName;
                    r[i].TypeDisplay = db.ObjectType.Find(r[i].TypeId).TypeName;
                    r[i].Images = GetObjectImages(r[i].Id);
                }

                return r;
            }
        }
        public List<ContractDTO> GetContractsDTO()
        {
            using (var db = new Model1())
            {
                db.Contract.Load();
                List<ContractDTO> r = db.Contract.ToList().Select(i => new ContractDTO(i)).ToList();
                for (int i = 0; i < r.Count; i++)
                {
                    r[i].DisplayReservationUs = db.Reservation.Find(r[i].ReservationId).User.FullName;
                    r[i].DisplayReservationAd = db.Reservation.Find(r[i].ReservationId).Object.Street;
                    r[i].DisplayReservationOw = db.Reservation.Find(r[i].ReservationId).Object.Owner.FullName;
                    r[i].DisplayUser = db.User.Find(r[i].UserId).FullName;
                }
                return r;
            }
        }
        public List<ImageSource> GetObjectImages(int id)
        {
            List<byte[]> lst = db.ObjectImage.Where(i  => i.ObjectId == id).Select(i => i.ObjImage).ToList();
            List<ImageSource> result = new List<ImageSource>();
            for (int i =0 ; i < lst.Count(); i++)
            {
                ImageSource img = (ImageSource)new ImageSourceConverter().ConvertFrom(lst[i]);
                result.Add(img);
            }
            return result;
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
