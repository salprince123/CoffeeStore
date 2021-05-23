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

namespace CoffeeStore.Discount
{
    /// <summary>
    /// Interaction logic for DiscountList.xaml
    /// </summary>
    public partial class DiscountList : UserControl
    {

        public DiscountList()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode= ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Thêm ưu đãi",
                Content = new PopupDiscountAdd(),
                Width = 540,
                Height = 500,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 1000 / 2) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 800 / 2) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }
    }
}
