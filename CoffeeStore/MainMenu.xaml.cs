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
        public MainMenu()
        {
            InitializeComponent();
            bus = new BUS_Beverage();
            this.DataContext = this;
            dgMenu.ItemsSource = bus.getAllBeverage().DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            DataRowView row = dg.SelectedItem as DataRowView;
            if (row != null)
            {
                tbName.Text = row["BeverageName"].ToString();
                tbPrice.Text = row["Price"].ToString();
                tbNote.Text = row["Unit"].ToString();
            }
            else
                MessageBox.Show("Null");
        }

        private void dgMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            DataRowView row = dg.SelectedItem as DataRowView;
            if (row != null)
            {
                tbName.Text = row["BeverageName"].ToString();
                tbPrice.Text = row["Price"].ToString();
                tbNote.Text = row["Unit"].ToString();
            }
            else
                MessageBox.Show("Null");
        }
    }
}
