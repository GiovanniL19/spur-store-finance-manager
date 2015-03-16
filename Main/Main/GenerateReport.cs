using Main.Objects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class GenerateReport
    {
        public Task<ConcurrentQueue<Report>> reportGenTask;
        public ConcurrentQueue<Report> report;
        public Statistics stats = new Statistics();

        public void GenerateTheReport(int weeksSelection, List<Order> Result, string store, string year, string supplier, string type)
        {
            report = new ConcurrentQueue<Report>();

            reportGenTask = Task<ConcurrentQueue<Report>>.Run(() =>
            {
                try
                {
                    for (int i = 0; i < Result.Count(); i++)
                    {
                        stats.grandTotal += Result[i].Cost;
                        if (Result[i].Week == weeksSelection && store == Convert.ToString(Result[i].Store))
                        {
                            stats.weekTotal += Result[i].Cost;
                        }
                        if (Result[i].Week == weeksSelection)
                        {
                            stats.allStoresWeek += Result[i].Cost;
                        }
                        if (year == Convert.ToString(Result[i].Year) && store == Convert.ToString(Result[i].Store))
                        {
                            stats.yearTotal += Result[i].Cost;
                        }
                        if (store == Convert.ToString(Result[i].Store))
                        {
                            stats.storeTotal += Result[i].Cost;
                        }
                        if (type == Convert.ToString(Result[i].Type))
                        {
                            stats.supplyTAll += Result[i].Cost;
                        }
                        if (type == Convert.ToString(Result[i].Type) && weeksSelection == Result[i].Week)
                        {
                            stats.supplyTWeek += Result[i].Cost;
                        }
                        if (type == Convert.ToString(Result[i].Type) && store == Result[i].Store)
                        {
                            stats.supplyTStore += Result[i].Cost;
                        }
                        if (type == Convert.ToString(Result[i].Type) && store == Result[i].Store && weeksSelection == Result[i].Week)
                        {
                            stats.supplyTStoreWeek += Result[i].Cost;
                        }
                        if (supplier == Result[i].Supplier)
                        {
                            stats.supplierSTotal += Result[i].Cost;
                        }

                        if (Result[i].Week == weeksSelection && Result[i].Store == store && year == Convert.ToString(Result[i].Year) && Result[i].Supplier == supplier && Result[i].Type == type)
                        {
                            Report reportObj = new Report();

                            reportObj.Supplier = Result[i].Supplier;
                            reportObj.Cost = Result[i].Cost;
                            reportObj.Type = Result[i].Type;
                            reportObj.Week = Result[i].Week;
                            reportObj.Year = Result[i].Year;

                            stats.selectedTotal += Result[i].Cost;

                            report.Enqueue(reportObj);
                        }
                        else if (Result[i].Week != weeksSelection && Result[i].Store == store && year == Convert.ToString(Result[i].Year) && Result[i].Supplier == supplier && Result[i].Type == type)
                        {
                            Report reportObj = new Report();

                            reportObj.Supplier = Result[i].Supplier;
                            reportObj.Cost = Result[i].Cost;
                            reportObj.Type = Result[i].Type;
                            reportObj.Week = Result[i].Week;
                            reportObj.Year = Result[i].Year;

                            stats.selectedTotal += Result[i].Cost;

                            report.Enqueue(reportObj);
                        }
                        else if (Result[i].Week == weeksSelection && Result[i].Store != store && year == Convert.ToString(Result[i].Year) && Result[i].Supplier == supplier && Result[i].Type == type)
                        {
                            Report reportObj = new Report();

                            reportObj.Supplier = Result[i].Supplier;
                            reportObj.Cost = Result[i].Cost;
                            reportObj.Type = Result[i].Type;
                            reportObj.Week = Result[i].Week;
                            reportObj.Year = Result[i].Year;

                            stats.selectedTotal += Result[i].Cost;

                            report.Enqueue(reportObj);
                        }
                        else if (Result[i].Week == weeksSelection && Result[i].Store == store && year != Convert.ToString(Result[i].Year) && Result[i].Supplier == supplier && Result[i].Type == type)
                        {
                            Report reportObj = new Report();

                            reportObj.Supplier = Result[i].Supplier;
                            reportObj.Cost = Result[i].Cost;
                            reportObj.Type = Result[i].Type;
                            reportObj.Week = Result[i].Week;
                            reportObj.Year = Result[i].Year;

                            stats.selectedTotal += Result[i].Cost;

                            report.Enqueue(reportObj);
                        }
                        else if (Result[i].Week == weeksSelection && Result[i].Store == store && year == Convert.ToString(Result[i].Year) && Result[i].Supplier != supplier && Result[i].Type == type)
                        {
                            Report reportObj = new Report();

                            reportObj.Supplier = Result[i].Supplier;
                            reportObj.Cost = Result[i].Cost;
                            reportObj.Type = Result[i].Type;
                            reportObj.Week = Result[i].Week;
                            reportObj.Year = Result[i].Year;

                            stats.selectedTotal += Result[i].Cost;

                            report.Enqueue(reportObj);
                        }
                        else if (Result[i].Week == weeksSelection && Result[i].Store == store && year == Convert.ToString(Result[i].Year) && Result[i].Supplier == supplier && Result[i].Type != type)
                        {
                            Report reportObj = new Report();

                            reportObj.Supplier = Result[i].Supplier;
                            reportObj.Cost = Result[i].Cost;
                            reportObj.Type = Result[i].Type;
                            reportObj.Week = Result[i].Week;
                            reportObj.Year = Result[i].Year;

                            stats.selectedTotal += Result[i].Cost;

                            report.Enqueue(reportObj);
                        }
                    }

                }
                catch (Exception e) { Console.WriteLine(e); }

                return report;
            });
        }
        public Statistics getStats()
        {
            return stats;
        }
    }
}
