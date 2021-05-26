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
        public InventoryExport()
        {
            InitializeComponent();
            LoadData();
        }
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
            var screen = new InventoryExportADD(username, _context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }
        private void btnWatch_Click(object sender, RoutedEventArgs e)
        {
            InventoryExportObject row = (InventoryExportObject)dataGridExport.SelectedItem;
            if (row != null)
            {
                System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
                ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
                ((MainWindow)App.Current.MainWindow).Effect = objBlur;
                Window window = new Window
                {
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.None,
                    Title = "Chi tiết phiếu xuất",
                    Content = new PopupInventoryExportDETAIL(row.ID, row.EmployName, row.InventoryDate),
                    Height = 630,
                    Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 1800 / 2) / 2,
                    Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 1300 / 2) / 2,
                    //Content = new PopupInventoryImportDETAIL("a","a","a")
                };
                window.ShowDialog();
                ((MainWindow)App.Current.MainWindow).Opacity = 1;
                ((MainWindow)App.Current.MainWindow).Effect = null;
            }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            InventoryExportObject row = (InventoryExportObject)dataGridExport.SelectedItem;
            if (row == null) return;
            var screen = new InventoryExportEDIT(row.ID, row.EmployName, row.InventoryDate, _context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ban co chac chan xoa?", "Thong bao", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                InventoryExportObject row = (InventoryExportObject)dataGridExport.SelectedItem;
                BUS_InventoryExportDetail detail = new BUS_InventoryExportDetail();
                BUS_InventoryExport export = new BUS_InventoryExport();
                detail.Delete(row.ID);
                export.Delete(row.ID);
                LoadData();
            }
        }
    }
}
