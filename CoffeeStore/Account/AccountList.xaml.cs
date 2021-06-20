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
            public bool state { get; set; }
            public AccountInfo() { }
            public AccountInfo(string newid, string newname, string newtype, string newpass, bool newState)
            {
                id = newid;
                name = newname;
                type = newtype;
                pass = newpass;
                state = newState;
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
            e.Row.Height = 40;
        }

        public void LoadData()
        {
            List<AccountInfo> employees = new List<AccountInfo>();
            BUS_Employees bus_employees = new BUS_Employees();
            DataTable temp = bus_employees.GetEmployees();
            
            foreach (DataRow row in temp.Rows)
            {
                string id = row["EmployeeID"].ToString();
                string name = row["EmployeeName"].ToString();
                string type = row["EmployeeTypeName"].ToString();
                string pass = row["Password"].ToString();
                bool state = true;
                if (row["State"].ToString() == "0")
                    state = false;
                employees.Add(item: new AccountInfo(id, name, type, pass, state));
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
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 540) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 430) / 2,
            };
            window.ShowDialog();
            LoadData();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string id = ((Button)sender).Tag.ToString();
            BUS_Employees busEmp = new BUS_Employees();
            string name = busEmp.GetEmpNameByID(id);

            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Xóa tài khoản",
                Content = new PopupDeleteConfirm($"Bạn có chắc chắn muốn xóa tài khoản {id} \n của nhân viên {name} không?", id, 1),
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
            string id = ((Button)sender).Tag.ToString();
            BUS_Employees busEmp = new BUS_Employees();
            DTO_Employees editEmp = busEmp.GetEmpByID(id);

            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Sửa tài khoản",
                Content = new PopupEditAccount(editEmp),
                Width = 540,
                Height = 430,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 540) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 430) / 2,
            };
            window.ShowDialog();
            LoadData();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void cbCheck_Click(object sender, RoutedEventArgs e)
        {
            string id = ((CheckBox)sender).Tag.ToString();
            bool? state = ((CheckBox)sender).IsChecked;
            BUS_Employees busEmp = new BUS_Employees();
            string name = busEmp.GetEmpNameByID(id);

            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window;
            if (state == true)
                window = new Window
                {
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.None,
                    Title = "Kích hoạt tài khoản",
                    Content = new PopupDeleteConfirm($"Bạn có chắc chắn muốn kích hoạt tài khoản {id} \n của nhân viên {name} không?", id, 3),
                    Width = 380,
                    Height = 210,
                    Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 380) / 2,
                    Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 210) / 2,
                };
            else
                window = new Window
                {
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.None,
                    Title = "Vô hiệu hóa tài khoản",
                    Content = new PopupDeleteConfirm($"Bạn có chắc chắn muốn vô hiệu hóa tài khoản \n{id} của nhân viên {name} không?", id, 4),
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
    }
}
