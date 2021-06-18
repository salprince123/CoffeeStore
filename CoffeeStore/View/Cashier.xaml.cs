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

namespace CoffeeStore.View
{
    /// <summary>
    /// Interaction logic for Cashier.xaml
    /// </summary>
    public partial class Cashier : UserControl
    {
        MainWindow _context;

        public class MenuBeverage
        {
            public string id { get; set; }
            public string name{ get; set;}
            public int cost { get; set; }
            public bool isOutOfStock { get; set; }
            public MenuBeverage(string newId, string newName, int newCost, bool newState)
            {
                id = newId;
                name = newName;
                cost = newCost;
                isOutOfStock = newState;
            }    
        }

        class BillItem
        {
            public string id { get; set; }
            public string name { get; set; }
            public int unitCost { get; set; }
            public int cost { get; set; }
            public int amount { get; set; }
            public BillItem(string newId, string newName, int newCost)
            {
                id = newId;
                name = newName;
                unitCost = newCost;
                cost = newCost;
                amount = 1;
            }
        }

        List<MenuBeverage> menuItems;
        List<BillItem> billItems;
        int total;
        int received;
        public Cashier(MainWindow mainWindow)
        {
            InitializeComponent();
            _context = mainWindow;
            LoadData();
        }

        public void LoadData()
        {
            menuItems = new List<MenuBeverage>();
            billItems = new List<BillItem>();
            BUS_Beverage busBev = new BUS_Beverage();
            DataTable BevsData = busBev.getAllBeverage();
            foreach (DataRow row in BevsData.Rows)
            {
                string id = row["BeverageID"].ToString();
                string name = row["BeverageName"].ToString();
                int price = Int32.Parse(row["Price"].ToString());
                bool isOutOfState;
                if (row["IsOutOfStock"].ToString() == "0")
                    isOutOfState = false;
                else isOutOfState = true;
                menuItems.Add(new MenuBeverage(id, name, price, isOutOfState));
            }

            ListViewMenu.ItemsSource = menuItems;
            ListViewMenu.Items.Refresh();

            dgBill.ItemsSource = billItems;
            dgBill.Items.Refresh();

            total = 0;
            received = 0;
            BUS_Discount busDiscount = new BUS_Discount();
            DTO_Discount curDiscount = busDiscount.GetCurrentDiscount();
            tblockDiscount.Text = curDiscount.DiscountValue.ToString() + " %";
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

        private void btnMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string id = ((Button)sender).Tag.ToString();
            string newName = "";
            int newCost = 0;
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (id == menuItems[i].id)
                {
                    if (menuItems[i].isOutOfStock)
                    {
                        MessageBox.Show("Món này đã hết hàng!");
                        return;
                    }
                    newName = menuItems[i].name;
                    newCost = menuItems[i].cost;
                    break;
                }
            }

            for (int i = 0; i < billItems.Count; i++)
            {
                if (id == billItems[i].id)
                {
                    billItems[i].amount++;
                    billItems[i].cost += billItems[i].unitCost;
                    dgBill.Items.Refresh();
                    total += billItems[i].unitCost;
                    tblockTotal.Text = MoneyToString(total);
                    tblockChange.Text = MoneyToString(received - total);
                    return;
                }
            }

            total += newCost;
            tblockTotal.Text = MoneyToString(total);
            tblockChange.Text = MoneyToString(received - total);

            billItems.Add(new BillItem(id, newName, newCost));
            dgBill.Items.Refresh();
        }

        private string MoneyToString(int amount)
        {
            string result = Math.Abs(amount).ToString();
            int start = result.Length % 3;
            if (start == 0)
                start = 3;

            for(int i = start; i < result.Length - 1; i = i + 4)
            {
                result = result.Insert(i, ".");
            }
            if (amount < 0)
                result = "-" + result;
            return result + " VNĐ";
        }

        private void tboxAmountReceived_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                received = Int32.Parse(tboxAmountReceived.Text);
                tblockChange.Text = MoneyToString(received - total);
            }
            catch
            {
                // validate for user add text in Amount box
            }
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
