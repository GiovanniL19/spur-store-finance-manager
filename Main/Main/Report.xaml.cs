using Microsoft.Win32;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        
        public MainWindow()
        {
            InitializeComponent();
            if (Properties.Settings.Default.welcome == true)
            {
                Welcome welcomeScreen = new Welcome();
                welcomeScreen.Show();
            }
        }
        public Folder folder;
        public GenerateReport gen = new GenerateReport();
        public Command command;

        public List<Order> result;
        public List<Report> report;

        public Stopwatch sW = new Stopwatch();

        //Filters
        public int weeksSelection = 0;
        public string year = "All Years";
        public string store;
        public string supplier;
        public string type;

        public bool view;

        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            if (result == null)
            {
                MessageBox.Show("Please Select a Folder", "Warning" , MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                reportGridGen.ClearValue(ItemsControl.ItemsSourceProperty);
                reportGridGen.Items.Clear();
                reportGridGen.Items.Refresh();
                gen.resetTotal();

                if (report != null)
                {
                    report = null;
                }
                gen.GenerateTheReport(weeksSelection, result.ToList(), store, year, supplier, type);

                Task watchReport = new Task(this.Report_Update);
                watchReport.Start();
            }
        }

        private void Report_Update()
        {
            gen.reportGenTask.Wait();
            report = new List<Report>(gen.reportGenTask.Result);
            if (report.Count > 0)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    numberOfReport.Content = report.Count().ToString();
                    
                    gTotal.Content = gen.getgTotal();
                    weekTotal.Content = gen.getWeekTotal();
                    yearTotal.Content = gen.getYearTotal();
                    storeTotal.Content = gen.getStoreTotal();
                    selectedTotal.Content = gen.getSelectedTotal();
                    
                    reportGridGen.ItemsSource = report;
                }));
            }
            else
            {
                MessageBox.Show("No Results Found, be sure all the filters have been selected correctly", "No Results Found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void selectFolder()
        {
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                status.Content = "Loading...";
                supplierList.Items.RemoveAt(0);
                supplierTypeList.Items.RemoveAt(0);

                folder = new Folder();

                sW.Start();
                folder.getFiles(folderDialog.SelectedPath);

                Task watcherTask = new Task(this.GUI_Update);
                watcherTask.Start();
            }
        }
        private void selectFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            selectFolder();
        }

        private void GUI_Update()
        {
            folder.getFilesTask.Wait();
            
            result = new List<Order>(folder.getFilesTask.Result);
            HashSet<string> suppliers = new HashSet<string>();
            HashSet<string> type = new HashSet<string>();

            Parallel.For (0, result.Count(), i =>
            {
                if (result[i].Supplier != null && suppliers.Contains(result[i].Supplier) || result[i].Type != null && type.Contains(result[i].Type))
                {
                    suppliers.Add(result[i].Supplier);
                    type.Add(result[i].Type);
                }
            });
            Dispatcher.Invoke(new Action(() =>{
                itemsParsedCount.Content = result.Count();
                reportGrid.ItemsSource = result;

                CBItem itemSupplier = new CBItem();
                itemSupplier.Content = "Select a Supplier";
                supplierList.Items.Add(itemSupplier);
                supplierList.SelectedItem = itemSupplier;

                CBItem itemType = new CBItem();
                itemType.Content = "Select Supplier Type";
                supplierTypeList.Items.Add(itemType);
                supplierTypeList.SelectedItem = itemType;
            
                foreach(var supplierItem in suppliers)
                {
                    CBItem itemAdd = new CBItem();
                    itemAdd.Content = supplierItem;
                    supplierList.Items.Add(itemAdd);
                }

                foreach (var typeItem in type)
                {
                    CBItem itemAdd = new CBItem();
                    itemAdd.Content = typeItem;   
                    supplierTypeList.Items.Add(itemAdd);
                }
                sW.Stop();
                status.Content = "Complete";
                timeTaken.Content = sW.Elapsed;
            }));
        }

        private void printDataBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("There are no reports to print", "Print", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Settings feature currently unavailable", "Settings", MessageBoxButton.OK, MessageBoxImage.Stop);
        }
        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }
        
        private void Save()
        {
            MessageBox.Show("Save feature currently unavailable", "Save", MessageBoxButton.OK, MessageBoxImage.Stop);
        }

        private void weeks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            weeksSelection = weeks.SelectedIndex;
        }

        private void storesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try{store = (storesList.SelectedItem as CBItem).Value.ToString();}catch(Exception){}
        }

        public void selectStore()
        {
            OpenFileDialog storeFileDialog = new OpenFileDialog();

            if (storeFileDialog.ShowDialog() == true)
            {
                StreamReader readFile = new StreamReader(storeFileDialog.FileName);

                storesList.Items.RemoveAt(0);
                string line;

                try
                {
                    if (Path.GetFileName(storeFileDialog.FileName) == "StoreCodes.csv")
                    {
                        CBItem AllStores = new CBItem();
                        AllStores.Content = "Select a Store";

                        storesList.Items.Add(AllStores);

                        storesList.SelectedItem = AllStores;

                        while ((line = readFile.ReadLine()) != null)
                        {
                            String[] split = line.Split(',');

                            CBItem item = new CBItem();
                            item.Value = split[0];
                            item.Content = split[1];

                            storesList.Items.Add(item);
                        }
                    }
                    else
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            MessageBox.Show("Please select file named: StoreCodes.csv");
                        }));
                    }
                }
                catch (Exception) { Dispatcher.Invoke(new Action(() => { MessageBox.Show("Error loading stores"); })); }
            } 
        }
        private void loadStores_Click(object sender, RoutedEventArgs e)
        {
            selectStore();
        }

        private void CalculatorBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process calc = System.Diagnostics.Process.Start("calc.exe");
            calc.WaitForInputIdle();
        }

        private void yearsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem) yearsList.SelectedItem).Content != null)
            {
                year = (yearsList.SelectedItem as ComboBoxItem).Content.ToString();
            }
        }

        private void aboutBtn_Click(object sender, RoutedEventArgs e)
        {
            About ab = new About();
            ab.Show();
        }

        private void supplierList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try { supplier = supplierList.SelectedItem.ToString(); }
            catch (Exception) { }
        }

        private void supplierTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try { type = supplierTypeList.SelectedItem.ToString(); }
            catch (Exception) { }
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            Welcome help = new Welcome();
            help.Show();
        }
    }
}