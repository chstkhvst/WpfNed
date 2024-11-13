using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WpfNed.EF; 
using WpfNed.Model;
using WpfNed.Services;
using WpfNed.View;

namespace WpfNed.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private IWindowService _windowService;
        private TableModel _tableModel; 
        private string _username;
        private string _password;
        private bool _isAuthenticated;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoginCommand { get; }

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
            LoginCommand = new RelayCommand(Login);
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}