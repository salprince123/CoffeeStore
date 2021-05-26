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
        public List<InventoryExportObject> list = new List<InventoryExportObject>();
        public List<InventoryExportObject> findList = new List<InventoryExportObject>();
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
                Window window = new Window
                {
                    Title = "Chi tiết phiếu xuất",
                    Content = new PopupInventoryExportDETAIL(row.ID, row.EmployName, row.InventoryDate)
                    //Content = new PopupInventoryImportDETAIL("a","a","a")
                };
                window.ShowDialog();
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
        private void dpFrom_CalendarClosed(object sender, RoutedEventArgs e)
        {
            try
            {
                tbDateStart.Text = dpFrom.SelectedDate.Value.ToString("dd/MM/yyyy");
                dpFrom.Text = "";
                Keyboard.Focus(tbDateStart);
            }
            catch (Exception)
            { }
            
        }

        private void dpTo_CalendarClosed(object sender, RoutedEventArgs e)
        {
            try
            {
                tbDateEnd.Text = dpTo.SelectedDate.Value.ToString("dd/MM/yyyy");
                dpTo.Text = "";
                Keyboard.Focus(tbDateEnd);
            }
            catch (Exception)
            { }
        }
        public void findExport()
        {
            findList.Clear();
            String from = tbDateStart.Text;
            String to = tbDateEnd.Text;
            String id = tbFind.Text.Trim();
            if (from == "" && to == "" && id == "")
            {
                this.dataGridExport.ItemsSource = list;
                this.dataGridExport.Items.Refresh();
                return;
            }
            foreach (InventoryExportObject obj in list.ToList())
            {
                if (obj.ID.Contains(id) && id != "")
                {
                    findList.Add(new InventoryExportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate, number = findList.Count + 1 });
                    continue;
                }
                if (obj.EmployName.Contains(id) && id != "")
                {
                    findList.Add(new InventoryExportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate, number = findList.Count + 1 });
                    continue;
                }
                if (from != "" || to != "")
                {
                    DateTime dtFind = DateTime.ParseExact(obj.InventoryDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (from != "" && to != "")
                    {
                        DateTime dtFrom = DateTime.ParseExact(from, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime dtTo = DateTime.ParseExact(to, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (dtFrom <= dtFind && dtTo >= dtFind)
                        {
                            findList.Add(new InventoryExportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate, number = findList.Count + 1 });
                            continue;
                        }
                    }
                    else if (from != "" && to == "")
                    {
                        DateTime dtFrom = DateTime.ParseExact(from, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (dtFrom <= dtFind)
                        {
                            findList.Add(new InventoryExportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate, number = findList.Count + 1 });
                            continue;
                        }
                    }
                    else if (from == "" && to != "")
                    {
                        DateTime dtTo = DateTime.ParseExact(to, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (dtTo >= dtFind)
                        {
                            findList.Add(new InventoryExportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate, number = findList.Count + 1 });
                            continue;
                        }
                    }
                }
            }
            dataGridExport.ItemsSource = findList;
            dataGridExport.Items.Refresh();
        }
        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            findExport();
        }
    }
}
