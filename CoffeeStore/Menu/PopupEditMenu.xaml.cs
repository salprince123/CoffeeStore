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
    /// Interaction logic for PopupEditMenu.xaml
    /// </summary>
    public partial class PopupEditMenu : UserControl
    {
        BUS_Beverage bus;
        string ID;
        MainWindow window;
        public PopupEditMenu()
        {
            InitializeComponent();
        }
        public PopupEditMenu(string name, string type, string price, string id, MainWindow context)
        {
            InitializeComponent();
            bus = new BUS_Beverage();
            tbName.Text = name;
            tbPrice.Text = price;
            cbBeverageType.ItemsSource = bus.getBeverageType();
            cbBeverageType.SelectedItem = type;
            ID = id;
            window = context;
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            tbNameValidation.Text = tbPriceValidation.Text = "";
            if (tbName.Text == "")
            {
                tbNameValidation.Text = "Tên món không được để trống.";
                return;
            }

            if (tbPrice.Text == "")
            {
                tbPriceValidation.Text = "Giá không được để trống.";
                return;
            }

            DTO_Beverage beverage = new DTO_Beverage();
            beverage.BeverageID = ID;
            beverage.BeverageName = tbName.Text;
            beverage.BeverageTypeID = bus.getBeverageTypeID(cbBeverageType.Text);
            beverage.Price = Int32.Parse(tbPrice.Text);
            if (bus.editBevverage(beverage) > 0)
            {
                MessageBox.Show($"Đã sửa thông tin của {tbName.Text}");
                Window.GetWindow(this).Close();
            }
            else
                MessageBox.Show("Đã có lỗi trong quá trình tạo món");
        }

        private void tbPrice_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberCheck.IsNumber(e.Text);
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
