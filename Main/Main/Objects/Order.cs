using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class Order
    {
        public String Supplier { get; set; }
        public String Type { get; set; }
        public String Store { get; set; }

        public Double Cost { get; set; }

        public int Week { get; set; }
        public int year { get; set; }
    }
}
