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
using System.Data.SQLite;
using CoffeeStore.BUS;
using CoffeeStore.ViewModel;
using MaterialDesignThemes.Wpf;

namespace CoffeeStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ItemMenu inventoryItemMenu;
        ItemMenu inventoryItemMenu1;
        public MainWindow()
        {
            InitializeComponent();
            this.InventoryMenuItem._context = this;
            var menuInventory = new List<SubItem>();
            menuInventory.Add(new SubItem("Tổng quan",0,new Home()));
            menuInventory.Add(new SubItem("Nhập kho",1));
            menuInventory.Add(new SubItem("Xuất kho",2));
            inventoryItemMenu = new ItemMenu("Kho", menuInventory, PackIconKind.Storage);
            InventoryMenuItem.DataContext = inventoryItemMenu;
            
            //example for adding new tab 
            this.InventoryMenuItem1._context = this;
            var menuInventory1 = new List<SubItem>();
            menuInventory1.Add(new SubItem("Tổng", 0, new Home()));
            menuInventory1.Add(new SubItem("Nhập ", 1));
            menuInventory1.Add(new SubItem("Xuấho", 2));
            inventoryItemMenu1 = new ItemMenu("KO", menuInventory1, PackIconKind.Storage);
            InventoryMenuItem1.DataContext = inventoryItemMenu1;
            
        }
        internal void SwitchScreen(object sender)
        {
            var screen = ((UserControl)sender);

            if (screen != null)
            {
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(screen);
            }
        }
    }
}
