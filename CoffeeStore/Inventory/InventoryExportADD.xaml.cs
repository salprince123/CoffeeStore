using CoffeeStore.BUS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public class CourseValidationRule : ValidationRule
        {
            public override ValidationResult Validate(object value,
                System.Globalization.CultureInfo cultureInfo)
            {
                InventoryExportDetailObject course = (value as BindingGroup).Items[0] as InventoryExportDetailObject;
                int s;
                if (Int32.TryParse(course.amount, out s) && s < 2)
                {
                    return new ValidationResult(false,
                        "Start Date must be earlier than End Date.");
                }
                else
                {
                    return ValidationResult.ValidResult;
                }
            }
        }
        public class InventoryExportDetailObject : INotifyPropertyChanged
        {
            public String name { get; set; }
            public String id { get; set; }
            public String unitPrice { get; set; }
            public String _amount;
            public string amount
            {
                get
                {
                    return this._amount;
                }

                set
                {
                    if (value != _amount)
                    {
                        _amount = value;
                        OnPropertyChanged();
                    }
                }
            }
            public String unit { get; set; }
            public String reason { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

        }
        public InventoryExportADD()
        {
            InitializeComponent();
            dataGridMaterialExport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
        }
        public InventoryExportADD(String id, MainWindow mainWindow)
        {
            this.selectionID = id;
            InitializeComponent();
            dataGridMaterialExport.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            tbDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            tbEmployeeName.Text = id;
            this._context = mainWindow;
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
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

        public int findInList(String id)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].id == id)
                    return i;
            }
            return -1;
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
                if (findInList(id) == -1)
                    list.Add(new InventoryExportDetailObject() { id = id, name = name, unit = unit });
            }
            dataGridMaterialExport.ItemsSource = list;
            dataGridMaterialExport.Items.Refresh();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            InventoryExportDetailObject row = (InventoryExportDetailObject)dataGridMaterialExport.SelectedItem;
            if (row != null)
            {
                try
                {
                    list.RemoveAt(findInList(row.id));
                    dataGridMaterialExport.Items.Refresh();
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
            BUS_InventoryExportDetail detail = new BUS_InventoryExportDetail();
            detail.ImportList(sqlString);
            var screen = new InventoryExport(_context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }

        private void DataGridTextColumn_Error(object sender, ValidationErrorEventArgs e)
        {

        }
    }
}
