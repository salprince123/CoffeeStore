﻿using CoffeeStore.BUS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
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
        public String selectionID = "";
        public List<String> MaterName { get; set; }
        public List<InventoryImportDetailObject> list = new List<InventoryImportDetailObject>();
        MainWindow _context;
        
        public class InventoryImportDetailObject : INotifyPropertyChanged
        {
            public int number { get; set; }
            public String name { get; set; }
            public String id { get; set; }
            public string _unitPrice;
            public string unitPrice
            {
                get
                {
                    return this._unitPrice;
                }

                set
                {
                    if (value != _unitPrice)
                    {
                        _unitPrice = value;
                        OnPropertyChanged();
                        OnPropertyChanged("totalCost");
                    }
                }
            }
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
                        OnPropertyChanged("totalCost");
                    }
                }
            }
            public String unit { get; set; }
            public String totalCost
            {
                get {
                    int temp, temp1;
                    if(int.TryParse(unitPrice, out temp )&& int.TryParse(amount, out temp1))
                        return (temp*temp1).ToString();
                    return null;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            
        }
        public InventoryImportADD()
        {
            InitializeComponent();
        }
        
        public InventoryImportADD(String id, MainWindow mainWindow)
        {
            this.selectionID = id;
            InitializeComponent();
            tbDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            tbEmployeeName.Text = id;
            this._context = mainWindow;
        }
        
        public bool containInList (String id)
        {
            foreach (InventoryImportDetailObject obj in list)
            {
                if (obj.id == id)
                    return true ;
            }
            return false;
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
                Content = new PopupMaterialToImport(this),
                Width = 540,
                Height = 450,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 1000 / 2) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 800 / 2) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
            
            BUS_Material mater = new BUS_Material();
            DataTable temp = mater.selectByName( this.MaterName);
            if (temp == null) return;
            foreach (DataRow row in temp.Rows)
            {
                string name = row["MaterialName"].ToString();
                string unit = row["Unit"].ToString();
                string id = row["MaterialID"].ToString();
                if(!containInList(id))
                    list.Add(new InventoryImportDetailObject() { id=id,number=list.Count+1, name=name, unit=unit });
            }
            dataGridImport.ItemsSource = list;
            dataGridImport.Items.Refresh();
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            
            dataGridImport.Items.Refresh();
            // save in database
            //insert InventoryImport
            BUS_InventoryImport import = new BUS_InventoryImport();
            String newImportID = import.Create(tbEmployeeName.Text, tbDate.Text);
            //MessageBox.Show(newImportID);
            //insert InventoryImportdetails
            if (newImportID == null) return;
            List<String> sqlString = new List<string>();
            foreach (InventoryImportDetailObject obj in list)
            {
                string temp = $"insert into InventoryImportDetail values ('{newImportID}','{obj.id}','{obj.amount}','{obj.unitPrice}')";
                sqlString.Add(temp);
            }
            BUS_InventoryImportDetail detail = new BUS_InventoryImportDetail();
            detail.ImportList(sqlString);
            var screen = new InventoryImport(_context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            InventoryImportDetailObject row = (InventoryImportDetailObject)dataGridImport.SelectedItem;
            if (row != null)
            {
                try
                {
                    list.RemoveAt(row.number - 1);
                    for (int i = 0; i < list.Count; i++)
                        list[i].number= i+1;
                    dataGridImport.Items.Refresh();
                }
                catch (Exception) { }
            }
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            var screen = new InventoryImport(_context);
            if (screen != null)
            {
                this._context.StackPanelMain.Children.Clear();
                this._context.StackPanelMain.Children.Add(screen);
            }
        }

    }
}
