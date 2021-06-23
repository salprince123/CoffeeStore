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
namespace CoffeeStore.Menu
{
    /// <summary>
    /// Interaction logic for MenuList.xaml
    /// </summary>
    public partial class MenuList : UserControl
    {
        BUS_Beverage bus;
        MainWindow _context;
        bool find = false;
        int maxNumpage;
        public MenuList()
        {
            InitializeComponent();
            dgMenu.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            bus = new BUS_Beverage();
            loadData();
        }
        public MenuList(MainWindow mainWindow)
        {
            InitializeComponent();
            dgMenu.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            bus = new BUS_Beverage();
            this._context = mainWindow;
            loadData();
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        void loadData()
        {
            var list = new ObservableCollection<DTO_Beverage>();
            cbBeverageType.ItemsSource = bus.getBeverageType();
            DataTable temp = bus.getAllBeverage();
            int rowNumber = Int32.Parse(tbNumPage.Text);
            int count = 1;
            foreach (DataRow row in temp.Rows)
            {
                string name = row["BeverageName"].ToString();
                string id = row["BeverageID"].ToString();
                string type = row["BeverageTypeName"].ToString();
                int price = Int32.Parse(row["Price"].ToString());
                if (count >= (rowNumber - 1) * 20 + 1 && count <= rowNumber * 20)
                {
                    list.Add(new DTO_Beverage() { BeverageID = id, BeverageName = name, BeverageTypeID = type, Price = price });
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
            dgMenu.ItemsSource = list;
            dgMenu.Items.Refresh();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //CREATE POPUP FROM HERE
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Thêm món",
                Content = new PopupAddMenu(_context),
                Width = 460,
                Height = 380,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 460) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 380) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loadData();

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DTO_Beverage row = (DTO_Beverage)dgMenu.SelectedItem;
            var rowView = dgMenu.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Sửa món",
                Content = new PopupEditMenu(row.BeverageName, row.BeverageTypeID, row.Price.ToString(), row.BeverageID, this._context),
                Width = 460,
                Height = 380,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 540) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 380) / 2,
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loadData();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DTO_Beverage row = (DTO_Beverage)dgMenu.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Xóa món",
                Content = new PopupDeleteConfirm(row, this._context),
                Height = 210,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 380) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 210) / 2,
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loadData();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            findBeverage();
        }
        void findBeverage()
        {
            find = true;
            var list = new ObservableCollection<DTO_Beverage>();
            DataTable temp = null;
            if (cbBeverageType.SelectedItem != null)
            {
                temp = bus.findBeverage(cbBeverageType.SelectedItem.ToString(), tbName.Text);
            }
            else
                temp = bus.findBeverage("", tbName.Text);
            int count = 1;
            int rowNumber = Int32.Parse(tbNumPage.Text);
            foreach (DataRow row in temp.Rows)
            {
                string name = row["BeverageName"].ToString();
                string id = row["BeverageID"].ToString();
                string type = row["BeverageTypeName"].ToString();
                int price = Int32.Parse(row["Price"].ToString());
                if (count >= (rowNumber - 1) * 20 + 1 && count <= rowNumber * 20)
                {
                    list.Add(new DTO_Beverage() { BeverageID = id, BeverageName = name, BeverageTypeID = type, Price = price });
                    count++;
                }
                else count++;                
            }
            dgMenu.ItemsSource = list;
            dgMenu.Items.Refresh();
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
                    findBeverage();
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
                    findBeverage();
                else
                    loadData();
            }

            
        }
    }
}
