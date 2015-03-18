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
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            if (Properties.Settings.Default.welcome == true)
            {
                welcomeCheckBtn.IsChecked = true;
            }
            else
            {
                welcomeCheckBtn.IsChecked = false;
            }

            if (Properties.Settings.Default.parallel == true)
            {
                parallelCheckBtn.IsChecked = true;
            }
            else
            {
                parallelCheckBtn.IsChecked = false;
            }

            if (Properties.Settings.Default.graphPopUp == true)
            {
                graphCheckBtn.IsChecked = true;
            }
            else
            {
                graphCheckBtn.IsChecked = false;
            }

            folderPathText.Text = Properties.Settings.Default.path;
            storesPathText.Text = Properties.Settings.Default.storesPath;
        }

        private void selectPath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.path = folderDialog.SelectedPath;
                folderPathText.Text = folderDialog.SelectedPath;
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (welcomeCheckBtn.IsChecked == true)
            {
                Properties.Settings.Default.welcome = true;
            }
            else
            {
                Properties.Settings.Default.welcome = false;
            }

            if (parallelCheckBtn.IsChecked == true)
            {
                Properties.Settings.Default.parallel = true;
            }
            else
            {
                Properties.Settings.Default.parallel = false;
            }
            if (graphCheckBtn.IsChecked == true)
            {
                Properties.Settings.Default.graphPopUp = true;
            }
            else
            {
                Properties.Settings.Default.graphPopUp = false;
            }
            
            Properties.Settings.Default.Save();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown(); 
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.path = null;
            Properties.Settings.Default.storesPath = null;

            Properties.Settings.Default.welcome = true;
            Properties.Settings.Default.parallel = true;
            Properties.Settings.Default.graphPopUp = false;
            Properties.Settings.Default.Save();

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown(); 
        }

        private void selectStorePath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog storeFileDialog = new OpenFileDialog();

            if (storeFileDialog.ShowDialog() == true)
            {
                
                if (System.IO.Path.GetFileName(storeFileDialog.FileName) == "StoreCodes.csv")
                {
                    Properties.Settings.Default.storesPath = storeFileDialog.FileName;
                    storesPathText.Text = storeFileDialog.FileName;
                }
            }
        }
    }
}
