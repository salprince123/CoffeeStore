using System;
using System.Collections.Generic;
using System.Data;
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
using CoffeeStore.BUS;
using LiveCharts;
using LiveCharts.Wpf;

namespace CoffeeStore.Report
{
    /// <summary>
    /// Interaction logic for ReportSale.xaml
    /// </summary>
    public partial class ReportSale : UserControl
    {
        DateTime start;
        DateTime end;
        public ReportSale()
        {
            InitializeComponent(); 
            DataContext = this;

            start = new DateTime(2021, 1, 1, 0, 0, 0);
            end = DateTime.Today;
            LoadChart();
        }
        private void LoadChart()
        {
            BUS_Beverage busBev = new BUS_Beverage();
            DataTable bevData = busBev.GetBeverageOrderBySellAmount(start, end);
            int countValue = bevData.Rows.Count;

            Labels = new List<string>();
            ChartValues<int> values = new ChartValues<int>();

            for (int i = 0; i < countValue; i++)
            {
                values.Add(Int32.Parse(bevData.Rows[i]["SellAmount"].ToString()));
                Labels.Add(bevData.Rows[i]["BeverageName"].ToString());
            }    

            SaleChart = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "",
                    Values = values,
                    Fill = Brushes.Orange
                }
            };
            Formatter = value => value.ToString("N");
            saleChart.Height = (Labels.Count + 20) * countValue; //Number of labels * 100
            saleChart.Series = SaleChart;
            saleChart.Update();
            DataContext = this;
        }

        public SeriesCollection SaleChart { get; set; }
        public List<string> Labels { get; set; }
        public Func<int, string> Formatter { get; set; }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            DateTime? datepicker = tbDateStart.SelectedDate;
            if (datepicker.ToString() == "")
                start = new DateTime(2021, 1, 1, 0, 0, 0);
            else
                start = datepicker.Value;

            datepicker = tbDateEnd.SelectedDate;
            if (datepicker.ToString() == "")
                end = DateTime.Today;
            else
                end = datepicker.Value;

            LoadChart();
        }
    }
}
