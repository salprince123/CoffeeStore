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
namespace CoffeeStore.Menu
{
    /// <summary>
    /// Interaction logic for PopupDeleteConfirm.xaml
    /// </summary>
    public partial class PopupDeleteConfirm : UserControl
    {
        BUS_Beverage bus = new BUS_Beverage();
        private string ID, name;
        MainWindow _context;
        public PopupDeleteConfirm()
        {
            InitializeComponent();
        }
        public PopupDeleteConfirm(DTO_Beverage beverage, MainWindow context)
        {
            InitializeComponent();
            tblContent.Text = "Dữ liệu về " + beverage.BeverageName + " sẽ bị xóa vĩnh viễn.\n Bạn chắc chắn muốn xóa?";
            ID = beverage.BeverageID;
            name = beverage.BeverageName;
            this._context = context;
        }
        public PopupDeleteConfirm(string id)
        {
            InitializeComponent();
            ID = id;
        }
        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (bus.deleteBevverage(ID) > 0)
            {
                MessageBox.Show($"Đã xóa {name}");
            }
            else
                MessageBox.Show($"Đã xảy ra lỗi trong quá trình xóa {name}");
            Window.GetWindow(this).Close();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

    }
}
