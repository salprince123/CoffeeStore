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
        bool find = false;
        int maxNumpage;
        class Discount : DTO_Discount
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
            int rowNumber = Int32.Parse(tbNumPage.Text);
            int count = 1;
            foreach (DataRow row in temp.Rows)
            {
                string name = row["DiscountName"].ToString();
                string id = row["DiscountID"].ToString();
                int value = Int32.Parse(row["DiscountValue"].ToString());
                string startdate = row["startdate"].ToString();
                string enddate = row["enddate"].ToString();
                string status = "";
                DateTime time = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
                if (DateTime.Compare(time, DateTime.Now.Date) >= 0 && DateTime.Compare(DateTime.ParseExact(startdate, "dd/MM/yyyy", null), DateTime.Now.Date) <= 0)
                {
                    status = "Đang diễn ra";
                }
                else if (DateTime.Compare(time, DateTime.Now.Date) < 0)
                {
                    status = "Đã hết hạn";
                }
                else if (DateTime.Compare(DateTime.ParseExact(startdate, "dd/MM/yyyy", null), DateTime.Now.Date) > 0)
                {
                    status = "Sắp diễn ra";
                }
                if (count >= (rowNumber - 1) * 20 + 1 && count <= rowNumber * 20)
                {
                    list.Add(new Discount() { DiscountID = id, DiscountName = name, DiscountValue = value, StartDate = startdate, EndDate = enddate, Status = status });
                    count++;
                }
                else count++;
            }
            if (temp.Rows.Count % 20 == 0)
            {
                maxNumpage = temp.Rows.Count / 20;
            }
            else
                maxNumpage = temp.Rows.Count / 20 + 1;
            dgDiscount.ItemsSource = list;
        }
        void findDiscount(string startdatefind, string enddatefind)
        {
            find = true;
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
                int rowNumber = Int32.Parse(tbNumPage.Text);
                int count = 1;
                DateTime time = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);
                if (DateTime.Compare(time, DateTime.Now.Date) >= 0 && DateTime.Compare(DateTime.ParseExact(startdate, "dd/MM/yyyy", null), DateTime.Now.Date) <= 0)
                {
                    status = "Đang diễn ra";
                }
                else if (DateTime.Compare(time, DateTime.Now.Date) < 0)
                {
                    status = "Đã hết hạn";
                }
                else if (DateTime.Compare(DateTime.ParseExact(startdate, "dd/MM/yyyy", null), DateTime.Now.Date) > 0)
                {
                    status = "Sắp diễn ra";
                }
                if ((DateTime.Compare(timeend, timestartfind) < 0) || (DateTime.Compare(timestart, timeendfind) > 0))
                {

                }
                else
                {

                    if (count >= (rowNumber - 1) * 20 + 1 && count <= rowNumber * 20)
                    {
                        list.Add(new Discount() { DiscountID = id, DiscountName = name, DiscountValue = value, StartDate = startdate, EndDate = enddate, Status = status });
                        count++;
                    }
                    else count++;
                }

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
                Width = 460,
                Height = 505,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
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
                Title = "Xóa ưu đãi",
                Content = new PopupDeleteConfirm(row, _context),
                Width = 380,
                Height = 210,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
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
                Width = 460,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
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
                Width = 460,
                Height = 505,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loadData();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(tbNumPage.Text) == maxNumpage)
            {

            }
            else
            {
                tbNumPage.Text = (Int32.Parse(tbNumPage.Text) + 1).ToString();
                if (find)
                    findDiscount(tbDateStart.SelectedDate.Value.ToString("dd/MM/yyyy"), tbDateEnd.SelectedDate.Value.ToString("dd/MM/yyyy"));
                else
                    loadData();
            }            
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(tbNumPage.Text) == 1)
            {

            }
            else
            {
                tbNumPage.Text = (Int32.Parse(tbNumPage.Text) - 1).ToString();
                if (find)
                    findDiscount(tbDateStart.SelectedDate.Value.ToString("dd/MM/yyyy"), tbDateEnd.SelectedDate.Value.ToString("dd/MM/yyyy"));
                else
                    loadData();
            }
        }

        private void tbNumPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void tbNumPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x));
            if (e.Text.Contains(" "))
                e.Handled = false;
        }
    }
}
