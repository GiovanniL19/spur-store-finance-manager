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

        public double selectedTotal = 0;
        public double weekTotal     = 0;
        public double yearTotal     = 0;
        public double total         = 0;
        public double storeTotal    = 0;


        public void GenerateTheReport(int weeksSelection, List<Order> Result, string store, string year, string supplier, string type)
        {
            report = new ConcurrentQueue<Report>();

            reportGenTask = Task<ConcurrentQueue<Report>>.Run(() =>
            {
                try
                {
                    for(int i = 0; i < Result.Count(); i++){
                        total += Result[i].Cost;
                        if (Result[i].Week == weeksSelection && store == Convert.ToString(Result[i].Store))
                        {
                            weekTotal += Result[i].Cost;
                        }
                        if (year == Convert.ToString(Result[i].year) && store == Convert.ToString(Result[i].Store))
                        {
                            yearTotal += Result[i].Cost;
                        }
                        if (store == Convert.ToString(Result[i].Store))
                        {
                            storeTotal += Result[i].Cost;
                        }
                        if (supplier == Convert.ToString(Result[i].Supplier))
                        {
                            storeTotal += Result[i].Cost;
                        }
                        if (Result[i].Week == weeksSelection && Result[i].Store == store && year == Convert.ToString(Result[i].year) && Result[i].Supplier == supplier && Result[i].Type == type)
                        {
                            Report reportObj = new Report();

                            reportObj.Supplier = Result[i].Supplier;
                            reportObj.Cost = Result[i].Cost;
                            reportObj.Type = Result[i].Type;
                            reportObj.Week = Result[i].Week;
                            reportObj.Year = Result[i].year;

                            selectedTotal += Result[i].Cost;

                            report.Enqueue(reportObj);
                        }
                        else if (Result[i].Week != weeksSelection && Result[i].Store == store && year == Convert.ToString(Result[i].year) && Result[i].Supplier == supplier && Result[i].Type == type)
                        {
                            Report reportObj = new Report();

                            reportObj.Supplier = Result[i].Supplier;
                            reportObj.Cost = Result[i].Cost;
                            reportObj.Type = Result[i].Type;
                            reportObj.Week = Result[i].Week;
                            reportObj.Year = Result[i].year;

                            selectedTotal += Result[i].Cost;

                            report.Enqueue(reportObj);
                        }
                        else if (Result[i].Week == weeksSelection && Result[i].Store != store && year == Convert.ToString(Result[i].year) && Result[i].Supplier == supplier && Result[i].Type == type)
                        {
                            Report reportObj = new Report();

                            reportObj.Supplier = Result[i].Supplier;
                            reportObj.Cost = Result[i].Cost;
                            reportObj.Type = Result[i].Type;
                            reportObj.Week = Result[i].Week;
                            reportObj.Year = Result[i].year;

                            selectedTotal += Result[i].Cost;

                            report.Enqueue(reportObj);
                        }
                        else if (Result[i].Week == weeksSelection && Result[i].Store == store && year != Convert.ToString(Result[i].year) && Result[i].Supplier == supplier && Result[i].Type == type)
                        {
                            Report reportObj = new Report();

                            reportObj.Supplier = Result[i].Supplier;
                            reportObj.Cost = Result[i].Cost;
                            reportObj.Type = Result[i].Type;
                            reportObj.Week = Result[i].Week;
                            reportObj.Year = Result[i].year;

                            selectedTotal += Result[i].Cost;

                            report.Enqueue(reportObj);
                        }
                        else if (Result[i].Week == weeksSelection && Result[i].Store == store && year == Convert.ToString(Result[i].year) && Result[i].Supplier != supplier && Result[i].Type == type)
                        {
                            Report reportObj = new Report();

                            reportObj.Supplier = Result[i].Supplier;
                            reportObj.Cost = Result[i].Cost;
                            reportObj.Type = Result[i].Type;
                            reportObj.Week = Result[i].Week;
                            reportObj.Year = Result[i].year;

                            selectedTotal += Result[i].Cost;

                            report.Enqueue(reportObj);
                        }
                        else if (Result[i].Week == weeksSelection && Result[i].Store == store && year == Convert.ToString(Result[i].year) && Result[i].Supplier == supplier && Result[i].Type != type)
                        {
                            Report reportObj = new Report();

                            reportObj.Supplier = Result[i].Supplier;
                            reportObj.Cost = Result[i].Cost;
                            reportObj.Type = Result[i].Type;
                            reportObj.Week = Result[i].Week;
                            reportObj.Year = Result[i].year;

                            selectedTotal += Result[i].Cost;

                            report.Enqueue(reportObj);
                        }
                    }

                }
                catch (Exception e) { Console.WriteLine(e); }
                return report;
            });
        }

        public string getSelectedTotal()
        {
            return string.Format("{0:C}", selectedTotal);
        }
        public string getYearTotal()
        {
            return string.Format("{0:C}", yearTotal);
        }
        public string getgTotal()
        {
            return string.Format("{0:C}", total);
        }
        public string getWeekTotal()
        {
            return string.Format("{0:C}", weekTotal);
        }
        public string getStoreTotal()
        {
            return string.Format("{0:C}", storeTotal);
        }
        public void resetTotal()
        {
            this.selectedTotal = 0;
            this.total = 0;
            this.yearTotal = 0;
            this.weekTotal = 0;
            this.storeTotal = 0;
        }
    }
}
