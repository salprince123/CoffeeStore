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
        public PopupEditMenu()
        {
            InitializeComponent();
        }
        public PopupEditMenu(string name, string type, string price, string id)
        {
            InitializeComponent();
            bus = new BUS_Beverage();
            tbName.Text = name;
            tbPrice.Text = price;
            cbBeverageType.Text = type;
            ID = id;
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (checkCondition())
            {
                DTO_Beverage beverage = new DTO_Beverage();
                beverage.BeverageID = ID;
                beverage.BeverageName = tbName.Text;
                beverage.BeverageTypeID = bus.getBeverageTypeID(cbBeverageType.Text);
                beverage.Price = Int32.Parse(tbPrice.Text);
                if (bus.editBevverage(beverage) > 0)
                {
                    MessageBox.Show("Thành công");
                }
                else
                    MessageBox.Show("Thất bại");
            }
            else
                MessageBox.Show("Không được để trống tên, giá và loại đồ uống");
        }
        private bool checkCondition()
        {
            return (tbName.Text != "" && tbPrice.Text != "" && cbBeverageType.Text != "");
        }

        private void tbPrice_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberCheck.IsNumber(e.Text);
        }
    }
}
