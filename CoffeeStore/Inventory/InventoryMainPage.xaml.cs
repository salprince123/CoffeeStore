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
            /*var list = new ObservableCollection<InventoryObject>();
            BUS_InventoryImport import = new BUS_InventoryImport();
            DataTable temp = import.selectAll();
            int number0 = 1;
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string employid = row["EmployeeID"].ToString();
                string id = row["importID"].ToString();
                string date = row["importDate"].ToString();
                //list.Add(new InventoryObject() { ID = id, EmployName = employid, InventoryDate = date, number = number0, Action = "" });
            }
            this.dataGridInfo.ItemsSource = list;*/
        }
    }
}
