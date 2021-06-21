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
    /// Interaction logic for MaterialList.xaml
    /// </summary>
    public partial class MaterialList : UserControl
    {
        
        public MaterialList()
        {
            InitializeComponent();
            dataGridMaterial.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            LoadData();
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        public List<InventoryObject> list = new List<InventoryObject>();
        public String selectionName = "";
        public class InventoryObject
        {
            public string id { get; set; }
            public String Name { get; set; }
            public String Unit { get; set; }
            public String Amount { get; set; }
            //this variable for button add/del/edit
            public String IsUsing { get; set; }
        }
        public void LoadData()
        {
            list.Clear();
            Dictionary<String, int> mapNameAmount = new Dictionary<string, int>();
            Dictionary<String, String> mapNameUnit = new Dictionary<string, string>();
            
            //mapping name with unit & import amount
            //With import amount
            BUS_InventoryImportDetail import = new BUS_InventoryImportDetail();
            DataTable temp = import.SelectAllImportDetailGroupByName();
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                string use = row["isUse"].ToString();
                if (use == "1")
                    mapNameAmount[name] = int.Parse(amount);
            }
            //With unit
            BUS_Material mater = new BUS_Material();
            DataTable tempMater = mater.selectAll();
            foreach (DataRow row in tempMater.Rows)
            {
                string name = row["MaterialName"].ToString();
                string unit = row["Unit"].ToString();
                string use = row["isUse"].ToString();
                if (use == "1")
                    mapNameUnit[name] = unit;
            }
            //calculate amount in stock = import - export (if have)
            BUS_InventoryExportDetail export = new BUS_InventoryExportDetail();
            DataTable temp1 = export.SelectAllExportDetailGroupByName();
            foreach (DataRow row in temp1.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                if (mapNameAmount.ContainsKey(name))
                    mapNameAmount[name] -= int.Parse(amount);
            }
            //finally get the amount of mater in stock (if not import yet then:  amount =0 )
            int number0 = 1;
            foreach (KeyValuePair<string, string> name in mapNameUnit)
            {
                int amount = 0;
                if (mapNameAmount.ContainsKey(name.Key))
                    amount = mapNameAmount[name.Key];
                list.Add(new InventoryObject() { Name = name.Key, Amount = amount.ToString(), Unit = name.Value });
                number0++;
            }
            if(list.Count %10 ==0 )
                lblMaxPage.Content = list.Count / 10;
            else lblMaxPage.Content = list.Count / 10 +1;
            List<InventoryObject> displayList = new List<InventoryObject>();
            if(list.Count < 10 )
            {
                displayList = list.GetRange(0, list.Count);
            }
            else displayList = list.GetRange(0, 10);
            this.dataGridMaterial.ItemsSource = displayList;
        }

        public void findMaterial(String keyword)
        {

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
            findMaterial(tbFind.Text);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;

            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Thêm vật liệu",
                Content = new PopupAddMaterial(),
                Width = 540,
                Height = 280,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 900 / 2) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 600 / 2) / 2,
            };
            window.ShowDialog();
            LoadData();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            InventoryObject row = (InventoryObject)dataGridMaterial.SelectedItem;
            if (row != null)
            {
                Window window = new Window
                {
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.None,
                    Title = "Sửa vật liệu",
                    Content = new PopupEditMaterial(row.Name, row.Unit),
                    Width = 540,
                    Height = 280,
                    Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 900 / 2) / 2,
                    Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 600 / 2) / 2,
                };
                window.ShowDialog();
                LoadData();
            }
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void dataGridInfo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                InventoryObject row = (InventoryObject)dataGridMaterial.SelectedItem;
                this.selectionName = row.Name;
            }
            catch (Exception) { }

        }

        public bool Delete(String name)
        {
            BUS_Material material = new BUS_Material();
            bool result = material.Delete(name);
            //if (result)
            //    MessageBox.Show($"Đã xóa thành công vật liệu {row.Name}");
            //else MessageBox.Show($"Xóa không thành công");
            LoadData();
            return result;
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            InventoryObject row = (InventoryObject)dataGridMaterial.SelectedItem;
            if(int.Parse(row.Amount) > 0)
            {
                MessageBox.Show($"Bạn không thể xóa vật liệu vẫn còn trong kho!");
                return;
            }
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Xóa vật liệu ",
                Content = new PopupDeleteConfirm(this, row.Name), //delete message
                Width = 380,
                Height = 210,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 380) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 210) / 2,
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;

           

        }
    }
}
