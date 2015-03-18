using Main.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// Interaction logic for Graphs.xaml
    /// </summary>
    public partial class Graphs : Window
    {
        public Graphs()
        {
            InitializeComponent();
            
        }
        public List<Report> report = new List<Report>();
        
        public void setData(List<Report> report)
        {
            this.report = report;
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            List<object> graphData = new List<object>();

            for (int i = 0; i < report.Count(); i++)
            {
                graphData.Add(new KeyValuePair<string, int>(report[i].Supplier.ToString(), Convert.ToInt32(report[i].Cost)));
            }

            ((ColumnSeries)mainChart.Series[0]).ItemsSource = graphData;
        }
    }
}
