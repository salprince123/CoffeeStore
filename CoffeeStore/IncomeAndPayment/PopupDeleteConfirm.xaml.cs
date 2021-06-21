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
using CoffeeStore.BUS;
using CoffeeStore.DTO;
namespace CoffeeStore.IncomeAndPayment
{
    /// <summary>
    /// Interaction logic for PopupDeleteConfirm.xaml
    /// </summary>
    public partial class PopupDeleteConfirm : UserControl
    {
        BUS_Payment bus;
        string ID;
        public PopupDeleteConfirm()
        {
            InitializeComponent();
        }
        public PopupDeleteConfirm(string id)
        {
            InitializeComponent();
            bus = new BUS_Payment();
            ID = id;
            tblContent.Text = "Dữ liệu về " + ID + " sẽ bị xóa vĩnh viễn.\n Bạn chắc chắn muốn xóa?";
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
                if (bus.deletePayment(ID) > 0)
                {
                    MessageBox.Show("Thành công");
                }
                else
                    MessageBox.Show("Thất bại");
            Window.GetWindow(this).Close();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
