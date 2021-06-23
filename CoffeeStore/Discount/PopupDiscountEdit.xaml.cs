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
namespace CoffeeStore.Discount
{
    /// <summary>
    /// Interaction logic for PopupDiscountEdit.xaml
    /// </summary>
    public partial class PopupDiscountEdit : UserControl
    {
        string ID;
        BUS_Discount busDiscount;
        MainWindow mainWindow;
        public PopupDiscountEdit()
        {
            InitializeComponent();
            busDiscount = new BUS_Discount();
        }
        public PopupDiscountEdit(string id, string name, string startdate, string enddate, string value, MainWindow window)
        {
            InitializeComponent();
            busDiscount = new BUS_Discount();
            ID = id;
            tbName.Text = name;
            tbStartDate.SelectedDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            tbStartDate.IsEnabled = false;
            tbEndDate.SelectedDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            tbPrice.Text = value;
            tbDescription.Text = "";
            this.mainWindow = window;
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            tbNameValidation.Text = tbDateValidation.Text = tbRateValidation.Text = "";
            if (tbName.Text == "")
            {
                tbNameValidation.Text = "Tên ưu đãi không được để trống.";
                return;
            }

            if (tbStartDate.SelectedDate == null)
            {
                tbDateValidation.Text = "Ngày bắt đầu không được để trống.";
                return;
            }
            /*
            if (DateTime.Compare((DateTime)tbStartDate.SelectedDate, DateTime.Now.Date) <= 0)
            {
                tbDateValidation.Text = "Ngày bắt đầu phải sau ngày hiện tại.";
                return;
            }
            */
            if (tbEndDate.SelectedDate == null)
            {
                tbDateValidation.Text = "Ngày kết thúc không được để trống.";
                return;
            }

            if (DateTime.Compare((DateTime)tbStartDate.SelectedDate, (DateTime)tbEndDate.SelectedDate) > 0)
            {
                tbDateValidation.Text = "Ngày bắt đầu phải trước ngày kết thúc.";
                return;
            }

            if (tbPrice.Text == "")
            {
                tbRateValidation.Text = "Mức ưu đãi không được để trống.";
                return;
            }

            DTO_Discount discount = new DTO_Discount();
            discount.DiscountID = ID;
            discount.DiscountName = tbName.Text;
            discount.DiscountValue = float.Parse(tbPrice.Text);
            discount.StartDate = tbStartDate.SelectedDate.Value.ToString("dd/MM/yyyy");
            discount.EndDate = tbEndDate.SelectedDate.Value.ToString("dd/MM/yyyy");
            if (busDiscount.editDiscount(discount) > 0)
            {
                MessageBox.Show($"Đã sửa ưu đãi {tbName.Text}");
                Window.GetWindow(this).Close();
            }
            else
                MessageBox.Show($"Đã có lỗi trong quá trình sửa {tbName.Text}");
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
