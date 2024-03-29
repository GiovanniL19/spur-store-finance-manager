﻿using Main.Objects;
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
                if (Directory.Exists(Properties.Settings.Default.storesPath) || File.Exists(Properties.Settings.Default.storesPath)) {
                    selectStore(Properties.Settings.Default.storesPath);
                }
                else
                {
                    MessageBox.Show("Store File is no longer available, it may of be moved or deleted " + "\n" + "Please go to settings and select a store file or click 'Load Stores'" + "\n" + "APPLICATION WILL NOW RESTART", "File Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Properties.Settings.Default.storesPath = "";
                    Properties.Settings.Default.Save();
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown(); 
                }
            }

            Loaded += onLoad_Loaded;
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void onLoad_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.welcome == true)
            {
                Welcome welcomeScreen = new Welcome();
                welcomeScreen.Show();
            }
        }


        public DataGenerator generateData = new DataGenerator();
        public ReportGenerator generateReport = new ReportGenerator();
        
        public List<Order> result;
        public List<Report> report;

        public Stopwatch sW = new Stopwatch();
        public Stopwatch sWGen = new Stopwatch();

        //Filters
        public int weeksSelection = 0;
        public string year;
        public string store;
        public string supplier;
        public string type;

        public static CancellationTokenSource cancelTask;
        public Statistics stats;
        public Graph graphDisplay = new Graph();


        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            if (result == null)
            {
                MessageBox.Show("Please Select a Folder", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                //Generate Report
                status.Content = "Generating...";
                
                loading.Visibility = Visibility.Visible;
                sWGen.Reset();
                sWGen.Start();
                reportGridGen.ClearValue(ItemsControl.ItemsSourceProperty);
                reportGridGen.Items.Clear();
                reportGridGen.Items.Refresh();

                if (report != null)
                {
                    report = null;
                }

                //Send data to GenerateReport
                generateReport.Execute(weeksSelection, result.ToList(), store, year, supplier, type);

                //Start the watchReport to wait for task to complete
                Task watchReport = new Task(this.Report_Update);
                watchReport.Start();
            }
        }

        private void Report_Update()
        {
            //Task will wait until the report is generate and that tasks returns a value
            generateReport.reportGenTask.Wait();
            //Store the returned data in a list full of Report objects
            report = new List<Report>(generateReport.reportGenTask.Result);

            if (report.Count > 0)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    //Prints report to labels and data grid
                    numberOfReport.Content = report.Count();

                    stats = generateReport.getStats();

                    gTotal.Content = string.Format("{0:C}", stats.grandTotal);
                    weekTotal.Content = string.Format("{0:C}", stats.weekTotal);
                    yearTotal.Content = string.Format("{0:C}", stats.yearTotal);
                    storeTotal.Content = string.Format("{0:C}", stats.storeTotal);
                    selectedTotal.Content = string.Format("{0:C}", stats.selectedTotal);
                    allStoresCWeek.Content = string.Format("{0:C}", stats.allStoresWeek);
                    sTAll.Content = string.Format("{0:C}", stats.supplyTAll);
                    sTWeek.Content = string.Format("{0:C}", stats.supplyTWeek);
                    sTStore.Content = string.Format("{0:C}", stats.supplyTStore);
                    sTStoreWeek.Content = string.Format("{0:C}", stats.supplyTWeek);
                    selectedSupplier.Content = string.Format("{0:C}", stats.supplierSTotal);

                    stats.clear();
                    
                    reportGridGen.ItemsSource = report;
                    status.Content = "Done";
                    graphDisplay = new Graph();
                    graphDisplay.setData(report);
                    if (Properties.Settings.Default.graphPopUp == true)
                    {
                        graphDisplay.Show();
                    }
                    sWGen.Stop();
                    timeTakenGen.Content = sWGen.Elapsed;
                    loading.Visibility = Visibility.Hidden;
                    
                }));
            }
            else
            {
                MessageBox.Show("No Results Found, be sure all the filters have been selected correctly", "No Results Found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void selectFolder(string path)
        {
            if (Directory.Exists(path) || File.Exists(path))
            {
                //Select a folder to read from
                cancelBtn.Visibility = Visibility.Visible;
                loading.Visibility = Visibility.Visible;

                sW.Reset();
                itemsParsedCount.Content = "0";
                timeTaken.Content = sW.Elapsed;

                status.Content = "Loading...";
                sW.Start(); //start stopwatch

                try
                {
                    supplierList.Items.RemoveAt(0);
                    supplierTypeList.Items.RemoveAt(0);
                    yearsList.Items.RemoveAt(0);

                    if (yearsList.Items != null)
                    {
                        yearsList.Items.Clear();
                    }
                    if (supplierList.Items != null)
                    {
                        supplierList.Items.Clear();
                    }
                    if (supplierTypeList.Items != null)
                    {
                        supplierTypeList.Items.Clear();
                    }
                }
                catch (Exception) { }

                cancelTask = new CancellationTokenSource();
                //Send selected folder path to folder class
                generateData.Execute(path);

                //Start the watchReport to wait for task to complete
                var watcherTask = Task.Factory.StartNew(() =>
                {
                    this.GUI_Update();
                }, cancelTask.Token);
            }
            else
            {
                MessageBox.Show("Selected folder is no longer available, it may of be moved or deleted " + "\n" + "Please go to settings and select a folder or click 'Select Folder'" + "\n" + "APPLICATION WILL NOW RESTART", "File Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                Properties.Settings.Default.path = null;
                Properties.Settings.Default.Save();
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown(); 
            }
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
            generateData.getFilesTask.Wait();

            //The parsed data object will be stored in a new list
            result = new List<Order>(generateData.getFilesTask.Result);

            //Hashset are used to remove duplicates
            HashSet<string> suppliers = new HashSet<string>();
            HashSet<string> types = new HashSet<string>();
            HashSet<int> yearsHash = new HashSet<int>();

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
                if (result[i].Year.ToString() != null)
                {
                    yearsHash.Add(result[i].Year);
                }
            }
            Console.WriteLine("End Hashset");
            //Prints data to user
            Dispatcher.Invoke(new Action(() =>
            {
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

                CBItem yearItem = new CBItem();
                yearItem.Content = "Select Year";
                yearsList.Items.Add(yearItem);
                yearsList.SelectedItem = yearItem;


                //Populates comboboxes
                foreach (var supplierItem in suppliers)
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
                foreach (var yearFound in yearsHash)
                {
                    int y = yearFound;   
                    CBItem itemAdd = new CBItem();
                    itemAdd.Content = y.ToString();

                    yearsList.Items.Add(itemAdd);
                }
                //End timer and set status to complete
                sW.Stop();
                if (cancelTask.IsCancellationRequested)
                {
                    status.Content = "Action Canceled";
                }
                else
                {
                    status.Content = "Done";
                }
                timeTaken.Content = sW.Elapsed;
                
                cancelBtn.Visibility = Visibility.Hidden;
                loading.Visibility = Visibility.Hidden;
                
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
            try { store = (storesList.SelectedItem as CBItem).Value.ToString(); }
            catch (Exception) { }
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
            try { year = yearsList.SelectedItem.ToString(); }
            catch (Exception) { }
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

        private void graphBtn_Click(object sender, RoutedEventArgs e)
        {
            if (report != null)
            {
                Graph graphs = new Graph();
                graphs.setData(report);
                graphs.Show();
            }
            else
            {
                MessageBox.Show("You need to generate a report before you can view graphs", "Generate Report", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void cancelTasksBtn_Click(object sender, RoutedEventArgs e)
        {
            if (cancelTask != null)
            {
                generateData.cancel();
                cancelTask.Cancel();
                cancelTask.Dispose();
                MessageBox.Show("Operation canceled, not all data has been generated", "Canceled", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Can not cancel, an error occurred", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}