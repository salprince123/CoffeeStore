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
    /// Interaction logic for PopupChangePassword.xaml
    /// </summary>
    public partial class PopupChangePassword : UserControl
    {
        string empId;
        public PopupChangePassword(string id)
        {
            InitializeComponent();
            empId = id;
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Close();
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (tboxOldPass.Text == "")
            {
                //Old password is empty
                MessageBox.Show($"Val 1.");
                return;
            }

            if (tboxNewPass.Text.Length < 4 || tboxNewPass.Text.Length > 20)
            {
                // New password < 4 characters or > 20 characters
                MessageBox.Show($"Val 2.");
                return;
            }

            if (tboxNewPass.Text.Length != tboxConfirmPass.Text.Length)
            {
                // New password not equal to Confirm password
                MessageBox.Show($"Val 3.");
                return;
            }
            
            BUS_Employees busAcc = new BUS_Employees();

            if (busAcc.GetPasswordByID(empId) != tboxOldPass.Text)
            {
                // Old password is wrong
                MessageBox.Show($"Wrong pass.");
                return;
            }

            if (busAcc.EditPassword(empId, tboxConfirmPass.Text))
            {
                MessageBox.Show($"Đã sửa mật khẩu của nhân viên {empId}.");
                Window.GetWindow(this).Close();
            }
            else MessageBox.Show($"Đã xảy ra lỗi trong quá trình sửa mật khẩu.");
        }
    }
}
