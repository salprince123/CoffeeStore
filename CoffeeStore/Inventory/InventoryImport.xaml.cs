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
    public partial class InventoryImport : UserControl
    {
        public String selectionID = "";
        public String selectionName = "";
        public class InventoryImportObject
        {
            public int number { get; set; }
            public String ID { get; set; }
            public String InventoryDate { get; set; }
            public String EmployName { get; set; }
            //this variable for button add/del/edit
            public String Action { get; set; }
        }
        public InventoryImportObject row;
        public InventoryImport()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            var list = new ObservableCollection<InventoryImportObject>();
            BUS_InventoryImport import = new BUS_InventoryImport();
            DataTable temp = import.selectAll();
            int number0 = 1;
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string employid = row["EmployeeID"].ToString();
                string id = row["importID"].ToString();
                string date = row["importDate"].ToString();
                list.Add(new InventoryImportObject() { ID = id, EmployName = employid, InventoryDate = date, number = number0, Action = "" });
                number0++;
            }
            this.dataGridImport.ItemsSource = list;
        }
        private void dataGridImport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            row = (InventoryImportObject)dataGridImport.SelectedItem;
            this.selectionID = row.ID;
            this.selectionName = row.EmployName;
        }

        private void AddImport_Click(object sender, RoutedEventArgs e)
        {
            
            Window window = new Window
            {
                Title = "My User Control Dialog",
                //Content = new InventoryImportADD()
            };
            window.ShowDialog();
        }

        private void edit_click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnWatch_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectionID != "")
            {
                Window window = new Window
                {
                    Title = "Chi tiết phiếu nhập",
                    Content = new InventoryImportDETAIL(selectionID,row.EmployName,row.InventoryDate)
                };
                window.ShowDialog();
            }
            else MessageBox.Show("Xin vui lòng chọn phiếu cần xem");
        }
    }
}
