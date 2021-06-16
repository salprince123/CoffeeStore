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
            public String ID { get; set; }
            public String InventoryDate { get; set; }
            public String EmployName { get; set; }
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
            username = "Vo Dinh Ngoc Huyen";
            BUS_InventoryImport import = new BUS_InventoryImport();
            DataTable temp = import.selectAll();
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string employid = row["EmployeeName"].ToString();
                string id = row["importID"].ToString();
                string date = row["importDate"].ToString();
                mainList.Add(new InventoryImportObject() { ID = id, EmployName = employid, InventoryDate = date });
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
                    findList.Add(new InventoryImportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate });
                    continue;
                }
                if(obj.EmployName.Contains(id) && id != "")
                {
                    result += $"find object {obj.ID} {obj.EmployName}  {obj.InventoryDate} ";
                    findList.Add(new InventoryImportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate });
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
                            findList.Add(new InventoryImportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate });
                            continue;
                        }
                    }
                    else if (from != "" && to == "")
                    {
                        DateTime dtFrom = DateTime.ParseExact(from, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (dtFrom <= dtFind)
                        {
                            findList.Add(new InventoryImportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate });
                            continue;
                        }
                    }
                    else if (from == "" && to != "")
                    {
                        DateTime dtTo = DateTime.ParseExact(to, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (dtTo >= dtFind)
                        {
                            findList.Add(new InventoryImportObject() { ID = obj.ID, EmployName = obj.EmployName, InventoryDate = obj.InventoryDate });
                            continue;
                        }
                    }



                }
            }
            dataGridImport.ItemsSource = findList;
            dataGridImport.Items.Refresh();
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
                System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
                ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
                ((MainWindow)App.Current.MainWindow).Effect = objBlur;
                Window window = new Window
                {
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.None,
                    Title = "Chi tiết phiếu nhập",
                    Content = new PopupInventoryImportDETAIL(row.ID, row.EmployName, row.InventoryDate),
                    Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 1800 / 2) / 2,
                    Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 1000 / 2) / 2,
                    //Content = new PopupInventoryImportDETAIL("a","a","a")
                };
                window.ShowDialog();
                ((MainWindow)App.Current.MainWindow).Opacity = 1;
                ((MainWindow)App.Current.MainWindow).Effect = null;
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
