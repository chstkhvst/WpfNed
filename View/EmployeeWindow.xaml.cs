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
using WpfNed.ViewModel;

namespace WpfNed.View
{
    /// <summary>
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        public EmployeeWindow()
        {
            InitializeComponent();

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

        private void ButtonRes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonUsers_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonContr_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSprav_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAddObj_Click(object sender, RoutedEventArgs e)
        {
            Window window = new AddObjWindow();
            window.Show();
            //this.Close();
        }
    }
}
