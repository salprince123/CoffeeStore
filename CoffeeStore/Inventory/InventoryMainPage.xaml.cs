﻿using CoffeeStore.BUS;
using System;
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

namespace CoffeeStore.Inventory
{
    /// <summary>
    /// Interaction logic for InventoryMainPage.xaml
    /// </summary>
    public partial class InventoryMainPage : UserControl
    {
        public String selectionName = "";
        public class InventoryObject
        {
            public int number { get; set; }
            public String Name { get; set; }
            public String Unit { get; set; }
            public String Amount { get; set; }
            //this variable for button add/del/edit
            public String Action { get; set; }
        }
        public InventoryMainPage()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            Dictionary<String, int> mapNameAmount = new Dictionary<string, int>();
            Dictionary<String, String> mapNameUnit= new Dictionary<string, string>();
            var list = new ObservableCollection<InventoryObject>();
            //mapping name with unit & import amount
                //With import amount
            BUS_InventoryImportDetail import = new BUS_InventoryImportDetail();
            DataTable temp = import.SelectAllImportDetailGroupByName();
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                mapNameAmount[name] = int.Parse(amount);
            }
            //With unit
            BUS_Material mater = new BUS_Material();
            DataTable tempMater = mater.selectAll();
            foreach (DataRow row in tempMater.Rows)
            {
                string name = row["MaterialName"].ToString();
                string unit = row["Unit"].ToString();
                mapNameUnit[name] = unit;
            }
            //calculate amount in stock = import - export (if have)
            BUS_InventoryExportDetail export = new BUS_InventoryExportDetail();
            DataTable temp1 = export.SelectAllExportDetailGroupByName();
            foreach (DataRow row in temp1.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                if(mapNameAmount.ContainsKey(name))
                    mapNameAmount[name] -= int.Parse(amount);
            }
            //finally get the amount of mater in stock (if not import yet then:  amount =0 )
            int number0 = 1;
            foreach (KeyValuePair<string, string> name in mapNameUnit)
            {
                int amount = 0;
                if (mapNameAmount.ContainsKey(name.Key))
                    amount = mapNameAmount[name.Key];
                list.Add(new InventoryObject() { number = number0, Name = name.Key, Amount = amount.ToString(), Unit = name.Value, Action = "" });
                number0++;
            }
            this.dataGridInfo.ItemsSource = list;
        }

        public void findMaterial(String keyword)
        {
            MessageBox.Show("vui long code xu li");
        }
        private void tbKeyword_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //tbKeyword.Clear();
        }


        private void tbKeyword_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //tbKeyword.Clear();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            findMaterial(tbKeyword.Text);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                Title = "Thêm nguyên liệu, vật liệu",
                Content = new PopupAddMaterial(),
                Width=540,
                Height=280,
                Left= (Application.Current.MainWindow.Left+ Application.Current.MainWindow.Width - 900/2)/2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height -600/2) / 2,
            };
            window.ShowDialog();
            LoadData();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectionName != "")
            {
                InventoryObject row = (InventoryObject)dataGridInfo.SelectedItem;
                if(row != null)
                {
                    System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
                    ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
                    ((MainWindow)App.Current.MainWindow).Effect = objBlur;
                    Window window = new Window
                    {
                        Title = "Sửa nguyên liệu, vật liệu",
                        Content = new PopupEditMaterial(row.Name, row.Unit),
                        Width = 540,
                        Height = 300,
                        Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 900 / 2) / 2,
                        Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 600 / 2) / 2,
                    };
                    window.ShowDialog();
                    LoadData();
                    ((MainWindow)App.Current.MainWindow).Opacity = 1;
                    ((MainWindow)App.Current.MainWindow).Effect = null;
                }
                
            }
            else MessageBox.Show("Xin vui lòng chọn phiếu cần xem");
            
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectionName != "")
            {
                MessageBox.Show("vui long code xu li");
            }
            else MessageBox.Show("Xin vui lòng chọn phiếu cần xóa");
            
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InventoryObject dataRowView = (InventoryObject)((Button)e.Source).DataContext;
                MessageBox.Show($"You Clicked : {dataRowView.Name}");
                //This is the code which will show the button click row data. Thank you.
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void dataGridInfo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                InventoryObject row = (InventoryObject)dataGridInfo.SelectedItem;
                this.selectionName = row.Name;
            }
            catch (Exception) { }
            
        }
    }
}
