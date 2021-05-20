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
    /// Interaction logic for Discount.xaml
    /// </summary>
    public partial class Discount : UserControl
    {
        BUS_Discount busDiscount;
        string ID;
        public Discount()
        {
            InitializeComponent();
            busDiscount = new BUS_Discount();
            dgDiscount.ItemsSource = busDiscount.getAllDiscount().DefaultView;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DTO_Discount discount = new DTO_Discount();
            discount.DiscountID = "1";
            discount.DiscountName = tbName.Text;
            discount.DiscountValue = float.Parse(tbPrice.Text);
            discount.StartDate = dpStartDate.SelectedDate.Value.ToString("dd/MM/yyyy");
            discount.EndDate = dpEndDate.SelectedDate.Value.ToString("dd/MM/yyyy");
            if (busDiscount.createNewDiscount(discount) > 0)
            {
                MessageBox.Show("OK");
                dgDiscount.ItemsSource = busDiscount.getAllDiscount().DefaultView;
            }
            else
                MessageBox.Show("Fail"+busDiscount.ID());

            
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dgDiscount.SelectedItem as DataRowView;
            if (row != null)
            {
                String ID = row["DiscountID"].ToString();
                if (busDiscount.deleteDiscount(ID) > 0)
                {
                    MessageBox.Show("Success");
                    dgDiscount.ItemsSource = busDiscount.getAllDiscount().DefaultView;
                }
                else
                    MessageBox.Show("Fail"+busDiscount.ID());
            }
            else
                MessageBox.Show(dgDiscount.SelectedItem.ToString());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DTO_Discount discount = new DTO_Discount();
            discount.DiscountID = ID;
            discount.DiscountName = tbName.Text;
            discount.DiscountValue = float.Parse(tbPrice.Text);
            discount.StartDate = dpStartDate.SelectedDate.Value.ToString("dd/MM/yyyy");
            discount.EndDate = dpEndDate.SelectedDate.Value.ToString("dd/MM/yyyy");
            if (busDiscount.editDiscount(discount) > 0)
            {
                MessageBox.Show("OK");
                dgDiscount.ItemsSource = busDiscount.getAllDiscount().DefaultView;
            }
            else
                MessageBox.Show("Fail");
        }

        private void dgDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("Select");
        }

        private void dgDiscount_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            DataRowView row = dataGrid.SelectedItem as DataRowView;
            if (row != null)
            {
                ID = row["DiscountID"].ToString();
                tbName.Text = row["DiscountName"].ToString();
                tbPrice.Text = row["DiscountValue"].ToString();
                try
                {
                    dpStartDate.SelectedDate = DateTime.ParseExact(row["StartDate"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    dpEndDate.SelectedDate = DateTime.ParseExact(row["EndDate"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(row["StartDate"].ToString());
                }
            }
        }

        private void dgDiscount_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
