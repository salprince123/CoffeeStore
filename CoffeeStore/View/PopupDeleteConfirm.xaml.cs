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

namespace CoffeeStore.View
{
    /// <summary>
    /// Interaction logic for PopupDeleteConfirm.xaml
    /// </summary>
    public partial class PopupDeleteConfirm : UserControl
    {
        MainWindow _context;
        String ID;
        public PopupDeleteConfirm()
        {
            InitializeComponent();
        }

        public PopupDeleteConfirm(DTO_Discount discount, MainWindow context)
        {
            InitializeComponent();
            ID = discount.DiscountID;
            this._context = context;
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
