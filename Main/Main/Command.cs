using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Main
{
    /// Developed by Giovanni Lenguito
    /// University: Staffordshire University
    /// Student ID: L010516C
    public class Command
    {
        public void Print(DataGrid content){
            PrintDialog printDialog = new PrintDialog();
            if ((printDialog.ShowDialog() == true))
            {
                printDialog.PrintVisual(content as Visual, "Print Report");
            }
        }
    }
}
