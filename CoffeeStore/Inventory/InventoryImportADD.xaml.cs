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
    /// Interaction logic for InventoryImportADD.xaml
    /// </summary>
    public partial class InventoryImportADD : UserControl
    {
        public String selectionID = "";
        public class InventoryImportDetailObject
        {
            public int number { get; set; }
            public String name { get; set; }
            public String unitPrice { get; set; }
            public String amount { get; set; }
            public String unit { get; set; }
            public String totalCost { get; set; }
        }
        public InventoryImportADD()
        {
            InitializeComponent();
            if (selectionID != "")
                LoadData();
        }
        public InventoryImportADD(String id)
        {
            this.selectionID = id;
            InitializeComponent();
            if (selectionID != "")
                LoadData();
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "",
                Content = new PopupMaterialToImport(),
                Width = 540,
                Height = 450,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 1000 / 2) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 800 / 2) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }
    }
}
