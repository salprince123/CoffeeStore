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
            username = _context.GetCurrentEmpName();
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
            var screen = new InventoryExportADD(_context.GetCurrentEmpName(), _context);
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
                    Width = 500,
                    Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 500) / 2,
                    Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 630) / 2,
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

            InventoryExportObject row = (InventoryExportObject)dataGridExport.SelectedItem;
            //get time of import 
            DateTime importDate = DateTime.ParseExact(row.InventoryDate, "dd/MM/yyyy", null);

            if((DateTime.Now - importDate) > TimeSpan.FromDays(2) )
            {
                MessageBox.Show($"Bạn chỉ có thể xóa phiếu xuất kho được tạo trong vòng 2 ngày!");
                return;
            }
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "xóa phiếu xuất kho! ",
                Content = new PopupDeleteConfirm(this, row.ID), //delete message
                Width = 380,
                Height = 210,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 380) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 210) / 2,
            };
            window.ShowDialog();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;

        }
        public void Delete(String id)
        {
            
            BUS_InventoryExportDetail detail = new BUS_InventoryExportDetail();
            BUS_InventoryExport export = new BUS_InventoryExport();
            detail.Delete(id);
            export.Delete(id);
            LoadData();
        }
        public void findExport()
        {
            findList.Clear();
            String id = tbFind.Text.Trim();
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
            foreach (InventoryExportObject obj in list.ToList())
            {

                DateTime importTime = DateTime.ParseExact(obj.InventoryDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if (fromTime <= importTime && importTime <= toTime)
                {
                    if (id != "")
                    {
                        if (obj.ID.Contains(id) || obj.EmployName.Contains(id))
                            findList.Add(new InventoryExportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate });
                    }
                    else
                    {
                        findList.Add(new InventoryExportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate });
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
