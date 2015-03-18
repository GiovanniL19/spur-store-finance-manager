using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Threading;

namespace Main
{
    /// Developed by Giovanni Lenguito
    /// University: Staffordshire University
    /// Student ID: L010516C
    public class DataGenerator
    {
        public Task<ConcurrentQueue<Order>> getFilesTask;
        private ConcurrentQueue<Order> DataFile;
        public static CancellationTokenSource cancelExecution;
        
        public void Execute(String SelectedPath)
        {
            DataFile = new ConcurrentQueue<Order>();
            cancelExecution = new CancellationTokenSource();
            getFilesTask = Task<ConcurrentQueue<Order>>.Factory.StartNew(() =>
            {
                string[] files = Directory.GetFiles(SelectedPath);

                if (Properties.Settings.Default.parallel == true)
                {
                    //ParallelOptions po = new ParallelOptions();
                    //po.MaxDegreeOfParallelism = 88; // uses only set core

                    ParallelOptions po = new ParallelOptions();
                    po.CancellationToken = cancelExecution.Token;

                    try
                    {
                        Parallel.ForEach(files, po, file =>
                        {
                            if (Path.GetExtension(file) == ".csv")
                            {
                                try
                                {
                                    List<string[]> csv = FileParser.Execute(file);

                                    file = Path.GetFileName(file);

                                    String[] splitItem = file.Split('.')[0].Split('_');

                                    foreach (var line in csv)
                                    {
                                        Order order = new Order();

                                        order.Store = splitItem[0];
                                        order.Supplier = line[0];
                                        order.Type = line[1];

                                        order.Cost = Convert.ToDouble(line[2]);
                                        order.Week = Convert.ToInt32(splitItem[1]);
                                        order.Year = Convert.ToInt32(splitItem[2]);

                                        DataFile.Enqueue(order);
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                            }
                        });
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("Canceled");
                    }
                }
                else
                {
                    foreach(string file in files)
                    {
                        string fileInUse = file;

                        if (Path.GetExtension(file) == ".csv")
                        {
                            try
                            {
                                List<string[]> csv = FileParser.Execute(file);

                                fileInUse = Path.GetFileName(file);

                                String[] splitItem = fileInUse.Split('.')[0].Split('_');

                                foreach (var line in csv)
                                {
                                    Order order = new Order();

                                    order.Store = splitItem[0];
                                    order.Supplier = line[0];
                                    order.Type = line[1];

                                    order.Cost = Convert.ToDouble(line[2]);
                                    order.Week = Convert.ToInt32(splitItem[1]);
                                    order.Year = Convert.ToInt32(splitItem[2]);

                                    DataFile.Enqueue(order);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }
                    }
                }

                return DataFile;
            }, cancelExecution.Token);
        }

        public void cancel()
        {
            cancelExecution.Cancel();
            cancelExecution.Dispose();
        }
    }
}
