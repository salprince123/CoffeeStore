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
        public InventoryImportObject row;
        public List<InventoryImportObject> mainList = new List<InventoryImportObject>();
        public List<InventoryImportObject> findList = new List<InventoryImportObject>();
        public class InventoryImportObject
        {
            public int number { get; set; }
            public String ID { get; set; }
            public String InventoryDate { get; set; }
            public String EmployName { get; set; }
        }
       
        public InventoryImport(MainWindow mainWindow)
        {
            InitializeComponent();
            this._context = mainWindow;
            LoadData();
        }
        public void LoadData()
        {
            username = "Vo Dinh Ngoc Huyen";
            BUS_InventoryImport import = new BUS_InventoryImport();
            DataTable temp = import.selectAll();
            int number0 = 1;
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string employid = row["EmployeeName"].ToString();
                string id = row["importID"].ToString();
                string date = row["importDate"].ToString();
                mainList.Add(new InventoryImportObject() { ID = id, EmployName = employid, InventoryDate = date, number = number0 });
                number0++;
            }
            this.dataGridImport.ItemsSource = mainList;
            this.dataGridImport.Items.Refresh();
        }
        public void findImport()
        {
            findList.Clear();
            String from = tbDateStart.Text;
            String to = tbDateEnd.Text;
            String id = tbIDFind.Text.Trim();
            if(from =="" && to =="" && id=="")
            {
                this.dataGridImport.ItemsSource = mainList;
                this.dataGridImport.Items.Refresh();
                return;
            }
            string result = "";
            foreach (InventoryImportObject obj in mainList.ToList())
            {
                if (obj.ID.Contains(id) && id!="")
                {
                    result += $"find object {obj.ID} {obj.EmployName}  {obj.InventoryDate} ";
                    findList.Add(new InventoryImportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate, number = findList.Count+1});
                    continue;
                }
                if(obj.EmployName.Contains(id) && id != "")
                {
                    result += $"find object {obj.ID} {obj.EmployName}  {obj.InventoryDate} ";
                    findList.Add(new InventoryImportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate, number = findList.Count + 1 });
                    continue;
                }    
                if(from != "" || to != "") 
                {
                    DateTime dtFind = DateTime.ParseExact(obj.InventoryDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (from!="" && to != "")
                    {
                        DateTime dtFrom = DateTime.ParseExact(from, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime dtTo = DateTime.ParseExact(to, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (dtFrom <= dtFind && dtTo >= dtFind)
                        {
                            findList.Add(new InventoryImportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate, number = findList.Count + 1 });
                            continue;
                        }
                    }
                    else if (from != "" && to == "")
                    {
                        DateTime dtFrom = DateTime.ParseExact(from, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (dtFrom <= dtFind)
                        {
                            findList.Add(new InventoryImportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate, number = findList.Count + 1 });
                            continue;
                        }
                    }
                    else if (from == "" && to != "")
                    {
                        DateTime dtTo = DateTime.ParseExact(to, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (dtTo >= dtFind)
                        {
                            findList.Add(new InventoryImportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate, number = findList.Count + 1 });
                            continue;
                        }
                    }



                }
            }
            dataGridImport.ItemsSource = findList;
            dataGridImport.Items.Refresh();
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

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            findImport();
        }

        private void dpFrom_CalendarClosed(object sender, RoutedEventArgs e)
        {
            tbDateStart.Text = dpFrom.SelectedDate.Value.ToString("dd/MM/yyyy");
            dpFrom.Text = "";
            Keyboard.Focus(tbDateStart);
        }

        private void dpTo_CalendarClosed(object sender, RoutedEventArgs e)
        {
            tbDateEnd.Text = dpTo.SelectedDate.Value.ToString("dd/MM/yyyy");
            dpTo.Text = "";
            Keyboard.Focus(tbDateEnd);
        }
    }


}
