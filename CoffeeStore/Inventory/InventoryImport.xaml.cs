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
        bool findFlag = false;
        MainWindow _context;
        public String username { get; set; }
        public InventoryImportObject row;
        public List<InventoryImportObject> mainList = new List<InventoryImportObject>();
        public List<InventoryImportObject> findList = new List<InventoryImportObject>();
        public class InventoryImportObject
        {
            public String ID { get; set; }
            public String InventoryDate { get; set; }
            public String EmployName { get; set; }
        }
        public class MaterialObject
        {
            public string id { get; set; }
            public String Name { get; set; }
            public String Unit { get; set; }
            public String Amount { get; set; }
            //this variable for button add/del/edit
            public String IsUsing { get; set; }
        }
        public InventoryImport(MainWindow mainWindow)
        {
            InitializeComponent();
            dataGridImport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            this._context = mainWindow;
            LoadData();
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
        public void LoadData()
        {
            username = _context.GetCurrentEmpName();
            mainList.Clear();
            BUS_InventoryImport import = new BUS_InventoryImport();
            DataTable temp = import.selectAll();
            foreach (DataRow row in temp.Rows)
            {
                string employid = row["EmployeeName"].ToString();
                string id = row["importID"].ToString();
                string date = row["importDate"].ToString();
                mainList.Add(new InventoryImportObject() { ID = id, EmployName = employid, InventoryDate = date });
            }
            if (mainList.Count % 10 == 0)
                lblMaxPage.Content = mainList.Count / 10;
            else lblMaxPage.Content = mainList.Count / 10 + 1;
            if (int.Parse(lblMaxPage.Content.ToString()) == 0)
                this.tbNumPage.Text = "0";
            splitDataGrid(1);

        }
        public void splitDataGrid(int numpage)
        {
            try
            {
                List<InventoryImportObject> displayList = new List<InventoryImportObject>();
                int numberPerSheet = 10;
                if (mainList.Count < numberPerSheet * numpage)
                {
                    displayList = mainList.GetRange((numpage - 1) * numberPerSheet, mainList.Count - (numpage - 1) * numberPerSheet);
                }
                else displayList = mainList.GetRange((numpage - 1) * numberPerSheet, numberPerSheet);
                //displayList = list.GetRange(10, list.Count-10);
                this.dataGridImport.Items.Refresh();
                this.dataGridImport.ItemsSource = displayList;
            }
            catch (Exception)
            {
                MessageBox.Show("Something wrong!");
            }
        }
        public void splitDataGridFind(int numpage)
        {
            try
            {
                List<InventoryImportObject> displayList = new List<InventoryImportObject>();
                int numberPerSheet = 10;
                if (findList.Count < numberPerSheet * numpage)
                {
                    displayList = findList.GetRange((numpage - 1) * numberPerSheet, findList.Count - (numpage - 1) * numberPerSheet);
                }
                else displayList = findList.GetRange((numpage - 1) * numberPerSheet, numberPerSheet);
                //displayList = list.GetRange(10, list.Count-10);
                this.dataGridImport.Items.Refresh();
                this.dataGridImport.ItemsSource = displayList;
            }
            catch (Exception)
            {
                MessageBox.Show("Something wrong!");
            }
        }
        public void findImport()
        {
            findList.Clear();
            String id = tbIDFind.Text.Trim();
            DateTime fromTime = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", null);
            DateTime toTime = DateTime.ParseExact("01/01/2100", "dd/MM/yyyy", null);
            try
            {
                fromTime = tbDateStart.SelectedDate.Value;
            }
            catch (Exception) { }
            try
            {
                toTime = tbDateEnd.SelectedDate.Value;
            }
            catch (Exception) { }
            if (toTime < fromTime)
            {
                MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc");
                return;
            }
                
            //MessageBox.Show($"{ fromTime} {toTime}");
            foreach (InventoryImportObject obj in mainList.ToList())
            {
                
                DateTime importTime = DateTime.ParseExact(obj.InventoryDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if(fromTime <= importTime && importTime <= toTime)
                {
                    if( id != "")
                    {
                        if(obj.ID.Contains(id) || obj.EmployName.Contains(id))
                            findList.Add(new InventoryImportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate });
                    }
                    else
                    {
                        findList.Add(new InventoryImportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate });
                    }
                }
            }
            if (mainList.Count % 10 == 0)
                lblMaxPage.Content = findList.Count / 10;
            else lblMaxPage.Content = findList.Count / 10 + 1;
            if (int.Parse(lblMaxPage.Content.ToString()) == 0)
                this.tbNumPage.Text = "0";
            splitDataGridFind(int.Parse(tbNumPage.Text));
            this.findFlag = true;
        }

        private void AddImport_Click(object sender, RoutedEventArgs e)
        {
            var screen = new InventoryImportADD(_context.GetCurrentEmpName(), _context);
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
                System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
                ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
                ((MainWindow)App.Current.MainWindow).Effect = objBlur;
                Window window = new Window
                {
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.None,
                    Title = "Chi tiết phiếu nhập",
                    Height = 630,
                    Width = 500,
                    Content = new PopupInventoryImportDETAIL(row.ID, row.EmployName, row.InventoryDate),
                    Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 500) / 2,
                    Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 630) / 2,
                    //Content = new PopupInventoryImportDETAIL("a","a","a")
                };
                window.ShowDialog();
                ((MainWindow)App.Current.MainWindow).Opacity = 1;
                ((MainWindow)App.Current.MainWindow).Effect = null;
            }
        }
        public Dictionary<String, int> loadAmountofMaterial()
        {
            Dictionary<String, int> mapNameAmount = new Dictionary<string, int>();
            Dictionary<String, String> mapNameUnit = new Dictionary<string, string>();
            //With import amount
            BUS_InventoryImportDetail import = new BUS_InventoryImportDetail();
            DataTable temp = import.SelectAllImportDetailGroupByName();
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                string use = row["isUse"].ToString();
                if (use == "1")
                    mapNameAmount[name] = int.Parse(amount);
            }
            //With unit
            BUS_Material mater = new BUS_Material();
            DataTable tempMater = mater.selectAll();
            foreach (DataRow row in tempMater.Rows)
            {
                string name = row["MaterialName"].ToString();
                string unit = row["Unit"].ToString();
                string use = row["isUse"].ToString();
                if (use == "1")
                    mapNameUnit[name] = unit;
            }
            //calculate amount in stock = import - export (if have)
            BUS_InventoryExportDetail export = new BUS_InventoryExportDetail();
            DataTable temp1 = export.SelectAllExportDetailGroupByName();
            foreach (DataRow row in temp1.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                if (mapNameAmount.ContainsKey(name))
                    mapNameAmount[name] -= int.Parse(amount);
            }
            foreach (KeyValuePair<string, string> name in mapNameUnit)
            {
                if (!mapNameAmount.ContainsKey(name.Key))
                    mapNameAmount[name.Key] = 0;
            }
            //MessageBox.Show(mapNameAmount.Count.ToString());
            return mapNameAmount;
        }
        public Dictionary<String, int> loadAllMaterialName(String importID)
        {
            Dictionary<String, int> result = new Dictionary<string, int>();
            BUS_InventoryImport import = new BUS_InventoryImport();
            DataTable temp = import.SelectAllMaterialNameFromDetail(importID);
            foreach (DataRow row in temp.Rows)
            {
                String  name= row["MaterialName"].ToString() ;
                String amount = row["Amount"].ToString();
                result[name] = int.Parse(amount) ;
            }
            return result;
        }
        public bool checkDeleteCondition(String importID)
        {
            Dictionary<String, int> mapNameAmountInStock = loadAmountofMaterial();
            Dictionary<String, int> mapNameAmountImport = loadAllMaterialName(importID);
            foreach (KeyValuePair<string, int> name in mapNameAmountImport)
            {
                try
                {
                    int amountInStock = mapNameAmountInStock[name.Key];
                    int amountImport = name.Value;
                    if (amountImport > amountInStock)
                        return false;
                }
                catch(Exception)
                {
                    return false;
                }
                
            }
            return true;
        }
        /* 
         About how  to check if the amount in the importDetail > the amount in stock 
        Step 1: Load Name & Amount have in stock into a Map (Stock Map)
        Step 2: Load all name& Amount of the import Want-to-remove into a Map (Import Map)
        Step 3: foreach element in Stock Map , we will comparision if Import > Stock 
                if(Import > Stock) it means the material have been used -> so we cant delete this import
         */
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
        
            InventoryImportObject row = (InventoryImportObject)dataGridImport.SelectedItem;
            DateTime importDate = DateTime.ParseExact(row.InventoryDate, "dd/MM/yyyy", null);            
            if (!checkDeleteCondition(row.ID) || ((DateTime.Now - importDate) > TimeSpan.FromDays(2)))
            {
                MessageBox.Show($"Bạn không thể xóa phiếu này");
                return;
            }
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "xóa phiếu nhập kho! ",
                Content = new PopupDeleteConfirm(this, row.ID), //delete message
                Width = 380,
                Height= 210,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 380) / 2,
                Top= (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height- 210) / 2,
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }
        public void Delete(String id)
        {
            BUS_InventoryImportDetail importDetail = new BUS_InventoryImportDetail();
            BUS_InventoryImport import = new BUS_InventoryImport();
            importDetail.Delete(id);
            import.Delete(id);
            LoadData();
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
            //tbDateStart.Text = tbDateStart.SelectedDate.Value.ToString("dd/MM/yyyy");
            //Keyboard.Focus(tbDateStart);
        }

        private void dpTo_CalendarClosed(object sender, RoutedEventArgs e)
        {
            
            //tbDateEnd.Text = tbDateEnd.SelectedDate.Value.ToString("dd/MM/yyyy");
            //Keyboard.Focus(tbDateEnd);
        }
        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            if (int.Parse(tbNumPage.Text) > 1)
            {
                tbNumPage.Text = (int.Parse(tbNumPage.Text) - 1).ToString();
                if (!findFlag)
                    splitDataGrid(int.Parse(tbNumPage.Text));
                else splitDataGridFind(int.Parse(tbNumPage.Text));
            }
        }

        private void btNext_Click(object sender, RoutedEventArgs e)
        {
            if ((int.Parse(lblMaxPage.Content.ToString()) - int.Parse(tbNumPage.Text)) >= 1)
            {
                tbNumPage.Text = (int.Parse(tbNumPage.Text) + 1).ToString();
                if (!findFlag )
                    splitDataGrid(int.Parse(tbNumPage.Text));
                else splitDataGridFind(int.Parse(tbNumPage.Text));
            }
        }
    }


}
