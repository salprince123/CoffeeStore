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
namespace CoffeeStore.Discount
{
    /// <summary>
    /// Interaction logic for DiscountList.xaml
    /// </summary>
    public partial class DiscountList : UserControl
    {
        BUS_Discount bus = new BUS_Discount();
        MainWindow _context;
        public DiscountList()
        {
            InitializeComponent();
            loadData();
        }
        public DiscountList(MainWindow window)
        {
            InitializeComponent();
            this._context = window;
            loadData();
        }
        void loadData()
        {
            var list = new ObservableCollection<DTO_Discount>();
            DataTable temp = bus.getAllDiscount();
            foreach (DataRow row in temp.Rows)
            {
                string name = row["DiscountName"].ToString();
                string id = row["DiscountID"].ToString();
                int value = Int32.Parse(row["Price"].ToString());
                string startdate = row["startdate"].ToString();
                string enddate = row["enddate"].ToString();
                list.Add(new DTO_Discount() { DiscountID = id, DiscountName = name, DiscountValue = value, StartDate = startdate, EndDate = enddate });
            }
            dgDiscount.ItemsSource = list;
            dgDiscount.Items.Refresh();
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode= ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Thêm ưu đãi",
                Content = new PopupDiscountAdd(_context),
                Width = 540,
                Height = 500,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 1000 / 2) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 800 / 2) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void btnWatch_Click(object sender, RoutedEventArgs e)
        {
            DTO_Discount row = (DTO_Discount)dgDiscount.SelectedItem;
            MessageBox.Show(row.DiscountID);
            var rowView = dgDiscount.SelectedItem;
            var screen = new PopupDiscountEdit(row.DiscountID, row.DiscountName, row.StartDate, row.EndDate, row.DiscountValue.ToString());
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DTO_Discount row = (DTO_Discount)dgDiscount.SelectedItem;
            MessageBox.Show(row.DiscountID);
            var rowView = dgDiscount.SelectedItem;
            var screen = new PopupDiscountEdit(row.DiscountID, row.DiscountName, row.StartDate, row.EndDate, row.DiscountValue.ToString());
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            dgDiscount.ItemsSource = bus.findDiscount(tbDateStart.Text, tbDateEnd.Text).DefaultView;
        }
    }
}
