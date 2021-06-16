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
            foreach (DataRow row in temp.Rows)
            {
                string name = row["BeverageName"].ToString();
                string id = row["BeverageID"].ToString();
                string type = row["BeverageTypeName"].ToString();
                int price = Int32.Parse(row["Price"].ToString());
                list.Add(new DTO_Beverage() { BeverageID = id, BeverageName = name, BeverageTypeID = type, Price = price });

            }

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
                Width = 540,
                Height = 350,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 1000 / 2) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 800 / 2) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            //CREATE POPUP TO HERE

            //THIS IS NOT CREATE POPUP PLEASE DONT USE THIS
            //var screen = new PopupAddMenu(_context);
            //if (screen != null)
            //{
            //    this._context.StackPanelMain.Children.Clear();
            //    this._context.StackPanelMain.Children.Add(screen);
            //}
        }

        private void dgMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dt = (DataGrid)sender;
            DTO_Beverage row = (DTO_Beverage)dt.SelectedItem;
            MessageBox.Show(row.BeverageID);
            
            var screen = new PopupEditMenu(row.BeverageName, row.BeverageTypeID, row.Price.ToString(), row.BeverageID, this._context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DTO_Beverage row = (DTO_Beverage)dgMenu.SelectedItem;
            //MessageBox.Show(row.BeverageID);
            var rowView = dgMenu.SelectedItem;

            //CREATE POPUP FROM HERE
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Sửa món",
                Content = new PopupEditMenu(row.BeverageName, row.BeverageTypeID, row.Price.ToString(), row.BeverageID, this._context),
                Width = 540,
                Height = 350,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 1000 / 2) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 800 / 2) / 2,
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            //CREATE POPUP TO HERE

            //THIS IS NOT CREATE POPUP PLEASE DONT USE THIS
            //var screen = new PopupEditMenu(row.BeverageName, row.BeverageTypeID, row.Price.ToString(), row.BeverageID, this._context);
            //if (screen != null)
            //{
            //    this._context.StackPanelMain.Children.Clear();
            //    this._context.StackPanelMain.Children.Add(screen);
            //}
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DTO_Beverage row = (DTO_Beverage)dgMenu.SelectedItem;

            //PLEASE USE THIS TO CREATE POPUP
            //System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            //((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            //((MainWindow)App.Current.MainWindow).Effect = objBlur;
            //Window window = new Window
            //{
            //    ResizeMode = ResizeMode.NoResize,
            //    WindowStyle = WindowStyle.None,
            //    Title = "Xóa món",
            //    Content = new PopupDeleteConfirm(), //Delete message
            //    Width = 380,
            //    Height = 210,
            //    Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 380) / 2,
            //    Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 210) / 2,
            //};
            //window.ShowDialog();
            //((MainWindow)App.Current.MainWindow).Opacity = 1;
            //((MainWindow)App.Current.MainWindow).Effect = null;

            //NOT THIS
            var screen = new PopupDeleteConfirm(row.BeverageID, this._context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }

        private void dgMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGrid dt = (DataGrid)sender;
            dynamic selctedItem = dt.SelectedItem;
            if (selctedItem != null)
            {
                string value = selctedItem.ID;
                MessageBox.Show(value);
            }
            /*
                        if (rowView!=null)
                        {
                            MessageBox.Show("Selected"+ rowView.
                        }
                        else
                            MessageBox.Show("Not selected");*/
        }

        private void dgMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //DataGrid dg = (DataGrid)sender;
            //row = dg.SelectedItem as DataRowView;
            //if (row != null)
            //{
            //    MessageBox.Show("OK");
            //}

        }

        private void dgMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            var list = new ObservableCollection<DTO_Beverage>();
            DataTable temp = bus.findBeverage(cbBeverageType.SelectedItem.ToString(), tbName.Text);
            foreach (DataRow row in temp.Rows)
            {
                string name = row["BeverageName"].ToString();
                string id = row["BeverageID"].ToString();
                string type = row["BeverageTypeName"].ToString();
                int price = Int32.Parse(row["Price"].ToString());
                list.Add(new DTO_Beverage() { BeverageID = id, BeverageName = name, BeverageTypeID = type, Price = price });
                Console.WriteLine("Find: " + name);
            }
            dgMenu.ItemsSource = list;
            dgMenu.Items.Refresh();
        }

    }
}
