using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace CoffeeStore.Report
{
    /// <summary>
    /// Interaction logic for ReportSale.xaml
    /// </summary>
    public partial class ReportSale : UserControl
    {
        public ReportSale()
        {
            InitializeComponent(); 
            DataContext = this;
            LoadChart();
        }
        private void LoadChart()
        {
            SaleChart = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 30, 40, 50, 90, 100, 30, 40, 50, 90, 100  },
                    Fill = Brushes.Orange
                }
            };
            Labels = new[] { "Món 1", "Món 2", "Món 3", "Món 4", "Món 5", "Món 6", "Món 7", "Món 8", "Món 9", "Món 10" };
            Formatter = value => value.ToString("N");
            saleChart.Height = 10 * 100; //Number of labels * 100
        }

        public SeriesCollection SaleChart { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
