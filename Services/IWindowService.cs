﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfNed.Services
{
    public interface IWindowService
    {
        void ShowWindow(string windowType, object viewModel = null);
        void OpenWindow(string windowType);
        void CloseWindow(Window window);
    }
}