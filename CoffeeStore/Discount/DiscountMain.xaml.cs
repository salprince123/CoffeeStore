using CoffeeStore.BUS;
using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for DiscountMain.xaml
    /// </summary>
    public partial class DiscountMain : UserControl
    {
        BUS_Discount busDiscount;
        string ID;
        public DiscountMain()
        {
            InitializeComponent();
            busDiscount = new BUS_Discount();
            dgDiscount.ItemsSource = busDiscount.getAllDiscount().DefaultView;
            dpStartDate.SelectedDate = DateTime.Today;
            dpEndDate.SelectedDate = DateTime.Today;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (checkCondition())
            {
                DTO_Discount discount = new DTO_Discount();
                discount.DiscountID = busDiscount.ID();
                discount.DiscountName = tbName.Text;
                discount.DiscountValue = float.Parse(tbPrice.Text);
                discount.StartDate = dpStartDate.SelectedDate.Value.ToString("dd/MM/yyyy");
                discount.EndDate = dpEndDate.SelectedDate.Value.ToString("dd/MM/yyyy");
                if (busDiscount.createNewDiscount(discount) > 0)
                {
                    MessageBox.Show("Thành công");
                    dgDiscount.ItemsSource = busDiscount.getAllDiscount().DefaultView;
                }
                else
                    MessageBox.Show("Thất bại" + busDiscount.ID());
            }
            else
                MessageBox.Show("Tên discount, giá trị discount, ngày bắt đầu và ngày kết thúc là bắt buộc");
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            /*DataRowView row = dgDiscount.SelectedItem as DataRowView;
            if (row != null)
            {
                String ID = row["Mã giảm giá"].ToString();
                if (busDiscount.deleteDiscount(ID) > 0)
                {
                    MessageBox.Show("Thành công");
                    dgDiscount.ItemsSource = busDiscount.getAllDiscount().DefaultView;
                }
                else
                    MessageBox.Show("Thất bại" + busDiscount.ID());
            }
            else
                MessageBox.Show(dgDiscount.SelectedItem.ToString());*/
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (checkCondition())
            {
                DTO_Discount discount = new DTO_Discount();
                discount.DiscountID = ID;
                discount.DiscountName = tbName.Text;
                discount.DiscountValue = float.Parse(tbPrice.Text);
                discount.StartDate = dpStartDate.SelectedDate.Value.ToString("dd/MM/yyyy");
                discount.EndDate = dpEndDate.SelectedDate.Value.ToString("dd/MM/yyyy");
                if (busDiscount.editDiscount(discount) > 0)
                {
                    MessageBox.Show("Thành công");
                    dgDiscount.ItemsSource = busDiscount.getAllDiscount().DefaultView;
                }
                else
                    MessageBox.Show("Thất bại");
            }
            else
                MessageBox.Show("Tên discount, giá trị discount, ngày bắt đầu và ngày kết thúc là bắt buộc");
        }

        private void dgDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           /* DataGrid dataGrid = (DataGrid)sender;
            DataRowView row = dataGrid.SelectedItem as DataRowView;
            if (row != null)
            {
                ID = row["Mã giảm giá"].ToString();

                tbName.Text = row["Tên ưu đãi"].ToString();
                tbPrice.Text = row["Mức ưu đãi (%)"].ToString();
                tbName.Text = row["Tên giảm giá"].ToString();
                tbPrice.Text = row["Phần trăm giảm"].ToString();
                try
                {
                    dpStartDate.SelectedDate = DateTime.ParseExact(row["Ngày bắt đầu"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    dpEndDate.SelectedDate = DateTime.ParseExact(row["Ngày kết thúc"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }*/
        }
        private void tbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberCheck.IsNumber(e.Text);
        }
        private bool checkCondition()
        {
            return (tbName.Text != "" && tbPrice.Text != "" && dpStartDate.SelectedDate != null && dpEndDate.SelectedDate != null && dpStartDate.SelectedDate < dpEndDate.SelectedDate);
        }
    }
}
