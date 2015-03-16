﻿using System;
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
        public double allStoresCWeek= 0;
        public double sTAll         = 0;
        public double sTWeek        = 0;
        public double sTStore       = 0;
        public double sTStoreWeek   = 0;
        public double selectS       = 0;

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
                        if (Result[i].Week == weeksSelection)
                        {
                            allStoresCWeek += Result[i].Cost;
                        }
                        if (year == Convert.ToString(Result[i].Year) && store == Convert.ToString(Result[i].Store))
                        {
                            yearTotal += Result[i].Cost;
                        }
                        if (store == Convert.ToString(Result[i].Store))
                        {
                            storeTotal += Result[i].Cost;
                        }
                        if (type == Convert.ToString(Result[i].Type))
                        {
                            sTAll += Result[i].Cost;
                        }
                        if (type == Convert.ToString(Result[i].Type) && weeksSelection == Result[i].Week)
                        {
                            sTWeek += Result[i].Cost;
                        }
                        if (type == Convert.ToString(Result[i].Type) && store == Result[i].Store)
                        {
                            sTStore += Result[i].Cost;
                        }
                        if (type == Convert.ToString(Result[i].Type) && store == Result[i].Store && weeksSelection == Result[i].Week)
                        {
                            sTStoreWeek += Result[i].Cost;
                        }
                        if (supplier == Result[i].Supplier)
                        {
                            selectS += Result[i].Cost;
                        }

                        if (Result[i].Week == weeksSelection && Result[i].Store == store && year == Convert.ToString(Result[i].Year) && Result[i].Supplier == supplier && Result[i].Type == type)
                        {
                            Report reportObj = new Report();

                            reportObj.Supplier = Result[i].Supplier;
                            reportObj.Cost = Result[i].Cost;
                            reportObj.Type = Result[i].Type;
                            reportObj.Week = Result[i].Week;
                            reportObj.Year = Result[i].Year;

                            selectedTotal += Result[i].Cost;

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

                            selectedTotal += Result[i].Cost;

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

                            selectedTotal += Result[i].Cost;

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

                            selectedTotal += Result[i].Cost;

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

                            selectedTotal += Result[i].Cost;

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
        public string getAllStoresCWeek()
        {
            return string.Format("{0:C}", allStoresCWeek);
        }
        public string getSTAll()
        {
            return string.Format("{0:C}", sTAll);
        }
        public string getSTWeek()
        {
            return string.Format("{0:C}", sTWeek);
        }
        public string getSTStore()
        {
            return string.Format("{0:C}", sTStore);
        }
        public string getSTStoreWeek()
        {
            return string.Format("{0:C}", sTStoreWeek);
        }
        public string getSelectedSupplier()
        {
            return string.Format("{0:C}", selectS);
        }
        public void resetTotal()
        {
            selectedTotal  = 0;
            total          = 0;
            yearTotal      = 0;
            weekTotal      = 0;
            storeTotal     = 0;
            allStoresCWeek = 0;
            sTAll          = 0;
            sTWeek         = 0;
            sTStore        = 0;
            sTStoreWeek    = 0;
            selectS        = 0;
        }
    }
}
