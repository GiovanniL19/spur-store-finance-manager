using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class CBItem
    {
        public string Content { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Content;
        }
    }
}
