using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
namespace Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int count = 0;
        public MainWindow()
        {
            InitializeComponent();
        }
        private System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();

        private void getReportBtn_Click(object sender, RoutedEventArgs e)
        {
            reportGenText.SelectAll();
            reportGenText.Selection.Text = "Full Report\n";
        }

        public List<string[]> parseCSVFile(string path)
        {
            List<string[]> parsedFile = new List<string[]>();

            try
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    string line;
                    string[] row;

                    while ((line = readFile.ReadLine()) != null)
                    {
                        row = line.Split(',');
                        parsedFile.Add(row);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("There was a problem with parsing the CSV file");
            }

            return parsedFile;
        }

        private void selectFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                listOfFiles.Items.Clear();
                Thread getFThread = new Thread(new ThreadStart(getFiles));
                getFThread.Start();
            }
        }
        private void getFiles()
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                
                string[] files = Directory.GetFiles(folderDialog.SelectedPath);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);

                    if (Path.GetExtension(file) == ".csv")
                    {
                        ListViewItem listItem = new ListViewItem();

                        listItem.Content = fileName;
                        listItem.Tag = file;
                        listOfFiles.Items.Add(listItem);
                        count++;
                        //parseCSVFile(file);
                    }
                    if (listOfFiles.Items.Count == 0)
                    {
                        listOfFiles.Items.Add("No CSV Files Found");
                        MessageBox.Show("No CSV Files Found");
                    }

                    itemsCount.Content = Convert.ToString(count);
                    stopWatch.Stop();

                    timeTaken.Content = Convert.ToString(stopWatch.Elapsed);
                }
            });
        }
        private void printDataBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("No Data To Print");
        }
        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
        }
        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void clearReportBtn_Click(object sender, RoutedEventArgs e)
        {
            reportGenText.SelectAll();
            reportGenText.Selection.Text = "";
        }
        
    }
}
