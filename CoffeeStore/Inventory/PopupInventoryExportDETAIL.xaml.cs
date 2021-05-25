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
    /// Interaction logic for PopupInventoryExportDETAIL.xaml
    /// </summary>
    public partial class PopupInventoryExportDETAIL : UserControl
    {
        public String selectionID = "";
        public String ExportName = "";

        public class InventoryExportDetailObject
        {
            public int number { get; set; }
            public String name { get; set; }
            public String unitPrice { get; set; }
            public String amount { get; set; }
            public String unit { get; set; }
            public String description { get; set; }
        }
        public PopupInventoryExportDETAIL()
        {
            InitializeComponent();
        }
        public PopupInventoryExportDETAIL(String id, String name, String date)
        {
            this.selectionID = id;
            this.ExportName = name;
            InitializeComponent();

            tbDate.Text = date;
            tbEmployeeName.Text = name;
            tbExportID.Text = id;
            if (selectionID != "")
                LoadData();
        }
        public void LoadData()
        {
            var list = new ObservableCollection<InventoryExportDetailObject>();
            BUS_InventoryExport export = new BUS_InventoryExport();
            DataTable temp = export.SelectDetail(selectionID);
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                string descrip = row["Mô tả"].ToString();
                string unit = row["Unit"].ToString();
                list.Add(new InventoryExportDetailObject() { number = list.Count+1, amount = amount, name = name, description = descrip,unit=unit });
            }
            this.dataGridImport.ItemsSource = list;
            if (list.Count == 0) return;
            tbDescription.Text = list[0].description;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}