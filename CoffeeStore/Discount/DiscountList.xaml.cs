using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CoffeeStore.DTO;
using CoffeeStore.View;
namespace CoffeeStore.Discount
{
    /// <summary>
    /// Interaction logic for DiscountList.xaml
    /// </summary>
    public partial class DiscountList : UserControl
    {
        BUS_Discount bus = new BUS_Discount();
        MainWindow _context;
        class Discount: DTO_Discount
            {
                public String Status { get; set; }
            }

        public DiscountList()
        {
            InitializeComponent();
            dgDiscount.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            loadData();
        }
        public DiscountList(MainWindow window)
        {
            InitializeComponent();
            dgDiscount.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            this._context = window;
            loadData();
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        void loadData()
        {
            var list = new ObservableCollection<Discount>();
            DataTable temp = bus.getAllDiscount();
            foreach (DataRow row in temp.Rows)
            {
                string name = row["DiscountName"].ToString();
                string id = row["DiscountID"].ToString();
                int value = Int32.Parse(row["DiscountValue"].ToString());
                string startdate = row["startdate"].ToString();
                string enddate = row["enddate"].ToString();
                string status = "";
                DateTime time = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
                if (DateTime.Compare(time, DateTime.Now.Date) >= 0)
                {
                    status = "Đang diễn ra";
                }
                else
                {
                    status = "Đã hết hạn";
                }

                list.Add(new Discount() { DiscountID = id, DiscountName = name, DiscountValue = value, StartDate = startdate, EndDate = enddate, Status = status });
            }
            dgDiscount.ItemsSource = list;
            //foreach (ItemsControl item in dgDiscount.Items)
            //{
            //   //MessageBox.Show(item.);
            //}    
            //dgDiscount.Items.Refresh();
        }
        void findDiscount(string startdatefind, string enddatefind)
        {
            var list = new ObservableCollection<Discount>();
            DataTable temp = bus.getAllDiscount();
            foreach (DataRow row in temp.Rows)
            {
                string name = row["DiscountName"].ToString();
                string id = row["DiscountID"].ToString();
                int value = Int32.Parse(row["DiscountValue"].ToString());
                string startdate = row["startdate"].ToString();
                string enddate = row["enddate"].ToString();
                string status = "";
                DateTime timeend = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
                DateTime timestart = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
                DateTime timeendfind = DateTime.ParseExact(enddatefind, "dd/MM/yyyy", null);
                DateTime timestartfind = DateTime.ParseExact(startdatefind, "dd/MM/yyyy", null);
                if (DateTime.Compare(timeend, DateTime.Now.Date) >= 0)
                {
                    status = "Đang diễn ra";
                }
                else
                {
                    status = "Đã hết hạn";
                }
                if ((DateTime.Compare(timeend, timeendfind) <= 0) &&(DateTime.Compare(timestart, timestartfind) >= 0))
                {
                    list.Add(new Discount() { DiscountID = id, DiscountName = name, DiscountValue = value, StartDate = startdate, EndDate = enddate, Status = status });
                }
                MessageBox.Show((DateTime.Compare(timeend, timeendfind)).ToString());
                MessageBox.Show((DateTime.Compare(timestart, timestartfind) >= 0).ToString());
            }
            dgDiscount.ItemsSource = list;
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Thêm ưu đãi",
                Content = new PopupDiscountAdd(_context),
                Width = 540,
                Height = 500,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 1000 / 2) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 1000 / 2) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loadData();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            this.findDiscount(tbDateStart.SelectedDate.Value.ToString("dd/MM/yyyy"), tbDateEnd.SelectedDate.Value.ToString("dd/MM/yyyy"));
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DTO_Discount row = (DTO_Discount)dgDiscount.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Chi tiết ưu đãi",
                Content = new PopupDeleteConfirm(row, _context),
                Width = 540,
                Height = 350,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 1000 / 2) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 800 / 2) / 2,
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loadData();
        }

        private void btnWatch_Click(object sender, RoutedEventArgs e)
        {
            DTO_Discount row = (DTO_Discount)dgDiscount.SelectedItem;
            var rowView = dgDiscount.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Chi tiết ưu đãi",
                Content = new PopupDiscountDetail(row.DiscountID),
                Width = 400,
                Height = 480,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 400) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 480) / 2,
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DTO_Discount row = (DTO_Discount)dgDiscount.SelectedItem;
            var rowView = dgDiscount.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Sửa ưu đãi",
                Content = new PopupDiscountEdit(row.DiscountID, row.DiscountName, row.StartDate, row.EndDate, row.DiscountValue.ToString(), _context),
                Width = 540,
                Height = 480,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 540) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 480) / 2,
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loadData();
        }

    }
}
