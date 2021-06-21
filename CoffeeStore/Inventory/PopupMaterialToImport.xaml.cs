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
    /// Interaction logic for PopupMaterialToImport.xaml
    /// </summary>
    public partial class PopupMaterialToImport : UserControl
    {
        public UserControl parent { get; set; }
        List<String> temp = new List<string>();
        public class MAterialObject
        {
            public string name { get; set; }
            public String unit { get; set; }
        
        }
        public PopupMaterialToImport(UserControl parent)
        {
            InitializeComponent();
            dataGridMaterialImport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            LoadData();
            this.parent = parent;
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        public void LoadData()
        {            
            var list = new ObservableCollection<MAterialObject>();
            BUS_Material mater = new BUS_Material();
            DataTable temp = mater.selectAll();
            foreach (DataRow row in temp.Rows)
            {
                string name = row["MaterialName"].ToString();
                string unit = row["Unit"].ToString();
                string use = row["isUse"].ToString();
                if (use == "1")
                    list.Add(new MAterialObject() {  name = name, unit = unit });
            }
            this.dataGridMaterialImport.ItemsSource = list;
        }

        private void cbCheck_Checked(object sender, RoutedEventArgs e)
        {
            MAterialObject row = (MAterialObject)dataGridMaterialImport.SelectedItem;
            temp.Add(row.name);
            //MessageBox.Show(row.name);
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(parent.GetType()==new InventoryImportADD().GetType())
                ((InventoryImportADD)parent).MaterName = temp;
            else ((InventoryImportEDIT)parent).MaterName = temp;
            Window.GetWindow(this).Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
