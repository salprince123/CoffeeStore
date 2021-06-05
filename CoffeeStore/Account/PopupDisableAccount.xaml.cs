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

namespace CoffeeStore.Account
{
    /// <summary>
    /// Interaction logic for PopupDisableAccount.xaml
    /// </summary>
    public partial class PopupDisableAccount : UserControl
    {
        string deleteEmpId;
        public PopupDisableAccount()
        {
            InitializeComponent();
        }

        public PopupDisableAccount(string content, string empID)
        {
            InitializeComponent();
            this.tblockContent.Text = content;
            deleteEmpId = empID;
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            BUS_Employees busEmp = new BUS_Employees();
            int result = busEmp.DeleteEmployee(deleteEmpId);
            if (result == 0)
            {
                MessageBox.Show($"Đã xảy ra lỗi trong quá trình xóa tài khoản.");
            }
            else
            {
                MessageBox.Show($"Đã xóa tài khoản {deleteEmpId}.");
                Window.GetWindow(this).Close();
            }

        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
