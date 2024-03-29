﻿using System;
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

namespace Main
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    /// Developed by Giovanni Lenguito
    /// University: Staffordshire University
    /// Student ID: L010516C
    public partial class Welcome : Window
    {
        public Welcome()
        {
            InitializeComponent();
        }
        
        private void welcomeCheck_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (welcomeCheck.IsChecked == false)
            {
                Properties.Settings.Default.welcome = false;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.welcome = true;
                Properties.Settings.Default.Save();
            }
        }
    }
}
