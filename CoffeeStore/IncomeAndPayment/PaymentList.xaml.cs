using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CoffeeStore.BUS;
using CoffeeStore.DTO;
using System.Data;

namespace CoffeeStore.IncomeAndPayment
{
    /// <summary>
    /// Interaction logic for PaymentList.xaml
    /// </summary>
    public partial class PaymentList : UserControl
    {
        MainWindow _context;
        BUS_Payment bus;
        bool find = false;
        int maxNumpage;
        public PaymentList()
        {
            InitializeComponent();
            dgPayment.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            bus = new BUS_Payment();
            loaddata();
        }
        public PaymentList(MainWindow window)
        {
            InitializeComponent();
            dgPayment.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            bus = new BUS_Payment();
            _context = window;
            loaddata();
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        void loaddata()
        {
            var list = new ObservableCollection<DTO_Payment>();
            DataTable temp = bus.getAllPayment();
            int rowNumber = Int32.Parse(tbNumPage.Text);
            int count = 1;
            foreach (DataRow row in temp.Rows)
            {
                string id = row["PaymentID"].ToString();
                float money = float.Parse(row["TotalAmount"].ToString());
                string employeename = row["EmployeeName"].ToString();
                string time = row["Time"].ToString();
                if (count >= (rowNumber - 1) * 20 + 1 && count <= rowNumber * 20)
                {
                    list.Add(new DTO_Payment() { PaymentID = id, Time = time, EmployeeID = employeename, TotalAmount = money });
                    count++;
                }
                else count++;
            }
            if (temp.Rows.Count%20==0)
                {
                    maxNumpage = temp.Rows.Count / 20;
                }
            else
                maxNumpage = temp.Rows.Count / 20+1;
            dgPayment.ItemsSource = list;
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Lập phiếu chi",
                Content = new PopupPaymentAdd(_context),
                Width = 460,
                Height = 420,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 460) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 400) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loaddata();
        }

        private void btnWatch_Click(object sender, RoutedEventArgs e)
        {
            DTO_Payment dto = (DTO_Payment)dgPayment.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Chi tiết phiếu chi",
                Content = new PopupPaymentDetail(dto),
                Width = 450,
                Height = 400,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 450) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 400) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DTO_Payment dto = (DTO_Payment)dgPayment.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Sửa phiếu chi",
                Content = new PopupPaymentEdit(dto),
                Width = 450,
                Height = 400,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 450) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 400) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loaddata();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DTO_Payment dto = (DTO_Payment)dgPayment.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Xóa phiếu chi",
                Content = new PopupDeleteConfirm("Dữ liệu về " + dto.PaymentID + " sẽ bị xóa vĩnh viễn.\n Bạn chắc chắn muốn xóa?", dto.PaymentID,2),
                Width = 450,
                Height = 400,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 450) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 400) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loaddata();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            findPayment();
        }
        void findPayment()
        {
            DateTime timestart = new DateTime(01 / 01 / 2021);
            if (dpDateStart.SelectedDate != null)
            {
                timestart = (DateTime)dpDateStart.SelectedDate;
            }
            DateTime timeend = DateTime.Now;
            if (dpDateEnd.SelectedDate != null)
            {
                timeend = (DateTime)dpDateEnd.SelectedDate;
            }
            var list = new ObservableCollection<DTO_Payment>();
            DataTable temp = bus.findPaymentbyID(tbNameFind.Text);
            int rowNumber = Int32.Parse(tbNumPage.Text);
            int count = 1;
            foreach (DataRow row in temp.Rows)
            {
                string id = row["PaymentID"].ToString();
                float money = float.Parse(row["TotalAmount"].ToString());
                string employeename = row["EmployeeName"].ToString();
                string time = row["Time"].ToString();
                DateTime timefind = DateTime.ParseExact(time, "dd/MM/yyyy", null);
                if (count >= (rowNumber - 1) * 20 + 1 && count <= rowNumber * 20)
                {
                    if (DateTime.Compare(timefind, timeend) <= 0 && DateTime.Compare(timefind, timestart) >= 0)
                    {
                        list.Add(new DTO_Payment() { PaymentID = id, Time = time, EmployeeID = employeename, TotalAmount = money });
                        count++;
                    }
                }
                else count++;
            }
            dgPayment.ItemsSource = list;
            find = true;
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(tbNumPage.Text) == 1)
            {

            }
            else
            {
                tbNumPage.Text = (Int32.Parse(tbNumPage.Text) - 1).ToString();
                if (!find)
                    loaddata();
                else
                    findPayment();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(tbNumPage.Text)==maxNumpage)
            {

            }
            else
            {
                tbNumPage.Text = (Int32.Parse(tbNumPage.Text) + 1).ToString();
                if (!find)
                    loaddata();
                else
                    findPayment();
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
