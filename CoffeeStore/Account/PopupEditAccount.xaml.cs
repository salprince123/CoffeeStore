using CoffeeStore.BUS;
using CoffeeStore.DTO;
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
using static CoffeeStore.Account.PopupAddAccount;

namespace CoffeeStore.Account
{
    /// <summary>
    /// Interaction logic for PopupEditAccount.xaml
    /// </summary>
    public partial class PopupEditAccount : UserControl
    {
        List<EmployeeType> empTypes;

        public PopupEditAccount()
        {
            InitializeComponent();
        }

        public PopupEditAccount(DTO_Employees editEmp)
        {
            InitializeComponent();
            LoadData(editEmp);
        }

        public void LoadData(DTO_Employees editEmp)
        {
            BUS_EmployeeType busEmpType = new BUS_EmployeeType();
            DataTable AccPer = busEmpType.GetEmployeeTypes();
            empTypes = new List<EmployeeType>();
            foreach (DataRow row in AccPer.Rows)
            {
                string id = row["EmployeeTypeID"].ToString();
                string name = row["EmployeeTypeName"].ToString();
                empTypes.Add(new EmployeeType(id, name));
            }
            this.cbEmpType.ItemsSource = empTypes;
            this.cbEmpType.Items.Refresh();

            tboxAccount.Text = editEmp.EmployeeID;
            tboxEmpName.Text = editEmp.EmployeeName;
            tboxPassword.Text = editEmp.Password;
            cbEmpType.Text = editEmp.EmployeeTypeID;
        }    

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (tboxEmpName.Text == "")
            {
                //Employee Name is empty
                return;
            }

            if (tboxPassword.Text == "")
            {
                //Password is empty
                return;
            }

            if (tboxPassword.Text.Length < 4 || tboxPassword.Text.Length > 20)
            {
                //Password < 4 characters or > 20 characters
                return;
            }

            string newEmpTypeID = "";
            foreach (EmployeeType empType in empTypes)
            {
                if (empType.name == cbEmpType.Text)
                {
                    newEmpTypeID = empType.id;
                    break;
                }
            }

            DTO_Employees newEmp = new DTO_Employees(tboxAccount.Text, tboxEmpName.Text, newEmpTypeID, tboxPassword.Text);

            BUS_Employees busAcc = new BUS_Employees();
            if (busAcc.EditEmployee(newEmp))
            {
                MessageBox.Show($"Đã sửa tài khoản {tboxAccount.Text} của nhân viên {tboxEmpName.Text}.");
                Window.GetWindow(this).Close();
            }
            else MessageBox.Show($"Đã xảy ra lỗi trong quá trình sửa tài khoản.");
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
