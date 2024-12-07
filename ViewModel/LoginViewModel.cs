using Microsoft.Win32;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfNed.EF; 
using WpfNed.Model;
using WpfNed.Services;
using WpfNed.View;
using WpfNed.DTO;

namespace WpfNed.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private IWindowService _windowService;
        private TableModel _tableModel;
        private UserModel um;
        private string _username;
        private string _password;
        private string _fullname;
        private bool _isAuthenticated;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string Fullname
        {
            get => _fullname;
            set
            {
                _fullname = value;
                OnPropertyChanged(nameof(Fullname));
            }
        }

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            private set
            {
                _isAuthenticated = value;
                OnPropertyChanged(nameof(IsAuthenticated));
            }
        }

        public LoginViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            _tableModel = new TableModel();
            um = new UserModel();
            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(Register);
        }

        private void Login()
        {
            var us = _tableModel.ValidateUser(Username, Password);
            if (us != null)
            {
                if (us.RoleId == 1)
                {
                    Window win = new EmployeeWindow();
                    var eViewModel = new SharedVM();
                    //Window win = new EmployeeWindow();
                    //var eViewModel = new EmployeeViewModel();
                    _windowService.ShowWindow("Employee", eViewModel);

                }
                else
                {
                    Window win = new MWindow();
                    var mainViewModel = new MWViewModel();
                    _windowService.ShowWindow("Main", mainViewModel);
                }
                IsAuthenticated = true;
                var currentWindow = Application.Current.Windows.OfType<Login>().FirstOrDefault();
                _windowService.CloseWindow(currentWindow);
            }
            else
            {
                IsAuthenticated = false;
                MessageBox.Show("Неверный логин или пароль.");
            }
        }
        private void Register()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Fullname) || !_tableModel.CheckUsers(Username))
            {
                MessageBox.Show("Регистрация невозможна.");
            }
            else
            {
                var newUser = new UserDTO
                {
                    Login = Username,
                    Password = Password,
                    FullName = Fullname,
                    RoleId = 2
                };

                um.AddObj(newUser);

                MessageBox.Show("Регистрация прошла успешно!");

                Username = string.Empty;
                Password = string.Empty;
                Fullname = string.Empty;
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}