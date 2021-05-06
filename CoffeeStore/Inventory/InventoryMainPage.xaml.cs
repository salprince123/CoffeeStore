using CoffeeStore.BUS;
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
            var list = new ObservableCollection<InventoryObject>();

            BUS_InventoryImportDetail import = new BUS_InventoryImportDetail();
            DataTable temp = import.SelectAllImportDetailGroupByName();
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                mapNameAmount[name] = int.Parse(amount);
            }
            BUS_InventoryExportDetail export = new BUS_InventoryExportDetail();
            DataTable temp1 = export.SelectAllExportDetailGroupByName();
            foreach (DataRow row in temp1.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                if(mapNameAmount.ContainsKey(name))
                    mapNameAmount[name] -= int.Parse(amount);
            }
            this.dataGridInfo.ItemsSource = list;

            int number0 = 1;
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                string unit = row["Đơn vị tính"].ToString();
                list.Add(new InventoryObject() { number = number0, Name = name, Amount = mapNameAmount[name].ToString(), Unit = unit, Action = "" });
                number0++;
            }
        }

        public void findMaterial(String keyword)
        {
            Dictionary<String, int> mapNameAmount = new Dictionary<string, int>();
            var list = new ObservableCollection<InventoryObject>();

            BUS_InventoryImportDetail import = new BUS_InventoryImportDetail();
            DataTable temp = import.Find(keyword);
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                mapNameAmount[name] = int.Parse(amount);
            }
            BUS_InventoryExportDetail export = new BUS_InventoryExportDetail();
            DataTable temp1 = export.Find(keyword);
            foreach (DataRow row in temp1.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                if (mapNameAmount.ContainsKey(name))
                    mapNameAmount[name] -= int.Parse(amount);
            }
            this.dataGridInfo.ItemsSource = list;

            int number0 = 1;
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                string unit = row["Đơn vị tính"].ToString();
                list.Add(new InventoryObject() { number = number0, Name = name, Amount = mapNameAmount[name].ToString(), Unit = unit, Action = "" });
                number0++;
            }
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
    }
}
