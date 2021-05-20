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
    /// Interaction logic for InventoryExport.xaml
    /// </summary>
    public partial class InventoryExport : UserControl
    {
        public String selectionID = "";
        public String selectionName = "";
        MainWindow _context;
        public String username { get; set; }
        public class InventoryExportObject
        {
            public int number { get; set; }
            public String ID { get; set; }
            public String InventoryDate { get; set; }
            public String EmployName { get; set; }
        }
        public InventoryExportObject row;
        public InventoryExport(MainWindow mainWindow)
        {
            InitializeComponent();
            this._context = mainWindow;
            LoadData();
        }
        public void LoadData()
        {
            username = "Tran Le Bao Chau";
            var list = new ObservableCollection<InventoryExportObject>();
            BUS_InventoryExport export = new BUS_InventoryExport();
            DataTable temp = export.SelectAll();
            int number0 = 1;
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string employid = row["EmployeeName"].ToString();
                string id = row["exportID"].ToString();
                string date = row["exportDate"].ToString();
                list.Add(new InventoryExportObject() { ID = id, EmployName = employid, InventoryDate = date, number = number0 });
                number0++;
            }
            this.dataGridExport.ItemsSource = list;
            this.dataGridExport.Items.Refresh();
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            //_context.SwitchToInventoryExportAdd();
        }
    }
}
