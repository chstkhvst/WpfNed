using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfNed.EF;

namespace WpfNed.DTO
{
    public class UserDTO
    {
        
        public UserDTO() { }
        public UserDTO(User u)
        {
            Id = u.Id;
            FullName = u.FullName;
            RoleId = u.RoleId;
            Passport = u.Passport;
            Login = u.Login;
            Phone = u.Phone;
            Password = u.Password;
            UserRole = u.UserRole;
        }
        public UserDTO(UserDTO other)
        {
            if (other != null)
            {
                Id = other.Id;
                FullName = other.FullName;
                RoleId = other.RoleId;
                Passport = other.Passport;
                Login = other.Login;
                Phone = other.Phone;
                Password = other.Password;
                UserRole = other.UserRole;
                other.DisplayRole = other.DisplayRole;
            }
        }
        public string DisplayRole { get; set; }
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        public int RoleId { get; set; }

        [StringLength(10)]
        public string Passport { get; set; }

        [StringLength(11)]
        public string Phone { get; set; }

        [Required]
        [StringLength(25)]
        public string Login { get; set; }

        [Required]
        [StringLength(10)]
        public string Password { get; set; }

        public virtual UserRole UserRole { get; set; }
    }
}
