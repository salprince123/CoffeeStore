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
using CoffeeStore.DTO;
using CoffeeStore.BUS;
using System.Data;

namespace CoffeeStore
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        BUS_Beverage bus;
        string ID;
        public MainMenu()
        {
            InitializeComponent();
            bus = new BUS_Beverage();
            this.DataContext = this;
            dgMenu.ItemsSource = bus.getAllBeverage().DefaultView;
            cbBeverageType.ItemsSource = bus.getBeverageType();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DTO_Beverage beverage = new DTO_Beverage();
            beverage.BeverageID = bus.createID();
            beverage.BeverageName = tbName.Text;
            beverage.BeverageTypeID = bus.getBeverageTypeID(cbBeverageType.SelectedItem.ToString());
            beverage.Price = Int32.Parse(tbPrice.Text);
            if (bus.createNewBevverage(beverage) > 0)
            {
                MessageBox.Show("Success");
                dgMenu.ItemsSource = bus.getAllBeverage().DefaultView;
            }
            else
                MessageBox.Show("Fail");
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (bus.deleteBevverage(ID)>0)
            {
                MessageBox.Show("Success");
                dgMenu.ItemsSource = bus.getAllBeverage().DefaultView;
            }
            else
                MessageBox.Show("Fail");
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DTO_Beverage beverage = new DTO_Beverage();
            beverage.BeverageID = ID;
            beverage.BeverageName = tbName.Text;
            beverage.BeverageTypeID = bus.getBeverageTypeID(cbBeverageType.SelectedItem.ToString());
            beverage.Price = Int32.Parse(tbPrice.Text);
            beverage.Amount = Int32.Parse(tbExistingAmount.Text.ToString());
            if (bus.editBevverage(beverage) > 0)
            {
                MessageBox.Show("Success");
                dgMenu.ItemsSource = bus.getAllBeverage().DefaultView;
            }
            else
                MessageBox.Show("Fail");
        }

        private void dgMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            DataRowView row = dg.SelectedItem as DataRowView;
            if (row != null)
            {
                ID = row["Mã đồ uống"].ToString();
                tbName.Text = row["Tên"].ToString();
                tbPrice.Text = row["Giá"].ToString();
                tbUnit.Text = row["Đơn vị"].ToString();
                tbExistingAmount.Text = row["Số lượng"].ToString();
                cbBeverageType.SelectedItem = row["Loại đồ uống"].ToString();
            }
        }

        private void tbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberCheck.IsNumber(e.Text);
        }

        private void tbExistingAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberCheck.IsNumber(e.Text);
        }
    }
}
