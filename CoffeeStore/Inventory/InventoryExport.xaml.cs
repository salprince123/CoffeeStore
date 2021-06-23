﻿using CoffeeStore.BUS;
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
        bool findFlag = false;
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
            if (list.Count % 10 == 0)
                lblMaxPage.Content = list.Count / 10;
            else lblMaxPage.Content = list.Count / 10 + 1;
            if (int.Parse(lblMaxPage.Content.ToString()) == 0)
                this.tbNumPage.Text = "0";
            splitDataGrid(1);

        }

        public void splitDataGrid(int numpage)
        {
            try
            {
                List<InventoryExportObject> displayList = new List<InventoryExportObject>();
                int numberPerSheet = 10;
                if (list.Count < numberPerSheet * numpage)
                {
                    displayList = list.GetRange((numpage - 1) * numberPerSheet, list.Count - (numpage - 1) * numberPerSheet);
                }
                else displayList = list.GetRange((numpage - 1) * numberPerSheet, numberPerSheet);
                //displayList = list.GetRange(10, list.Count-10);
                this.dataGridExport.Items.Refresh();
                this.dataGridExport.ItemsSource = displayList;
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
                List<InventoryExportObject> displayList = new List<InventoryExportObject>();
                int numberPerSheet = 10;
                if (findList.Count < numberPerSheet * numpage)
                {
                    displayList = findList.GetRange((numpage - 1) * numberPerSheet, findList.Count - (numpage - 1) * numberPerSheet);
                }
                else displayList = findList.GetRange((numpage - 1) * numberPerSheet, numberPerSheet);
                //displayList = list.GetRange(10, list.Count-10);
                this.dataGridExport.Items.Refresh();
                this.dataGridExport.ItemsSource = displayList;
            }
            catch (Exception)
            {
                MessageBox.Show("Something wrong!");
            }
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
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
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
                WindowStartupLocation = WindowStartupLocation.CenterScreen
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
            findFlag = true;
            dataGridExport.ItemsSource = findList;
            dataGridExport.Items.Refresh();
        }
        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            findExport();
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
                if (!findFlag)
                    splitDataGrid(int.Parse(tbNumPage.Text));
                else splitDataGridFind(int.Parse(tbNumPage.Text));
            }
        }

        private void tbNumPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void tbNumPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.Any(x => Char.IsDigit(x));
            if (e.Text.Contains(" "))
                e.Handled = false;
        }
    }
}
