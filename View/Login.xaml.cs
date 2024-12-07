using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfNed.Services;
using WpfNed.ViewModel;

namespace WpfNed.View
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            var windowService = new WindowService();
            DataContext = new LoginViewModel(windowService);
        }
        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //private void btnLogin_Click(object sender, RoutedEventArgs e)
        //{
        //    MWindow mainWindow = new MWindow();
        //    mainWindow.Show();
        //    this.Close();
        //}
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                ((LoginViewModel)DataContext).Password = passwordBox.Password;
            }
        }
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            tbFullname.Visibility = Visibility.Visible;
            txtFullname.Visibility = Visibility.Visible;
            ((TextBox)txtFullname.Child).Text = string.Empty;
            btnAlreadyHave.Visibility = Visibility.Visible;
            btnSignUp.Visibility = Visibility.Visible; 
            btnRegister.Visibility = Visibility.Collapsed;
            btnLogin.Visibility = Visibility.Collapsed;
            
        }

        private void btnAlreadyHave_Click(object sender, RoutedEventArgs e)
        {
            tbFullname.Visibility = Visibility.Collapsed;
            txtFullname.Visibility = Visibility.Collapsed;
            btnAlreadyHave.Visibility = Visibility.Collapsed;
            btnSignUp.Visibility = Visibility.Collapsed;
            btnRegister.Visibility = Visibility.Visible;
            btnLogin.Visibility = Visibility.Visible;
        }

        //private void btnSignUp_Click(object sender, RoutedEventArgs e)
        //{
        //    //MWindow mainWindow = new MWindow();
        //    //mainWindow.Show();
        //    //this.Close();
        //}
    }
}
