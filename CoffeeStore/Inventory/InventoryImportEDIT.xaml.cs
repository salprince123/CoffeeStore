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
        public List<String> MaterName { get; set; }
        public List<InventoryImportDetailObject> list = new List<InventoryImportDetailObject>();
        public List<String> sqlCommand = new List<string>();

        MainWindow _context;        
        public class InventoryImportDetailObject
        {
            public String name { get; set; }
            public String id { get; set; }
            public String unitPrice { get; set; }
            public String amount { get; set; }
            public String unit { get; set; }
            public String totalCost { get; set; }
        }
        public InventoryImportEDIT()
        {
            InitializeComponent();
            dataGridMaterialImport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            if (selectionID != "")
                LoadData();
        }
        public InventoryImportEDIT(String id, String importname, String importdate, MainWindow mainWindow)
        {
            this.selectionID = id;
            this.ImportName = importname;
            InitializeComponent();
            dataGridMaterialImport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            if (selectionID != "")
                LoadData();
            this._context = mainWindow;
            tbEmployeeName.Text = importname;
            tbDate.Text = importdate;
            tbImportID.Text = id;
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        public void LoadData()
        {
            BUS_InventoryImport import = new BUS_InventoryImport();
            DataTable temp = import.selectDetail(selectionID);
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string id = row["ID"].ToString();
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                string unitprice = row["Đơn giá"].ToString();
                string unit = row["Đơn vị tính"].ToString();
                int tongtien = int.Parse(amount) * int.Parse(unitprice);
                list.Add(new InventoryImportDetailObject() { id= id, amount = amount, name = name, unit = unit, totalCost = tongtien.ToString(), unitPrice = unitprice });
            }
            this.dataGridMaterialImport.ItemsSource = list;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var screen = new InventoryImport(_context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
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
                Content = new PopupMaterialToImport(this),
                Width = 540,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;

            BUS_Material mater = new BUS_Material();
            DataTable temp = mater.selectByName(this.MaterName);
            if (temp == null) return;
            foreach (DataRow row in temp.Rows)
            {
                string name = row["MaterialName"].ToString();
                string unit = row["Unit"].ToString();
                string id = row["MaterialID"].ToString();
                list.Add(new InventoryImportDetailObject() { id = id,  name = name, unit = unit });
            }
            dataGridMaterialImport.ItemsSource = list;
            dataGridMaterialImport.Items.Refresh();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            InventoryImportDetailObject row = (InventoryImportDetailObject)dataGridMaterialImport.SelectedItem;
            if (row != null)
            {
                /*try
                {
                    list.RemoveAt(row.number - 1);
                    for (int i = 0; i < list.Count; i++)
                        list[i].number = i + 1;
                    dataGridImport.Items.Refresh();
                    
                }
                catch (Exception) { }*/
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(sqlCommand[0]);
            foreach (InventoryImportDetailObject obj in list)
            {
                string temp = $"insert into InventoryImportDetail values ('{selectionID}','{obj.id}','{obj.amount}','{obj.unitPrice}')";
                sqlCommand.Add(temp);
            }
            BUS_InventoryImportDetail detail = new BUS_InventoryImportDetail();
            detail.Delete(selectionID);
            detail.ImportList(sqlCommand);            
            var screen = new InventoryImport(_context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }
        private void tbPrice_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= tbPrice_GotFocus;
        }

        private void tbAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= tbAmount_GotFocus;
        }
    }
}
