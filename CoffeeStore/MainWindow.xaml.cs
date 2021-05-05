﻿using System;
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
using BeautySolutions.View.ViewModel;
using MaterialDesignThemes.Wpf;

namespace CoffeeStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        public MainWindow()
        {
            InitializeComponent();

            var item0 = new ItemMenu("Trang chủ", new UserControl(), PackIconKind.ViewDashboard);
            
            var item1 = new ItemMenu("Thu ngân", new UserControl(), PackIconKind.Schedule);

            var item2 = new ItemMenu("Menu", new UserControl(), PackIconKind.CalendarTextOutline);

            var item3 = new ItemMenu("Ưu đãi", new UserControl(), PackIconKind.ShoppingBasket);

            var menuInventory = new List<SubItem>();
            menuInventory.Add(new SubItem("Thông tin kho"));
            menuInventory.Add(new SubItem("Nhập kho"));
            menuInventory.Add(new SubItem("Xuất kho"));
            var item4 = new ItemMenu("Kho", menuInventory, PackIconKind.Warehouse);

            var menuRevenue = new List<SubItem>();
            menuRevenue.Add(new SubItem("Danh sách hóa đơn"));
            menuRevenue.Add(new SubItem("Danh sách chi"));
            var item5 = new ItemMenu("Thu chi", menuRevenue, PackIconKind.ScaleBalance);

            var menuReport = new List<SubItem>();
            menuReport.Add(new SubItem("Mặt hàng bán chạy"));
            menuReport.Add(new SubItem("Lợi nhuận"));
            var item6 = new ItemMenu("Báo cáo thống kê", menuReport, PackIconKind.ChartLineVariant);

            var menuAccount = new List<SubItem>();
            menuAccount.Add(new SubItem("Tài khoản"));
            menuAccount.Add(new SubItem("Nhóm tài khoản"));
            var item7 = new ItemMenu("Tài khoản", menuAccount, PackIconKind.Register);

            Menu.Children.Add(new MenuItem(item0));
            Menu.Children.Add(new MenuItem(item1));
            Menu.Children.Add(new MenuItem(item2));
            Menu.Children.Add(new MenuItem(item3));
            Menu.Children.Add(new MenuItem(item4));
            Menu.Children.Add(new MenuItem(item5));
            Menu.Children.Add(new MenuItem(item6));
            Menu.Children.Add(new MenuItem(item7));
        }
    }
}
