using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Objects
{
    public class Statistics
    {
        public double selectedTotal { get; set; }
        public double weekTotal { get; set; }
        public double yearTotal { get; set; }
        public double grandTotal { get; set; }
        public double storeTotal { get; set; }
        public double allStoresWeek { get; set; }
        public double supplyTAll { get; set; }
        public double supplyTWeek { get; set; }
        public double supplyTStore { get; set; }
        public double supplyTStoreWeek { get; set; }
        public double supplierSTotal { get; set; }

        public void clear()
        {
            selectedTotal = 0;
            weekTotal = 0;
            yearTotal = 0;
            grandTotal = 0;
            storeTotal = 0;
            allStoresWeek = 0;
            supplyTAll = 0;
            supplyTWeek = 0;
            supplyTStore = 0;
            supplyTStoreWeek = 0;
            supplierSTotal = 0;
        }
    }
}
