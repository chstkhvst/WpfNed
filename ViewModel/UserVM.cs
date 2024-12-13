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
using System.Text.RegularExpressions;
using System.Windows;

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
            AddObjInDBCommand = new RelayCommand<Window>(AddUser);
            UpdObjInDBCommand = new RelayCommand<Window>(UpdateUser);
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
        private void UpdateUser(Window w)
        {
            if (string.IsNullOrEmpty(SelectedUser.FullName) || SelectedUser.FullName.Length > 35 || !Regex.IsMatch(SelectedUser.FullName, @"^[а-яА-Яa-zA-Z\s]+$"))
                MessageBox.Show("Пожалуйста, введите ФИО корректно.", "Ошибка!");
            else if (string.IsNullOrEmpty(SelectedUser.Password) || SelectedUser.Password.Length > 10)
                MessageBox.Show("Пожалуйста, введите пароль длиной до 10 символов.", "Ошибка!");
            else if (string.IsNullOrEmpty(SelectedUser.Login) || SelectedUser.Login.Length > 10)
                MessageBox.Show("Пожалуйста, введите логин длиной до 10 символов.", "Ошибка!");
            else if (SelectedUser.Passport != null && (SelectedUser.Passport.Length != 10))
                MessageBox.Show("Пожалуйста, введите корректно паспортные данные.", "Ошибка!");
            else if (SelectedUser.Phone != null && (SelectedUser.Phone.Length != 11))
                MessageBox.Show("Пожалуйста, введите корректно номер телефона.", "Ошибка!");
            else if (SelectedUser.RoleId == 0)
                MessageBox.Show("Пожалуйста, выберите роль пользователя.", "Ошибка!");
            else
            {
                ut.UpdateUser(SelectedUser);
                RefreshObjects();
                w.Close();
            }
        }
        public void AddUser(Window w)
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
            if (string.IsNullOrEmpty(SelectedUser.FullName) || SelectedUser.FullName.Length > 35 || !Regex.IsMatch(SelectedUser.FullName, @"^[а-яА-Яa-zA-Z\s]+$"))
                MessageBox.Show("Пожалуйста, введите ФИО корректно.", "Ошибка!");
            else if (string.IsNullOrEmpty(SelectedUser.Password) || SelectedUser.Password.Length > 10)
                MessageBox.Show("Пожалуйста, введите пароль длиной до 10 символов.", "Ошибка!");
            else if (string.IsNullOrEmpty(SelectedUser.Login) || SelectedUser.Login.Length > 10)
                MessageBox.Show("Пожалуйста, введите логин длиной до 10 символов.", "Ошибка!");
            else if (SelectedUser.Passport != null && (SelectedUser.Passport.Length != 10))
                MessageBox.Show("Пожалуйста, введите корректно паспортные данные.", "Ошибка!");
            else if (SelectedUser.Phone != null && (SelectedUser.Phone.Length != 11))
                MessageBox.Show("Пожалуйста, введите корректно номер телефона.", "Ошибка!");
            else if (SelectedUser.RoleId == 0)
                MessageBox.Show("Пожалуйста, выберите роль пользователя.", "Ошибка!");
            else
            {
                ut.AddObj(SelectedUser);
                //Users = null;
                //Users.Add(newObject);
                RefreshObjects();
                w.Close();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
