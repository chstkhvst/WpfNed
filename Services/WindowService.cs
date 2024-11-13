using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfNed.View;

namespace WpfNed.Services
{
    public class WindowService : IWindowService
    {
        public void ShowWindow(string windowType, object viewModel)
        {
            Window window;

            switch (windowType)
            {
                case "Main":
                    window = new MWindow();
                    window.DataContext = viewModel; 
                    break;
                case "Employee":
                    window = new EmployeeWindow();
                    break;
                case "AddObj":
                    window = new AddObjWindow();
                    window.DataContext = viewModel;
                    break;
                case "EditObj":
                    window = new EditObjectWindow();
                    window.DataContext = viewModel;
                    break;
                default:
                    throw new ArgumentException("Unknown window type");
            }

            //window.DataContext = viewModel;
            window.Show();
        }
        public void OpenWindow(string windowType)
        {
            Window window;

            switch (windowType)
            {
                case "AddObj":
                    window = new AddObjWindow();
                    break;
                case "Employee":
                    window = new EmployeeWindow();
                    break;
                default:
                    throw new ArgumentException("Unknown window type");
            }

            window.Show();
        }
        public void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
