using CoffeeStore.BUS;
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

namespace CoffeeStore.IncomeAndPayment
{
    /// <summary>
    /// Interaction logic for ReceiptList.xaml
    /// </summary>
    public partial class ReceiptList : UserControl
    {
        public class ReceiptItem
        {
            public string id { get; set; }
            public DateTime time { get; set; }
            public string createrName { get; set; }
            public int total { get; set; }
            public ReceiptItem(string newID, DateTime newTime, string newCreaterName, int newTotal)
            {
                id = newID;
                time = newTime;
                createrName = newCreaterName;
                total = newTotal;
            }

        }

        List<ReceiptItem> receiptItems;

        public ReceiptList()
        {
            InitializeComponent();
            dgReceipt.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            Loaded += LoadData;
            //LoadData();
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }

        void LoadData(Object sender, RoutedEventArgs e)
        {
            receiptItems = new List<ReceiptItem>();
            BUS_Receipt busReceipt = new BUS_Receipt();
            DataTable receipts = busReceipt.GetReceipts();
            foreach (DataRow row in receipts.Rows)
            {
                string id = row["ReceiptID"].ToString();
                DateTime time = TimeZone.CurrentTimeZone.ToLocalTime((DateTime)row["Time"]);
                string creater = row["EmployeeName"].ToString();
                int total = (int)float.Parse(row["Total"].ToString());
                receiptItems.Add(new ReceiptItem(id, time, creater, total));
            }
            dgReceipt.ItemsSource = receiptItems;
            dgReceipt.Items.Refresh();
        }    

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            DateTime? datepicker = dpDateEnd.SelectedDate;
            MessageBox.Show(datepicker.ToString());
        }

        private void btnWatch_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Chi tiết hóa đơn",
                Content = new ReceiptDetail(),
                Width = 450,
                Height = 800,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 450) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 400) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }
    }
}
