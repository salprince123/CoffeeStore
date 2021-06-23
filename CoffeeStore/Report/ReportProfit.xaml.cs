using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private int months = 12;
        private int dates = 30;
        public ReportProfit()
        {
            InitializeComponent();
            LoadGeneralChartYear();
            DataContext = this;
        }
        private void LoadGeneralChartYear()
        {
            GeneralChart = new SeriesCollection();
            LabelGeneralCharts = new string[months+1];

            GeneralChart.Add(new ColumnSeries
            {
                Title = "Doanh thu ",
                Values = new ChartValues<double> { 10, 50, 39, 10, 50 }
            });
            GeneralChart.Add(new ColumnSeries
            {
                Title = "Chi phí",
                Values = new ChartValues<double> { 10, 50, 39, 10, 50, 39, 10, 50, 39, 90, 7, 50 }
            });
            GeneralChart.Add(new ColumnSeries
            {
                Title = "Lợi nhuận",
                Values = new ChartValues<double> { 10, 50, 39, 10, 50, 39, 10, 50, 39, 90, 7, 50 }
            });

            for (int i = 0; i < months; i++)
            {
                LabelGeneralCharts[i] = "Tháng " + (i + 1);
            }    
            FormatterGeneralCharts = value => value.ToString("N");
            generalChart.Width = LabelGeneralCharts.Length * 100 + 3 * 200;
            generalChartTitle.Text = "Biểu đồ thống kê tổng quát năm ...";
        }

        private void LoadGeneralChartMonth()
        {
            GeneralChart = new SeriesCollection();
            LabelGeneralCharts = new string[dates+1];

            GeneralChart.Add(new ColumnSeries
            {
                Title = "Doanh thu ",
                Values = new ChartValues<double> { 10, 50, 39, 10, 50, 39, 10, 50, 39, 90, 7, 50 }
            });
            GeneralChart.Add(new ColumnSeries
            {
                Title = "Chi phí",
                Values = new ChartValues<double> { 10, 50, 39, 10, 50, 39, 10, 50, 39, 90, 7, 50 }
            });
            GeneralChart.Add(new ColumnSeries
            {
                Title = "Lợi nhuận",
                Values = new ChartValues<double> { 10, 50, 39, 10, 50, 39, 10, 50, 39, 90, 7, 50 }
            });

            for (int i = 0; i < dates; i++)
            {
                LabelGeneralCharts[i] = "Ngày " + (i + 1);
            }
            FormatterGeneralCharts = value => value.ToString("N");
            generalChart.Width = LabelGeneralCharts.Length * 100 + 3 * 200;
            generalChartTitle.Text = "Biểu đồ thống kê tổng quát .../... ";
        }

        public SeriesCollection GeneralChart { get; set; }
        public string[] LabelGeneralCharts { get; set; }
        public Func<double, string> FormatterGeneralCharts { get; set; }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            if (cbYearMonth.Text == "Theo tháng")
            {
                LoadGeneralChartMonth();
                generalChart.Update();
            } else
            {
                LoadGeneralChartYear();
                generalChart.Update();
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
