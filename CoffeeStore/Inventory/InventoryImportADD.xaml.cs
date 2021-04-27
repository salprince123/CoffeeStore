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
    /// Interaction logic for InventoryImportADD.xaml
    /// </summary>
    public partial class InventoryImportADD : UserControl
    {
        public class InventoryImportDetailObject
        {
            public int number { get; set; }
            public String name { get; set; }
            public String unitPrice { get; set; }
            public String amount { get; set; }
            public String unit { get; set; }
            public String totalCost { get; set; }
        }
        public InventoryImportADD()
        {
            InitializeComponent();
            LoadData();
        }
        public void LoadData()
        {
            var list = new ObservableCollection<InventoryImportDetailObject>();
            BUS_InventoryImport import = new BUS_InventoryImport();
            DataTable temp = import.selectDetail("Imp0000001");
            int number0 = 1;
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string name = row["Tên"].ToString();
                string amount = row["Số lượng"].ToString();
                string unitprice = row["Đơn giá"].ToString();
                string unit = row["Đơn vị tính"].ToString();
                int tongtien = int.Parse(amount) * int.Parse(unitprice);
                list.Add(new InventoryImportDetailObject() { number=number0 , amount= amount, name=name, unit=unit , totalCost=tongtien.ToString(), unitPrice=unitprice});
            }
            this.dataGridImport.ItemsSource = list;
        }
    }
}
