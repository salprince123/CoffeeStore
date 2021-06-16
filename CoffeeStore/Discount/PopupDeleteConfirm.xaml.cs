using CoffeeStore.DTO;
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

namespace CoffeeStore.Discount
{
    /// <summary>
    /// Interaction logic for PopupDeleteConfirm.xaml
    /// </summary>
    public partial class PopupDeleteConfirm : UserControl
    {
        MainWindow _context;
        String ID;
        BUS.BUS_Discount bus = new BUS.BUS_Discount();
        public PopupDeleteConfirm()
        {
            InitializeComponent();
        }

        public PopupDeleteConfirm(DTO_Discount discount, MainWindow context)
        {
            InitializeComponent();
            ID = discount.DiscountID;
            Content = "Dữ liệu về "+discount.DiscountName+" sẽ bị xóa vĩnh viễn.\n Bạn chắc chắn muốn xóa?";
            this._context = context;
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (bus.deleteDiscount(ID) > 0)
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
