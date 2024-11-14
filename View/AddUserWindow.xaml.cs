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

namespace WpfNed.View
{
    /// <summary>
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public AddUserWindow(int mode, object vM)
        {
            InitializeComponent();
            if (mode == 1)
            {
                this.DataContext = vM;
                btn.Content = "Добавить";
                btn.SetBinding(Button.CommandProperty, new Binding("AddObjInDBCommand"));
            }
            else {
                this.DataContext = vM;
                btn.Content = "Редактировать";
                btn.SetBinding(Button.CommandProperty, new Binding("UpdObjInDBCommand"));
            }
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
            //EmployeeWindow em = new EmployeeWindow();
            //em.Show();
            this.Close();
        }

    }
}
