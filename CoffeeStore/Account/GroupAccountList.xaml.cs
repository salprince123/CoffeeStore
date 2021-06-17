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

static class Constants
{
    public const string CASHIER = "Thu ngân";
    public const string ACCOUNT = "Quản lý tài khoản";
    public const string ACCOUNTTYPE = "Quản lý loại tài khoản";
    public const string INVENTORY = "Quản lý kho";
    public const string COST = "Quản lý chi phí";
    public const string MENU = "Quản lý menu";
    public const string DISCOUNT = "Quản lý ưu đãi";
    public const string REPORT = "Báo cáo";
}

namespace CoffeeStore.Account
{
    /// <summary>
    /// Interaction logic for GroupAccountList.xaml
    /// </summary>
    public partial class GroupAccountList : UserControl
    {
        public class GroupAccountInfo
        {
            public string name { get; set; }
            public bool cashier { get; set; }
            public bool account { get; set; }
            public bool accountType { get; set; }
            public bool inventory { get; set; }
            public bool cost { get; set; }
            public bool menu { get; set; }
            public bool discount { get; set; }
            public bool report { get; set; }
            public GroupAccountInfo() { }
            public GroupAccountInfo(string newName, bool newCashPer, bool newAcc, bool newAccType, bool newInv, bool newCost, bool newMenu, bool newDiscount, bool newReport)
            {
                name = newName;
                cashier = newCashPer;
                account = newAcc;
                accountType = newAccType;
                inventory = newInv;
                cost = newCost;
                menu = newMenu;
                discount = newDiscount;
                report = newReport;
            }
        }
        public GroupAccountList()
        {
            InitializeComponent();
            dataGridGroupAccount.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            LoadData();
        }

        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }

        public void LoadData()
        {
            List<GroupAccountInfo> groupAccountInfos = new List<GroupAccountInfo>();
            BUS_AccessPermission bus_accper = new BUS_AccessPermission();
            DataTable temp = bus_accper.GetAccessInfo();

            foreach (DataRow row in temp.Rows)
            {
                GroupAccountInfo newGrAccInfo = new GroupAccountInfo();

                newGrAccInfo.name = row["EmployeeType"].ToString();

                if (row[columnName: Constants.CASHIER].ToString() == "0")
                    newGrAccInfo.cashier = false;
                else
                    newGrAccInfo.cashier = true;

                if (row[columnName: Constants.ACCOUNT].ToString() == "0")
                    newGrAccInfo.account = false;
                else
                    newGrAccInfo.account = true;

                if (row[columnName: Constants.ACCOUNTTYPE].ToString() == "0")
                    newGrAccInfo.accountType = false;
                else
                    newGrAccInfo.accountType = true;

                if (row[columnName: Constants.INVENTORY].ToString() == "0")
                    newGrAccInfo.inventory = false;
                else
                    newGrAccInfo.inventory = true;

                if (row[columnName: Constants.COST].ToString() == "0")
                    newGrAccInfo.cost = false;
                else
                    newGrAccInfo.cost = true;

                if (row[columnName: Constants.MENU].ToString() == "0")
                    newGrAccInfo.menu = false;
                else
                    newGrAccInfo.menu = true;

                if (row[columnName: Constants.DISCOUNT].ToString() == "0")
                    newGrAccInfo.discount = false;
                else
                    newGrAccInfo.discount = true;

                if (row[columnName: Constants.REPORT].ToString() == "0")
                    newGrAccInfo.report = false;
                else
                    newGrAccInfo.report = true;

                groupAccountInfos.Add(item: newGrAccInfo);
            }

            this.dataGridGroupAccount.ItemsSource = groupAccountInfos;
            this.dataGridGroupAccount.Items.Refresh();
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
                Title = "Thêm nhóm tài khoản",
                Content = new PopupAddGroupAccount(),
                Width = 540,
                Height = 400,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 540) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 400) / 2,
            };
            window.ShowDialog();
            LoadData();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            GroupAccountInfo row = (GroupAccountInfo)dataGridGroupAccount.SelectedItem;
            if (row == null) return;

            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Xóa tài khoản",
                Content = new PopupDeleteConfirm($"Bạn có chắc chắn muốn xóa \nloại tài khoản {row.name} không?", row.name, 2),
                Width = 380,
                Height = 210,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 380) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 210) / 2,
            };
            window.ShowDialog();
            LoadData();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            GroupAccountInfo row = (GroupAccountInfo)dataGridGroupAccount.SelectedItem;
            if (row == null) return;
            GroupAccountInfo editGrAcc = new GroupAccountInfo(row.name, row.cashier, row.account, row.accountType, row.inventory, row.cost, row.menu, row.discount, row.report);

            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Sửa loại tài khoản",
                Content = new PopupEditGroupAccount(editGrAcc),
                Width = 540,
                Height = 400,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 540) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 400) / 2,
            };
            window.ShowDialog();
            LoadData();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }
    }
}
