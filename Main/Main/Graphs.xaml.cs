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
            if (Properties.Settings.Default.graphPopUp == true)
            {
                loadGraph();
            }
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            loadGraph();
        }
        public void loadGraph()
        {
            List<object> graphData = new List<object>();

            Parallel.For(0, report.Count(), i => {
                graphData.Add(new KeyValuePair<string, int>(report[i].Supplier.ToString(), Convert.ToInt32(report[i].Cost)));
            });

            ((ColumnSeries)mainChart.Series[0]).ItemsSource = graphData;
            mainChart.Visibility = Visibility.Visible;
        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
