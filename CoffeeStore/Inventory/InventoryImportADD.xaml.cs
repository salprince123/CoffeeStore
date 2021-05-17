using CoffeeStore.BUS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public List<String> MaterName { get; set; }
        public List<InventoryImportDetailObject> list = new List<InventoryImportDetailObject>();
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
        }
        
        public InventoryImportADD(String id)
        {
            this.selectionID = id;
            InitializeComponent();
            tbDate.Text = DateTime.Now.ToString("dd/mm/yyyy");
            tbEmployeeName.Text = id;
        }
        

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window
            {
                Title = "Nguyen vat lieu trong kho",
                Content = new PopupMaterialToImport(this),
                Width = 600,
                Height = 900
            };
            window.ShowDialog();
            
            BUS_Material mater = new BUS_Material();
            DataTable temp = mater.selectByName( this.MaterName);
            foreach (DataRow row in temp.Rows)
            {
                string name = row["MaterialName"].ToString();
                string unit = row["Unit"].ToString();
                list.Add(new InventoryImportDetailObject() { number=list.Count+1, name=name, unit=unit });
            }
            dataGridImport.ItemsSource = list;
            dataGridImport.Items.Refresh();

        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (InventoryImportDetailObject obj in list)
            {
                if (obj.unitPrice != null && obj.amount != null)
                    obj.totalCost = (int.Parse(obj.unitPrice) * int.Parse(obj.amount)).ToString();
            }
            dataGridImport.Items.Refresh();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            InventoryImportDetailObject row = (InventoryImportDetailObject)dataGridImport.SelectedItem;
            if (row != null)
            {
                try
                {
                    list.RemoveAt(row.number - 1);
                    for (int i = 0; i < list.Count; i++)
                        list[i].number= i+1;

                    dataGridImport.Items.Refresh();
                }
                catch (Exception) { }
            }
        }
    }
}
