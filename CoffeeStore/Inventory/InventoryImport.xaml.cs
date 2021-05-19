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
        MainWindow _context;
        public String username { get; set; }
        public class InventoryImportObject
        {
            public int number { get; set; }
            public String ID { get; set; }
            public String InventoryDate { get; set; }
            public String EmployName { get; set; }
        }
        public InventoryImportObject row;
        public InventoryImport(MainWindow mainWindow)
        {
            InitializeComponent();
            this._context = mainWindow;
            LoadData();
        }
        public void LoadData()
        {
            username = "Tran Le Bao Chau";
            var list = new ObservableCollection<InventoryImportObject>();
            BUS_InventoryImport import = new BUS_InventoryImport();
            DataTable temp = import.selectAll();
            int number0 = 1;
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string employid = row["EmployeeName"].ToString();
                string id = row["importID"].ToString();
                string date = row["importDate"].ToString();
                list.Add(new InventoryImportObject() { ID = id, EmployName = employid, InventoryDate = date, number = number0 });
                number0++;
            }
            this.dataGridImport.ItemsSource = list;
            this.dataGridImport.Items.Refresh();
        }
        private void dataGridImport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            row = (InventoryImportObject)dataGridImport.SelectedItem;
        }

        private void AddImport_Click(object sender, RoutedEventArgs e)
        {
            var screen = new InventoryImportADD(username, _context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }


        private void btnWatch_Click(object sender, RoutedEventArgs e)
        {
            InventoryImportObject row = (InventoryImportObject)dataGridImport.SelectedItem;
            if (row != null)
            {
                Window window = new Window
                {
                    Title = "Chi tiết phiếu nhập",
                    Content = new PopupInventoryImportDETAIL(row.ID, row.EmployName, row.InventoryDate)
                    //Content = new PopupInventoryImportDETAIL("a","a","a")
                };
                window.ShowDialog();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ban co chac chan xoa?","Thong bao", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                InventoryImportObject row = (InventoryImportObject)dataGridImport.SelectedItem;
                BUS_InventoryImportDetail importDetail = new BUS_InventoryImportDetail();
                BUS_InventoryImport import = new BUS_InventoryImport();
                importDetail.Delete(row.ID);
                import.Delete(row.ID);
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            InventoryImportObject row = (InventoryImportObject)dataGridImport.SelectedItem;
            if (row == null) return;
                var screen = new InventoryImportEDIT(row.ID, row.EmployName, row.InventoryDate, _context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }
    }


}
