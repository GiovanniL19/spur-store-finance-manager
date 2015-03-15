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
    /// Developed by Giovanni Lenguito
    /// University: Staffordshire University
    /// Student ID: L010516C
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            if (Properties.Settings.Default.path != "")
            {
                loadPathBtn.Visibility = Visibility.Visible;
            }
            if (Properties.Settings.Default.welcome == true)
            {
                Welcome welcomeScreen = new Welcome();
                welcomeScreen.Show();
            }
            if (Properties.Settings.Default.parallel == true)
            {
                typeOfEx.Content = "Parallel";
            }
            else
            {
                typeOfEx.Content = "Not Parallel";
            }
            if (Properties.Settings.Default.storesPath != "")
            {
                selectStore(Properties.Settings.Default.storesPath);
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
                //Generate Report
                status.Content = "Generating...";
                reportGridGen.ClearValue(ItemsControl.ItemsSourceProperty);
                reportGridGen.Items.Clear();
                reportGridGen.Items.Refresh();
                gen.resetTotal();

                if (report != null)
                {
                    report = null;
                }

                //Send data to GenerateReport
                gen.GenerateTheReport(weeksSelection, result.ToList(), store, year, supplier, type);

                //Start the watchReport to wait for task to complete
                Task watchReport = new Task(this.Report_Update);
                watchReport.Start();
            }
        }

        private void Report_Update()
        {
            //Task will wait until the report is generate and that tasks returns a value
            gen.reportGenTask.Wait();
            //Store the returned data in a list full of Report objects
            report = new List<Report>(gen.reportGenTask.Result);

            if (report.Count > 0)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    //Prints report to labels and data grid
                    numberOfReport.Content = report.Count().ToString();
                    
                    gTotal.Content = gen.getgTotal();
                    weekTotal.Content = gen.getWeekTotal();
                    yearTotal.Content = gen.getYearTotal();
                    storeTotal.Content = gen.getStoreTotal();
                    selectedTotal.Content = gen.getSelectedTotal();
                    allStoresCWeek.Content = gen.getAllStoresCWeek();
                    sTAll.Content = gen.getSTAll();
                    sTWeek.Content = gen.getSTWeek();
                    sTStore.Content = gen.getSTStore();
                    sTStoreWeek.Content = gen.getSTStoreWeek();
                    selectedSupplier.Content = gen.getSelectedSupplier();

                    status.Content = "Done";
                    reportGridGen.ItemsSource = report;
                }));
            }
            else
            {
                MessageBox.Show("No Results Found, be sure all the filters have been selected correctly", "No Results Found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void selectFolder(string path)
        {
            //Select a folder to read from
            status.Content = "Loading...";
            supplierList.Items.RemoveAt(0);
            supplierTypeList.Items.RemoveAt(0);

            folder = new Folder();

            sW.Start(); //start stopwatch

            //Send selected folder path to folder class
            folder.getFiles(path);

            //Start the watchReport to wait for task to complete
            Task watcherTask = new Task(this.GUI_Update);
            watcherTask.Start();
            
        }

        private void selectFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            //go to selectFolder Method
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectFolder(folderDialog.SelectedPath);
            }
        }

        private void GUI_Update()
        {
            //Task will wait until the files have been parsed
            folder.getFilesTask.Wait();

            //The parsed data object will be stored in a new list
            result = new List<Order>(folder.getFilesTask.Result);

            //Hashset are used to remove duplicates
            HashSet<string> suppliers = new HashSet<string>();
            HashSet<string> types = new HashSet<string>();

            for (int i = 0; i < result.Count(); i++)
            {
                if (result[i].Supplier != null)
                {
                    suppliers.Add(result[i].Supplier);
                }
                if (result[i].Type != null)
                {
                    types.Add(result[i].Type);
                }    
            }
            Console.WriteLine("End Hashset");
            //Prints data to user
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
             

                //Populates comboboxes
                foreach(var supplierItem in suppliers)
                {
                    CBItem itemAdd = new CBItem();
                    itemAdd.Content = supplierItem;
                
                    supplierList.Items.Add(itemAdd);
                }

                foreach (var typeItem in types)
                {
                    if (typeItem != null)
                    {
                        CBItem itemAdd = new CBItem();
                        itemAdd.Content = typeItem;
                        supplierTypeList.Items.Add(itemAdd);
                    }
                }

                //End timer and set status to complete
                sW.Stop();
                status.Content = "Done";
                timeTaken.Content = sW.Elapsed;
                sW.Reset();
            }));
           
        }

        private void printDataBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("There are no reports to print", "Print", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void weeks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            weeksSelection = weeks.SelectedIndex;
        }

        private void storesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try{store = (storesList.SelectedItem as CBItem).Value.ToString();}catch(Exception){}
        }

        public void selectStore(string path)
        {
            StreamReader readFile = new StreamReader(path);
            string line;

            storesList.Items.Clear();
                try
                {
                    if (Path.GetFileName(path) == "StoreCodes.csv")
                    {
                        CBItem AllStores = new CBItem();
                        AllStores.Content = "Select a Store";

                        storesList.Items.Add(AllStores);

                        storesList.SelectedItem = AllStores;

                        //reads the stores and populates combobox
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
        private void loadStores_Click(object sender, RoutedEventArgs e)
        {
            //Select the store csv
            OpenFileDialog storeFileDialog = new OpenFileDialog();

            if (storeFileDialog.ShowDialog() == true)
            {
                selectStore(storeFileDialog.FileName);        
            } 
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

        private void loadPath_Click(object sender, RoutedEventArgs e)
        {
            selectFolder(Properties.Settings.Default.path);
        }
    }
}