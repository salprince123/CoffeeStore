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
    /// Interaction logic for PopupAddMenu.xaml
    /// </summary>
    public partial class PopupAddMenu : UserControl
    {
        BUS_Beverage bus = new BUS_Beverage();
        MainWindow main;
        public PopupAddMenu()
        {
            InitializeComponent();
        }
        public PopupAddMenu(MainWindow window)
        {
            InitializeComponent();
            cbBeverageType.ItemsSource = bus.getBeverageType();
            main = window;
        }
        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (checkCondition())
            {
                DTO_Beverage beverage = new DTO_Beverage();
                beverage.BeverageID = bus.createID();
                beverage.BeverageName = tbName.Text;
                beverage.BeverageTypeID = bus.getBeverageTypeID(cbBeverageType.Text);
                beverage.Price = Int32.Parse(tbPrice.Text);
                if (bus.createNewBevverage(beverage) > 0)
                {
                    MessageBox.Show("Thành công");
                }
                else
                    MessageBox.Show("Thất bại");
                var screen = new MenuList(main);
                if (screen != null)
                {
                    this.main.StackPanelMain.Children.Clear();
                    this.main.StackPanelMain.Children.Add(screen);
                }
            }
            else
                MessageBox.Show("Không được để trống tên, giá và loại đồ uống");
        }
       
        private bool checkCondition()
        {
            return (tbName.Text != "" && tbPrice.Text != "" && cbBeverageType.Text!= "");
        }

        private void tbPrice_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberCheck.IsNumber(e.Text);
        }
        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            var screen = new MenuList(main);
            if (screen != null)
            {
                this.main.StackPanelMain.Children.Clear();
                this.main.StackPanelMain.Children.Add(screen);
            }
        }
    }
}
