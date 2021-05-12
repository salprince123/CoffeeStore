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
    /// Interaction logic for InventoryImportEDIT.xaml
    /// </summary>
    public partial class InventoryImportEDIT : UserControl
    {
        public String selectionID = "";
        public String ImportName = "";
        public class BindingObject
        {
            public string id { get; set; }
            public string name { get; set; }
            public string day { get; set; }
            public BindingObject(String id0, string name0, string day0)
            {
                this.id = id0;
                this.name = name0;
                this.day = day0;
            }
        }
        public class InventoryImportDetailObject
        {
            public int number { get; set; }
            public String name { get; set; }
            public String unitPrice { get; set; }
            public String amount { get; set; }
            public String unit { get; set; }
            public String totalCost { get; set; }
        }
        public InventoryImportEDIT()
        {
            InitializeComponent();
            if (selectionID != "")
                LoadData();
        }
        public InventoryImportEDIT(String id, String importname, String importdate)
        {
            this.selectionID = id;
            this.ImportName = importname;
            InitializeComponent();
            if (selectionID != "")
                LoadData();
            this.DataContext = new BindingObject(id, importname, importdate);
        }
        public void LoadData()
        {
            var list = new ObservableCollection<InventoryImportDetailObject>();
            BUS_InventoryImport import = new BUS_InventoryImport();
            DataTable temp = import.selectDetail(selectionID);
            int number0 = 1;
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                string unitprice = row["Đơn giá"].ToString();
                string unit = row["Đơn vị tính"].ToString();
                int tongtien = int.Parse(amount) * int.Parse(unitprice);
                list.Add(new InventoryImportDetailObject() { number = number0, amount = amount, name = name, unit = unit, totalCost = tongtien.ToString(), unitPrice = unitprice });
            }
            this.dataGridImport.ItemsSource = list;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window
            {
                Title = "Them chi tiet nhap kho",
                Content = new PopupAddImportDetail()
            };
            window.ShowDialog();
        }
    }
}
