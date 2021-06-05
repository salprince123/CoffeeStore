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
//using System.Windows.Controls.DataVisualization;

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
            LoadBarChartData();
        }

        private void LoadBarChartData()
        {
            List<KeyValuePair<string, int>> valueListGeneral = new List<KeyValuePair<string, int>>();
            valueListGeneral.Add(new KeyValuePair<string, int>("Chi phí", 60));
            valueListGeneral.Add(new KeyValuePair<string, int>("Doanh thu", 20));
            valueListGeneral.Add(new KeyValuePair<string, int>("Lợi nhuận", 50));
            barChartGeneral.DataContext = valueListGeneral;

            List<KeyValuePair<string, int>> valueListPayment = new List<KeyValuePair<string, int>>();
            valueListPayment.Add(new KeyValuePair<string, int>("Nguyên liệu 1", 60));
            valueListPayment.Add(new KeyValuePair<string, int>("Nguyên liệu 2", 20));
            valueListPayment.Add(new KeyValuePair<string, int>("Nguyên liệu 3", 50));
            barChartPayment.DataContext = valueListPayment;

            List<KeyValuePair<string, int>> valueListProfit = new List<KeyValuePair<string, int>>();
            valueListProfit.Add(new KeyValuePair<string, int>("Món 1", 60));
            valueListProfit.Add(new KeyValuePair<string, int>("Món 2", 20));
            valueListProfit.Add(new KeyValuePair<string, int>("Món 3", 50));
            barChartProfit.DataContext = valueListProfit;
        }
    }
}
