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

namespace CoffeeStore.View
{
    /// <summary>
    /// Interaction logic for Cashier.xaml
    /// </summary>
    public partial class Cashier : UserControl
    {
        MainWindow _context;
        public Cashier(MainWindow mainWindow)
        {
            InitializeComponent();
            _context = mainWindow;
        }

        private void MenuStatus_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Content = new PopupEditMenuStatus(),
                Width = 450,
                Height = 450,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width/2 + 450 / 2) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height/2 - 460 / 2) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            _context.SwitchToMenu();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            _context.SwitchBackHome();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Discount_Click(object sender, RoutedEventArgs e)
        {
            _context.SwitchToDiscount();
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            _context.LogOut();
        }
        private void ChangePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            _context.PopupChangePassword();
        }
    }
}
