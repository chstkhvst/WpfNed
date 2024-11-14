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
        public void OpenWindow(string windowType, object vM, int mode)
        {
            Window window;

            switch (windowType)
            {
                case "AddUser":
                    if (mode == 1)
                    {
                        window = new AddUserWindow(1, vM);
                    }
                    else
                    {
                        window = new AddUserWindow(2, vM);
                    }
                    break;
                default:
                    throw new ArgumentException("Unknown window type");
            }

            window.ShowDialog();
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
