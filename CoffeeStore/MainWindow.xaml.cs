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

namespace CoffeeStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string currentEmpType;
        string currentEmpID;
        public MainWindow()
        {
            
            InitializeComponent();
            currentEmpID = "";
            var item1 = new ItemMenu("Thu ngân", new Cashier(this, currentEmpID), PackIconKind.Schedule);

            var item2 = new ItemMenu("Menu", "AP006", new Menu.MenuList(this), PackIconKind.CalendarTextOutline);

            var item3 = new ItemMenu("Ưu đãi", "AP007", new Discount.DiscountList(this), PackIconKind.ShoppingBasket);

            var menuInventory = new List<SubItem>();
            menuInventory.Add(new SubItem("Thông tin kho", "AP003", new Inventory.MaterialList()));
            menuInventory.Add(new SubItem("Nhập kho", "AP003", new Inventory.InventoryImport(this)));
            menuInventory.Add(new SubItem("Xuất kho", "AP003", new Inventory.InventoryExport(this)));
            var item4 = new ItemMenu("Kho", menuInventory, PackIconKind.Warehouse);

            var menuRevenue = new List<SubItem>();
            menuRevenue.Add(new SubItem("Danh sách hóa đơn", "AP001", new IncomeAndPayment.ReceiptList()));
            menuRevenue.Add(new SubItem("Danh sách chi", "AP005" , new IncomeAndPayment.PaymentList()));
            var item5 = new ItemMenu("Thu chi", menuRevenue, PackIconKind.ScaleBalance);

            var menuReport = new List<SubItem>();
            menuReport.Add(new SubItem("Mặt hàng bán chạy", "AP008"));
            menuReport.Add(new SubItem("Lợi nhuận", "AP008", new Report.ReportProfit()));
            var item6 = new ItemMenu("Báo cáo thống kê", menuReport, PackIconKind.ChartLineVariant);

            var menuAccount = new List<SubItem>();
            menuAccount.Add(new SubItem("Tài khoản", "AP002", new Account.AccountList(this)));
            menuAccount.Add(new SubItem("Nhóm tài khoản", "AP003", new Account.GroupAccountList()));
            var item7 = new ItemMenu("Tài khoản", menuAccount, PackIconKind.Register);

            Menu.Children.Add(new MenuItem(item1, this));
            Menu.Children.Add(new MenuItem(item2, this));
            Menu.Children.Add(new MenuItem(item3, this));
            Menu.Children.Add(new MenuItem(item4, this));
            Menu.Children.Add(new MenuItem(item5, this));
            Menu.Children.Add(new MenuItem(item6, this));
            Menu.Children.Add(new MenuItem(item7, this));
            loginScreen.btnManager.Click += LoginScreen_BtnManager_Click;
            loginScreen.btnSale.Click += LoginScreen_BtnSale_Click;

            //var screen = new Menu.MenuList();
            //StackPanelMain.Children.Add(screen);
        }

        private void LoginScreen_BtnSale_Click(object sender, RoutedEventArgs e)
        {
            bool checkResult = loginScreen.CheckPassword();
            if (checkResult)
            {
                tblockUsername.Text = loginScreen.txtBoxAccount.Text;
                BUS_Employees busEmp = new BUS_Employees();
                currentEmpID = tblockUsername.Text;
                currentEmpType = busEmp.GetEmpTypeByID(tblockUsername.Text);
                gridLogin.Children.Clear();
                var screen = new Cashier(this, currentEmpID);
                gridLogin.Children.Add(screen);
            }
        }

        private void LoginScreen_BtnManager_Click(object sender, RoutedEventArgs e)
        {
            bool checkResult = loginScreen.CheckPassword();
            if (checkResult)
            {
                tblockUsername.Text = loginScreen.txtBoxAccount.Text;
                BUS_Employees busEmp = new BUS_Employees();
                currentEmpID = tblockUsername.Text;
                ((ItemMenu)((MenuItem)Menu.Children[0]).DataContext)._Cashier.SetCurrrentUser(currentEmpID);
                currentEmpType = busEmp.GetEmpTypeByID(tblockUsername.Text);
                gridLogin.Children.Clear();
            }
        }

        private void ChangePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            PopupChangePassword();
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            LogOut();
            currentEmpID = "";
            currentEmpType = "";
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
        internal void SwitchWindow(object sender, int type)
        {
            var screen = ((Cashier)sender);
            screen.LoadData();
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

        internal void SwitchToDiscount()
        {
            var screen = new Discount.DiscountList();
            gridLogin.Children.Clear();
            StackPanelMain.Children.Clear();
            StackPanelMain.Children.Add(screen);
        }

        internal void SwitchToMenu()
        {
            var screen = new Menu.MenuList();
            gridLogin.Children.Clear();
            StackPanelMain.Children.Clear();
            StackPanelMain.Children.Add(screen);
        }

        internal void LogOut()
        {
            gridLogin.Children.Clear();
            gridLogin.Children.Add(loginScreen);
        }

        internal void PopupChangePassword()
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Đổi mật khẩu",
                Content = new Account.PopupChangePassword(currentEmpID),
                Width = 540,
                Height = 350,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 540) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 360) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public string GetCurrentEmpType()
        {
            return currentEmpType;
        }    
        public string GetCurrentEmpName()
        {
            BUS_Employees bus = new BUS_Employees();
            return bus.GetEmpNameByID(this.currentEmpID);
        }
    }
}
