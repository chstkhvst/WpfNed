using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNed.DTO;
using WpfNed.EF;

namespace WpfNed.Model
{
    public class UserModel
    {
        Model1 db = new Model1();
        public void AddObj(UserDTO o)
        {
            var newObject = new User
            {
                Login = o.Login,
                Passport = o.Passport,
                Password    = o.Password,
                FullName = o.FullName,
                RoleId = o.RoleId,
                Phone = o.Phone,
            };
            db.User.Add(newObject);
            db.SaveChanges();
        }
        public void DeleteObject(UserDTO obj)
        {
            //var existingObject = db.User.FirstOrDefault(o => o.Id == obj.Id);
            var existingObject = db.User.Find(obj.Id);
            db.User.Remove(existingObject);
            db.SaveChanges();
        }
        public void UpdateUser(UserDTO updatedObject)
        {
            var existingObject = db.User.FirstOrDefault(u => u.Id == updatedObject.Id);
            if (existingObject != null)
            {
                existingObject.FullName = updatedObject.FullName;
                existingObject.Passport = updatedObject.Passport;
                existingObject.Password = updatedObject.Password;
                //existingObject.UserRole = updatedObject.UserRole;
                existingObject.RoleId = updatedObject.RoleId;
                existingObject.Login = updatedObject.Login;
                existingObject.Phone = updatedObject.Phone;

                db.SaveChanges();
            }
        }
    }
}
