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
using WpfNed.EF;
using WpfNed.Model;


namespace WpfNed.View
{
    /// <summary>
    /// Логика взаимодействия для MWindow.xaml
    /// </summary>
    using WpfNed.ViewModel;
    public partial class MWindow : Window
    {
        public MWindow()
        {
            InitializeComponent();
            this.Language = System.Windows.Markup.XmlLanguage.GetLanguage("ru-RU");
            DataContext = new MWViewModel();
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

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

