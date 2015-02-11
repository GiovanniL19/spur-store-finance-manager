using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private System.Windows.Forms.FolderBrowserDialog fileDialog = new System.Windows.Forms.FolderBrowserDialog();

        private void getReportBtn_Click(object sender, RoutedEventArgs e)
        {
         reportGenText.AppendText("\n---------------------------Report---------------------------");
        }
        private void selectFolderBtn_Click(object sender, RoutedEventArgs e)
        {
           fileDialog.ShowDialog();
        }
        private void viewtBtn_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
