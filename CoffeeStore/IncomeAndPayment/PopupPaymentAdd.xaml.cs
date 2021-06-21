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

namespace CoffeeStore.IncomeAndPayment
{
    /// <summary>
    /// Interaction logic for PopupPaymentAdd.xaml
    /// </summary>
    public partial class PopupPaymentAdd : UserControl
    {
        BUS_Payment bus;
        MainWindow mainWindow;
        public PopupPaymentAdd()
        {
            InitializeComponent();
            bus = new BUS_Payment();
        }

        public PopupPaymentAdd(MainWindow main)
        {
            InitializeComponent();
            bus = new BUS_Payment();
            mainWindow = main;
        }
        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (dp1.SelectedDate != null && tbMoney.Text != "")
            {
                DTO_Payment dto = new DTO_Payment();
                dto.PaymentID = bus.createID();
                dto.EmployeeID = "E001";
                dto.Time = dp1.SelectedDate.Value.ToString("dd/MM/yyyy");
                dto.TotalAmount = float.Parse(tbMoney.Text);
                dto.Description = bus.createID();
                if (bus.createNewPayment(dto) > 0)
                    MessageBox.Show("Thành công.");
                else
                    MessageBox.Show("Thất bại.");
                Window.GetWindow(this).Close();
            }
            else
                MessageBox.Show("Phải nhập đầy đủ ngày nhập và số tiền đã chi");
        }

        private void tbMoney_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberCheck.IsNumber(e.Text);
        }
       
    }
}
