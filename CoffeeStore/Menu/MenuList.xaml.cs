﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CoffeeStore.BUS;
using CoffeeStore.DTO;
namespace CoffeeStore.Menu
{
    /// <summary>
    /// Interaction logic for MenuList.xaml
    /// </summary>
    public partial class MenuList : UserControl
    {
        BUS_Beverage bus;
        MainWindow _context;
        bool find = false;
        int maxNumpage;
        int numRow;
        int currentNumpage;
        public MenuList()
        {
            InitializeComponent();
            dgMenu.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            bus = new BUS_Beverage();
            loadData();
        }
        public MenuList(MainWindow mainWindow)
        {
            InitializeComponent();
            dgMenu.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            bus = new BUS_Beverage();
            currentNumpage = 1;
            this._context = mainWindow;
            loadData();
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        void loadData()
        {
            var list = new ObservableCollection<DTO_Beverage>();
            List<string> listt = new List<string>();
            listt = bus.getBeverageType();
            listt.Add("All");
            cbBeverageType.ItemsSource = listt;
            DataTable temp = bus.getAllBeverage();
            int rowNumber = Int32.Parse(tbNumPage.Text);
            int count = 1;
            foreach (DataRow row in temp.Rows)
            {
                string name = row["BeverageName"].ToString();
                string id = row["BeverageID"].ToString();
                string type = row["BeverageTypeName"].ToString();
                int price = Int32.Parse(row["Price"].ToString());
                if (count >= (rowNumber - 1) * 20 + 1 && count <= rowNumber * 20)
                {
                    list.Add(new DTO_Beverage() { BeverageID = id, BeverageName = name, BeverageTypeID = type, Price = price });
                    count++;
                }
                else count++;
            }
            numRow = temp.Rows.Count;
            dgMenu.ItemsSource = list;
            dgMenu.Items.Refresh();
            setNumPage();
        }
        void setNumPage()
        {

            if (numRow % 20 == 0)
            {
                maxNumpage = numRow / 20;
            }
            else
                maxNumpage = numRow / 20 + 1;

            lblMaxPage.Content = maxNumpage.ToString();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //CREATE POPUP FROM HERE
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Thêm món",
                Content = new PopupAddMenu(_context),
                Width = 460,
                Height = 380,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loadData();

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DTO_Beverage row = (DTO_Beverage)dgMenu.SelectedItem;
            var rowView = dgMenu.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Sửa món",
                Content = new PopupEditMenu(row.BeverageName, row.BeverageTypeID, row.Price.ToString(), row.BeverageID, this._context),
                Width = 460,
                Height = 380,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            loadData();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DTO_Beverage row = (DTO_Beverage)dgMenu.SelectedItem;
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Xóa món",
                Content = new PopupDeleteConfirm(row, this._context),
                Width = 420,
                Height = 210,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null; 
            setNumPage();
            if (maxNumpage < int.Parse(tbNumPage.Text))
                tbNumPage.Text = (int.Parse(tbNumPage.Text) - 1).ToString();
            loadData();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            findBeverage();
            tbNumPage.Text = "1";
        }
        void findBeverage()
        {
            find = true;
            var list = new ObservableCollection<DTO_Beverage>();
            DataTable temp = null;
            if (cbBeverageType.SelectedItem != null && !cbBeverageType.SelectedItem.ToString().Equals("All"))
            {
                temp = bus.findBeverage(cbBeverageType.SelectedItem.ToString(), tbName.Text);
            }
            else
                if ((cbBeverageType.SelectedItem == null || cbBeverageType.SelectedItem.ToString().Equals("All")) && tbName.Text.Length != 0 )
                    temp = bus.findBeverage("", tbName.Text);
                else
                    loadData();
            if (temp!=null)
            {
                int count = 1;
                int rowNumber = Int32.Parse(tbNumPage.Text);
                foreach (DataRow row in temp.Rows)
                {
                    string name = row["BeverageName"].ToString();
                    string id = row["BeverageID"].ToString();
                    string type = row["BeverageTypeName"].ToString();
                    int price = Int32.Parse(row["Price"].ToString());
                    if (count >= (rowNumber - 1) * 20 + 1 && count <= rowNumber * 20)
                    {
                        list.Add(new DTO_Beverage() { BeverageID = id, BeverageName = name, BeverageTypeID = type, Price = price });
                        count++;
                    }
                    else count++;
                }
                numRow = temp.Rows.Count;
                setNumPage();
                dgMenu.ItemsSource = list;
                dgMenu.Items.Refresh();
            }    
            
        }
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            currentNumpage = Int32.Parse(tbNumPage.Text);
            if (Int32.Parse(tbNumPage.Text) == 1)
            {

            }
            else
            {
                tbNumPage.Text = (Int32.Parse(tbNumPage.Text) - 1).ToString();
                currentNumpage--;
                if (!find)
                    loadData();
                else
                    findBeverage();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            currentNumpage = Int32.Parse(tbNumPage.Text);
            if (Int32.Parse(tbNumPage.Text) == maxNumpage)
            {

            }
            else
            {
                tbNumPage.Text = (Int32.Parse(tbNumPage.Text) + 1).ToString();
                currentNumpage++;
                if (!find)
                    loadData();
                else
                    findBeverage();
            }
            

        }

        private void tbNumPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
            if (e.Key == Key.Enter)
            {
                if (tbNumPage.Text.Length != 0 && int.Parse(tbNumPage.Text) <= maxNumpage && int.Parse(tbNumPage.Text) > 0)
                    loadData();
                else
                {
                    tbNumPage.Text = currentNumpage.ToString();
                    loadData();
                }
            }
        }

        private void tbNumPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x));
            if (e.Text.Contains(" "))
                e.Handled = false;
        }

        private void tbName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && tbName.Text.Length==0 && !cbBeverageType.SelectedItem.ToString().Equals("All"))
            {
                findBeverage();
            }
            if (e.Key == Key.Delete && tbName.Text.Length == 0 && cbBeverageType.SelectedItem.ToString().Equals("All"))
                loadData();
        }
    }
}
