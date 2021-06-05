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
    public const string cashier = "Thu ngân";
    public const string changeAccount = "Thêm/Sửa tài khoản";
    public const string importInventory = "Nhập kho";
    public const string exportInventory = "Xuất kho";
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
            public bool changeAccount { get; set; }
            public bool importInventory { get; set; }
            public bool exportInventory { get; set; }

            public GroupAccountInfo() { }
            public GroupAccountInfo(string newName, bool newCashPer, bool newChangeAcc, bool newImport, bool newExport)
            {
                name = newName;
                cashier = newCashPer;
                changeAccount = newChangeAcc;
                importInventory = newImport;
                exportInventory = newExport;
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
        }

        public void LoadData()
        {
            List<GroupAccountInfo> groupAccountInfos = new List<GroupAccountInfo>();
            BUS_AccessPermission bus_accper = new BUS_AccessPermission();
            DataTable temp = bus_accper.GetAccessInfo();

            foreach (DataRow row in temp.Rows)
            {
                string name = row["EmployeeType"].ToString();
                bool cashier;
                if (row[columnName: Constants.cashier].ToString() == "0")
                    cashier = false;
                else
                    cashier = true;
                bool changeAccount;
                if (row[columnName: Constants.changeAccount].ToString() == "0")
                    changeAccount = false;
                else
                    changeAccount = true;
                bool importInventory;
                if (row[columnName: Constants.importInventory].ToString() == "0")
                    importInventory = false;
                else
                    importInventory = true;
                bool exportInventory;
                if (row[columnName: Constants.exportInventory].ToString() == "0")
                    exportInventory = false;
                else
                    exportInventory = true;
                groupAccountInfos.Add(item: new GroupAccountInfo(name, cashier, changeAccount, importInventory, exportInventory));
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
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 1000 / 2) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 800 / 2) / 2,
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
                Content = new PopupDeleteConfirm($"Bạn có chắc chắn muốn xóa loại khoản {row.name} không?", row.name),
                Width = 540,
                Height = 430,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 1000 / 2) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 800 / 2) / 2,
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
            GroupAccountInfo editGrAcc = new GroupAccountInfo(row.name, row.cashier, row.changeAccount, row.importInventory, row.exportInventory);

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
                Height = 430,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 1000 / 2) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 800 / 2) / 2,
            };
            window.ShowDialog();
            LoadData();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }
    }
}
