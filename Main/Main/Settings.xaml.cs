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
            folderPathText.Text = Properties.Settings.Default.path;
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
            Properties.Settings.Default.Save();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown(); 
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.path = null;
            Properties.Settings.Default.welcome = true;
            Properties.Settings.Default.parallel = true;
            Properties.Settings.Default.Save();
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown(); 
        }

    }
}
