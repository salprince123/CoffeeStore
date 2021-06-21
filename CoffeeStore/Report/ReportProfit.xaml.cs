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
    /// Interaction logic for ReportProfit.xaml
    /// </summary>
    public partial class ReportProfit : UserControl
    {
        public ReportProfit()
        {
            InitializeComponent();
            LoadGeneralChart();
            LoadProfitChart();
            DataContext = this;
        }
        private void LoadGeneralChart()
        {
            GeneralChart = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39 },
                    Fill = Brushes.Orange
                }
            };
            LabelGeneralCharts = new[] { "Chi phí", "Doanh thu", "Lợi nhuận" };
            FormatterGeneralCharts = value => value.ToString("N");
        }

        private void LoadProfitChart()
        {
            ProfitChart = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 10, 50, 39, 10, 50, 39 },
                    Fill = Brushes.Orange
                }
            };
            LabelProfitChart = new[] { "Món 1", "Món 2", "Món 3", "Món 1", "Món 2", "Món 3", "Món 1", "Món 2", "Món 3" };
            FormatterProfitChart = value => value.ToString("N");
            profitChart.Height = (LabelProfitChart.Length + 1) * 100; //Number of labels * 100
        }

        public SeriesCollection GeneralChart { get; set; }
        public SeriesCollection ProfitChart { get; set; }
        public string[] LabelGeneralCharts { get; set; }
        public string[] LabelProfitChart { get; set; }
        public Func<double, string> FormatterGeneralCharts { get; set; }
        public Func<double, string> FormatterProfitChart { get; set; }

    }
}
