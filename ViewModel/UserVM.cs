using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WpfNed.Model;
using WpfNed.EF;
using WpfNed.Services;
using System.Collections.ObjectModel;
using WpfNed.DTO;
using System.ComponentModel.DataAnnotations;

namespace WpfNed.ViewModel
{
    public class UserVM : INotifyPropertyChanged
    {
        TableModel tb = new TableModel();
        UserModel ut = new UserModel();
        private List<UserDTO> _users;
        IWindowService _windowService;
        public ICommand UpdObjInDBCommand { get; }
        public ICommand AddObjInDBCommand { get; }
        public ICommand AddObjCommand { get; }
        public ICommand DeleteObjCommand { get; }
        public ICommand RefreshObjCommand { get; }
        public ICommand EditObjCommand { get; }

        //public string FullName { get; set; }
        //public int RoleId { get; set; }
        //public string Passport { get; set; }
        //public string Phone { get; set; }
        //public string Login { get; set; }
        //public string Password { get; set; }
        public UserVM()
        {
            _windowService = new WindowService();
            AddObjInDBCommand = new RelayCommand(AddUser);
            UpdObjInDBCommand = new RelayCommand(UpdateUser);
            AddObjCommand = new RelayCommand(OpenAddUser);
            EditObjCommand = new RelayCommand(OpenUpdUser);
            DeleteObjCommand = new RelayCommand(DeleteSelectedObject);
            RefreshObjCommand = new RelayCommand(RefreshObjects);
        }
        private List<UserRole> _userRoles;
        public List<UserRole> UserRoles
        {
            get
            {
                if (_userRoles == null)
                {
                    _userRoles = tb.GetUserRoles();
                    OnPropertyChanged(nameof(UserRoles));
                }
                return _userRoles;
            }
            set
            {
                if (_userRoles != value)
                {
                    _userRoles = value;
                    OnPropertyChanged(nameof(UserRoles));
                }
            }
        }
        public List<UserDTO> Users
        {
            get
            {
                if (_users == null)
                {
                    _users = tb.GetUsersDTO()/*.Select(i => new UserDTO(i)).ToList()*/;
                    //_users = /*new ObservableCollection<UserDTO>*/(us.Select(u => new UserDTO(u)));
                    OnPropertyChanged(nameof(Users));
                }
                return _users;
            }
            set
            {
                if (_users != value)
                {
                    _users = value;
                    OnPropertyChanged(nameof(Users));
                }
            }
        }
        private UserDTO _selectedUser;
        public UserDTO SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        public void OpenAddUser()
        {
            //_windowService.ShowWindow("AddUser", this);
            SelectedUser = new UserDTO();
            _windowService.OpenWindow("AddUser", this, 1);
        }
        public void OpenUpdUser()
        {
            if (SelectedUser != null)
            {
                SelectedUser = new UserDTO(SelectedUser);
                _windowService.OpenWindow("AddUser", this, 2);
            }         
        }
        public void RefreshObjects()
        {
            var us = tb.GetUsersDTO();
            Users.Clear();
            Users = tb.GetUsersDTO()/*.Select(i => new UserDTO(i)).ToList()*/;
            OnPropertyChanged("Users");
        }
        private void DeleteSelectedObject()
        {
            if (Users != null)
            {
                ut.DeleteObject(SelectedUser);
                Users.Remove(SelectedUser);
                RefreshObjects();
            }
        }
        private void UpdateUser()
        {
            ut.UpdateUser(SelectedUser);
            RefreshObjects();
        }
        public void AddUser()
        {
            //var newObject = new UserDTO
            //{
            //    FullName = this.FullName,
            //    Login = this.Login,
            //    Password = this.Password,
            //    Passport = this.Passport,
            //    RoleId = (int)this.RoleId,
            //    Phone = this.Phone,
            //};
            //newObject.UserRole = UserRoles.FirstOrDefault(r => r.Id == newObject.RoleId);
            ut.AddObj(SelectedUser);
            //Users = null;
            //Users.Add(newObject);
            RefreshObjects();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
