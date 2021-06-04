using CoffeeStore.BUS;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for InventoryExportADD.xaml
    /// </summary>
    public partial class InventoryExportADD : UserControl
    {
        public String selectionID = "";
        public List<String> MaterName { get; set; }
        public List<InventoryExportDetailObject> list = new List<InventoryExportDetailObject>();
        MainWindow _context;
        public class InventoryExportDetailObject
        {
            public int number { get; set; }
            public String name { get; set; }
            public String id { get; set; }
            public String unitPrice { get; set; }
            public String amount { get; set; }
            public String unit { get; set; }
            public String reason { get; set; }
        }
        public InventoryExportADD()
        {
            InitializeComponent();
        }
        public InventoryExportADD(String id, MainWindow mainWindow)
        {
            this.selectionID = id;
            InitializeComponent();
            tbDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            tbEmployeeName.Text = id;
            this._context = mainWindow;
        }
        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            var screen = new InventoryExport(_context);
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
                Content = new PopupMaterialToExport(this),
                Width = 540,
                Height = 450,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 1000 / 2) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 800 / 2) / 2,
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
                list.Add(new InventoryExportDetailObject() { id = id, number = list.Count + 1, name = name, unit = unit });
            }
            dataGridImport.ItemsSource = list;
            dataGridImport.Items.Refresh();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            InventoryExportDetailObject row = (InventoryExportDetailObject)dataGridImport.SelectedItem;
            if (row != null)
            {
                try
                {
                    list.RemoveAt(row.number - 1);
                    for (int i = 0; i < list.Count; i++)
                        list[i].number = i + 1;
                    dataGridImport.Items.Refresh();
                }
                catch (Exception) { }
            }
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            // save in database
            //insert InventoryExport
            BUS_InventoryExport export = new BUS_InventoryExport();
            String newExportID = export.Create(tbEmployeeName.Text, tbDate.Text);
            //MessageBox.Show(newImportID);
            //insert InventoryImportdetails
            if (newExportID == null) return;
            List<String> sqlString = new List<string>();
            foreach (InventoryExportDetailObject obj in list)
            {
                string temp = $"insert into InventoryExportDetail values ('{newExportID}','{obj.id}','{obj.amount}','{tbDescription.Text}')";
                sqlString.Add(temp);
            }
            BUS_InventoryImportDetail detail = new BUS_InventoryImportDetail();
            detail.ImportList(sqlString);
            var screen = new InventoryExport(_context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }
    }
}
