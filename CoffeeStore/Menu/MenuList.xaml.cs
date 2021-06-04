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
            bus = new BUS_Beverage();
            loadData();
        }
        public MenuList(MainWindow mainWindow)
        {
            InitializeComponent();
            this._context = mainWindow;
            loadData();
        }
        void loadData()
        {
            var list = new ObservableCollection<DTO_Beverage>();
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
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Thêm món",
                Content = new PopupAddMenu(),
                Width = 540,
                Height = 350,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 1000 / 2) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 800 / 2) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void dgMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid dt = (DataGrid)sender;
            DataRowView row = dt.SelectedItem as DataRowView;
            var screen = new PopupEditMenu(row[1].ToString(), row[2].ToString(), row[3].ToString(), row[0].ToString());
            if (screen!=null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }    
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
