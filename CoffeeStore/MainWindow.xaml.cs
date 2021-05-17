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
using System.Data.SQLite;
using CoffeeStore.BUS;
using BeautySolutions.View.ViewModel;
using MaterialDesignThemes.Wpf;
using CoffeeStore.View;
using CoffeeStore.Discount;

namespace CoffeeStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        public MainWindow()
        {
            this.FontFamily = new FontFamily("Open Sans");
            InitializeComponent();

            var item0 = new ItemMenu("Trang chủ", new UserControl(), PackIconKind.ViewDashboard);
            
            var item1 = new ItemMenu("Thu ngân", new Cashier(this), PackIconKind.Schedule);

            var item2 = new ItemMenu("Menu", new Menu.MenuList(), PackIconKind.CalendarTextOutline);

            var item3 = new ItemMenu("Ưu đãi", new DiscountMain(), PackIconKind.ShoppingBasket);

            var menuInventory = new List<SubItem>();
            menuInventory.Add(new SubItem("Thông tin kho",new Inventory.InventoryMainPage()));
            menuInventory.Add(new SubItem("Nhập kho", new Inventory.InventoryImport()));
            menuInventory.Add(new SubItem("Xuất kho"));
            var item4 = new ItemMenu("Kho", menuInventory, PackIconKind.Warehouse);

            var menuRevenue = new List<SubItem>();
            menuRevenue.Add(new SubItem("Danh sách hóa đơn"));
            menuRevenue.Add(new SubItem("Danh sách chi"));
            var item5 = new ItemMenu("Thu chi", menuRevenue, PackIconKind.ScaleBalance);

            var menuReport = new List<SubItem>();
            menuReport.Add(new SubItem("Mặt hàng bán chạy"));
            menuReport.Add(new SubItem("Lợi nhuận"));
            var item6 = new ItemMenu("Báo cáo thống kê", menuReport, PackIconKind.ChartLineVariant);

            var menuAccount = new List<SubItem>();
            menuAccount.Add(new SubItem("Tài khoản", new Account.AccountList()));
            menuAccount.Add(new SubItem("Nhóm tài khoản", new Account.GroupAccountList()));
            var item7 = new ItemMenu("Tài khoản", menuAccount, PackIconKind.Register);

            Menu.Children.Add(new MenuItem(item0, this));
            Menu.Children.Add(new MenuItem(item1, this));
            Menu.Children.Add(new MenuItem(item2, this));
            Menu.Children.Add(new MenuItem(item3, this));
            Menu.Children.Add(new MenuItem(item4, this));
            Menu.Children.Add(new MenuItem(item5, this));
            Menu.Children.Add(new MenuItem(item6, this));
            Menu.Children.Add(new MenuItem(item7, this));

            loginScreen.btnManager.Click += LoginScreen_BtnManager_Click;
            loginScreen.btnSale.Click += LoginScreen_BtnSale_Click;
        }

        private void LoginScreen_BtnSale_Click(object sender, RoutedEventArgs e)
        {
            bool checkResult = loginScreen.CheckPassword();
            var screen = new Cashier(this);
            if (checkResult)
            {
                gridLogin.Children.Clear();
                gridLogin.Children.Add(screen);
            }   
        }

        private void LoginScreen_BtnManager_Click(object sender, RoutedEventArgs e)
        {
            bool checkResult = loginScreen.CheckPassword();
            if (checkResult)
            {
                gridLogin.Children.Clear();
            }
        }
        internal void SwitchScreen(object sender)   
        {
            var screen = ((UserControl)sender);
            if (screen != null)
            {
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(screen);
            }
        }
        internal void SwitchWindow(object sender)
        {
            var screen = ((UserControl)sender);
            if (screen != null)
            {
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(screen);
            }
        }
        internal void SwitchWindow(object sender,int type)
        {
            var screen = ((Cashier)sender);
            if (screen != null)
            {
                gridLogin.Children.Clear();
                gridLogin.Children.Add(screen);
            } 
        }

        public void SwitchBackHome()
        {
            gridLogin.Children.Clear();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
