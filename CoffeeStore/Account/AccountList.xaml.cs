using CoffeeStore.BUS;
using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeStore.Account
{
    /// <summary>
    /// Interaction logic for AccountList.xaml
    /// </summary>
    public partial class AccountList : UserControl
    {
        MainWindow _context;
        class AccountInfo
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string pass { get; set; }

            public AccountInfo() { }
            public AccountInfo(string newid, string newname, string newtype, string newpass)
            {
                id = newid;
                name = newname;
                type = newtype;
                pass = newpass;
            }
        }

        public AccountList()
        {
            InitializeComponent();
            dataGridAccount.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            LoadData();
        }

        public AccountList(MainWindow mainWindow)
        {
            InitializeComponent();
            dataGridAccount.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            this._context = mainWindow;
            LoadData();
        }

        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        public void LoadData()
        {
            List<AccountInfo> employees = new List<AccountInfo>();
            BUS_Employees bus_employees = new BUS_Employees();
            DataTable temp = bus_employees.GetActiveEmployees();
            
            foreach (DataRow row in temp.Rows)
            {
                string id = row["EmployeeID"].ToString();
                string name = row["EmployeeName"].ToString();
                string type = row["EmployeeTypeName"].ToString();
                string pass = row["Password"].ToString();
                employees.Add(item: new AccountInfo(id, name, type, pass));
            }
            this.dataGridAccount.ItemsSource = employees;
            this.dataGridAccount.Items.Refresh();
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
                Title = "Thêm tài khoản",
                Content = new PopupAddAccount(),
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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            AccountInfo row = (AccountInfo)dataGridAccount.SelectedItem;
            if (row == null) return;

            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Xóa tài khoản",
                Content = new PopupDeleteConfirm($"Bạn có chắc chắn muốn xóa tài khoản {row.id} của nhân viên {row.name} không?", row.id),
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
            AccountInfo row = (AccountInfo)dataGridAccount.SelectedItem;
            if (row == null) return;
            DTO_Employees editEmp = new DTO_Employees(row.id, row.name, row.type, row.pass);

            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Thêm tài khoản",
                Content = new PopupEditAccount(editEmp),
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
