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
            public String ID { get; set; }
            public String InventoryDate { get; set; }
            public String EmployName { get; set; }
        }
        public InventoryExportObject row;
        public InventoryExport()
        {
            InitializeComponent();
            dataGridExport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            LoadData();
        }
        public InventoryExport(MainWindow mainWindow)
        {
            InitializeComponent();
            dataGridExport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
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
            username = "Tran Le Bao Chau";
            list.Clear();
            BUS_InventoryExport export = new BUS_InventoryExport();
            DataTable temp = export.SelectAll();
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string employid = row["EmployeeName"].ToString();
                string id = row["exportID"].ToString();
                string date = row["exportDate"].ToString();
                list.Add(new InventoryExportObject() { ID = id, EmployName = employid, InventoryDate = date });

            }
            this.dataGridExport.Items.Refresh();
            this.dataGridExport.ItemsSource = list;
            
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
            //PLEASE USE THIS TO CREATE POPUP
            //System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            //((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            //((MainWindow)App.Current.MainWindow).Effect = objBlur;
            //Window window = new Window
            //{
            //    ResizeMode = ResizeMode.NoResize,
            //    WindowStyle = WindowStyle.None,
            //    Title = "Xóa ... ",
            //    Content = new PopupDeleteConfirm(), //Delete message
            //    Width = 380,
            //    Height = 210,
            //    Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 380) / 2,
            //    Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 210) / 2,
            //};
            //window.ShowDialog();
            //((MainWindow)App.Current.MainWindow).Opacity = 1;
            //((MainWindow)App.Current.MainWindow).Effect = null;

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
                    findList.Add(new InventoryExportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate });
                    continue;
                }
                if (obj.EmployName.Contains(id) && id != "")
                {
                    findList.Add(new InventoryExportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate });
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
                            findList.Add(new InventoryExportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate });
                            continue;
                        }
                    }
                    else if (from != "" && to == "")
                    {
                        DateTime dtFrom = DateTime.ParseExact(from, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (dtFrom <= dtFind)
                        {
                            findList.Add(new InventoryExportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate });
                            continue;
                        }
                    }
                    else if (from == "" && to != "")
                    {
                        DateTime dtTo = DateTime.ParseExact(to, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (dtTo >= dtFind)
                        {
                            findList.Add(new InventoryExportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate });
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
