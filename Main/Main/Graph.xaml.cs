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
    public partial class Graph : Window
    {
        public Graph()
        {
            InitializeComponent();            
        }
        public List<object> graphData;

        public void setData(List<Report> reports)
        {
            graphData = new List<object>();

            var suppliers = reports.GroupBy(r => r.Supplier, (key, g) => new { Supplier = key, Reports = g.ToList()});

            foreach(var s in suppliers)
            {
                graphData.Add(new KeyValuePair<string, double>(s.Supplier + "\n" + string.Format("{0:C}",s.Reports.Sum(o => o.Cost)), s.Reports.Sum(o => o.Cost)));
            }
           
            loadGraph();
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            loadGraph();
        }

        public void loadGraph()
        {
            ((ColumnSeries)mainChart.Series[0]).ItemsSource = graphData;
            mainChart.Visibility = Visibility.Visible;
        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
