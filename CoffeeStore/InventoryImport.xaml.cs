using CoffeeStore.BUS;
using CoffeeStore.Inventory;
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

namespace CoffeeStore
{
    /// <summary>
    /// Interaction logic for InventoryImport.xaml
    /// </summary>
    public partial class InventoryImport : UserControl
    {
        public class InventoryImportObject
        {
            public int number { get; set; }
            public String ID { get; set; }
            public String InventoryDate { get; set; }
            public String EmployName { get; set; }
            //this variable for button add/del/edit
            public String Action { get; set; }
        }
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
                list.Add(new InventoryImportObject() { ID = id, EmployName = employid, InventoryDate=date, number=number0, Action= ""});
            }
            this.dataGridImport.ItemsSource = list;
        }
        private void dataGridImport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void AddImport_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window
            {
                Title = "My User Control Dialog",
                Content = new InventoryImportADD()
            };
            window.ShowDialog();
        }
    }
}
