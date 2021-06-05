using CoffeeStore.BUS;
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
using static CoffeeStore.Account.GroupAccountList;

namespace CoffeeStore.Account
{
    /// <summary>
    /// Interaction logic for PopupDeleteConfirm.xaml
    /// </summary>
    public partial class PopupDeleteConfirm : UserControl
    {
        string deleteEmpTypeId;
        public PopupDeleteConfirm()
        {
            InitializeComponent();
        }

        public PopupDeleteConfirm(string content, string empTypeID)
        {
            InitializeComponent();
            this.tblockContent.Text = content;
            BUS_EmployeeType busEmp = new BUS_EmployeeType();
            deleteEmpTypeId = busEmp.GetIDByName(empTypeID);
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            BUS_AccessPermissionGroup busAccPerGr = new BUS_AccessPermissionGroup();
            busAccPerGr.DeleteByEmpTypeID(deleteEmpTypeId);

            BUS_EmployeeType busEmp = new BUS_EmployeeType();
            int result = busEmp.DeleteEmployeeType(deleteEmpTypeId);
            if (result == 0)
            {
                MessageBox.Show($"Không thể xóa do vẫn còn tài khoản có loại tài khoản này.");
            }
            else
            {
                MessageBox.Show($"Đã xóa loại tài khoản {deleteEmpTypeId}.");
                Window.GetWindow(this).Close();
            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
