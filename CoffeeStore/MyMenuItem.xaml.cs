using CoffeeStore.ViewModel;
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

namespace CoffeeStore
{
    /// <summary>
    /// Interaction logic for MyMenuItem.xaml
    /// </summary>
    public partial class MyMenuItem : UserControl
    {
        public static readonly DependencyProperty itemMenuProperty =
       DependencyProperty.Register("ItemMenus", typeof(ItemMenu), typeof(MyMenuItem), new PropertyMetadata(new ItemMenu()));
        public ItemMenu ItemMenus
        {
            get { return (ItemMenu)GetValue(itemMenuProperty); }
            set { SetValue(itemMenuProperty, value); }
        }

        public MainWindow _context;
        public MyMenuItem(ItemMenu itemMenu)
        {
            InitializeComponent();
            this.ItemMenus = itemMenu;
            
            ExpanderMenu.Visibility = itemMenu.SubItems == null ? Visibility.Collapsed : Visibility.Visible;
            ListViewItemMenu.Visibility = itemMenu.SubItems == null ? Visibility.Visible : Visibility.Collapsed;
            this.DataContext = itemMenu;
        }
        public MyMenuItem()
        {
            InitializeComponent();
        }
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(_context != null)
                _context.SwitchScreen(((SubItem)((ListView)sender).SelectedItem).Screen);
        }

    }
}
