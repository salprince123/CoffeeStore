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
    /// Interaction logic for InventoryExportEDIT.xaml
    /// </summary>
    public partial class InventoryExportEDIT : UserControl
    {  
        public String selectionID = "";
        public String ImportName = "";
        public List<String> MaterName { get; set; }
        public List<InventoryObject> list = new List<InventoryObject>();
        public List<String> sqlCommand = new List<string>();

        MainWindow _context;
        public class InventoryObject
        {
            public int number { get; set; }
            public String name { get; set; }
            public String id { get; set; }
            public String unitPrice { get; set; }
            public String amount { get; set; }
            public String unit { get; set; }
            public String totalCost { get; set; }
            public String description { get; set; }
        }
        public InventoryExportEDIT()
        {
            InitializeComponent();
            if (selectionID != "")
                LoadData();
        }
        public InventoryExportEDIT(String id, String name, String date, MainWindow mainWindow)
        {
            this.ImportName = name;
            this.selectionID = id;
            InitializeComponent();
            LoadData();
            this._context = mainWindow;
            tbEmployeeName.Text = name;
            tbDate.Text = date;
            tbExportID.Text = id;
        }
        public void LoadData()
        {
            var list = new ObservableCollection<InventoryObject>();
            BUS_InventoryExport export = new BUS_InventoryExport();
            DataTable temp = export.SelectDetail(selectionID);
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                string descrip = row["Mô tả"].ToString();
                string unit = row["Unit"].ToString();
                list.Add(new InventoryObject() { number = list.Count + 1, amount = amount, name = name, description = descrip, unit = unit });
            }
            this.dataGridImport.ItemsSource = list;
            if (list.Count == 0) return;
            tbDescription.Text = list[0].description;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var screen = new InventoryExport(_context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }

        
    }
}
