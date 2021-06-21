﻿using CoffeeStore.BUS;
using System;
using System.Collections.Generic;
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

namespace CoffeeStore.IncomeAndPayment
{
    /// <summary>
    /// Interaction logic for ReceiptList.xaml
    /// </summary>
    public partial class ReceiptList : UserControl
    {
        public class ReceiptItem
        {
            public string id { get; set; }
            public DateTime time { get; set; }
            public string createrName { get; set; }
            public int total { get; set; }
            public ReceiptItem(string newID, DateTime newTime, string newCreaterName, int newTotal)
            {
                id = newID;
                time = newTime;
                createrName = newCreaterName;
                total = newTotal;
            }
        }

        DateTime start;
        DateTime end;
        string keyword;
        int limitRow;
        int currentPage;
        List<ReceiptItem> receiptItems;

        public ReceiptList()
        {
            InitializeComponent();
            limitRow = 5;
            currentPage = 1;
            tbNumPage.Text = "1";
            btnPagePre.IsEnabled = false;
            dgReceipt.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
            Loaded += LoadData;
            //LoadData();
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1 + limitRow * (currentPage - 1);
            e.Row.Height = 40;
        }

        void LoadData(Object sender, RoutedEventArgs e)
        {
            start = new DateTime(2021, 1, 1, 0, 0, 0);
            end = DateTime.Today;
            keyword = "";

            BUS_Receipt busReceipt = new BUS_Receipt();
            int empCount = busReceipt.CountReceipt(start, end, keyword);
            if (empCount % limitRow == 0)
                lblMaxPage.Content = empCount / limitRow;
            else
                lblMaxPage.Content = empCount / limitRow + 1;
            if (currentPage == (int)lblMaxPage.Content)
                btnPageNext.IsEnabled = false;
            else
                btnPageNext.IsEnabled = true;

            ReloadDGReceipt();
        }
        
        private void ReloadDGReceipt()
        {
            receiptItems = new List<ReceiptItem>();
            BUS_Receipt busReceipt = new BUS_Receipt();
            DataTable receipts = busReceipt.GetReceipts(start, end, keyword, limitRow, limitRow * (currentPage - 1));
            foreach (DataRow row in receipts.Rows)
            {
                string id = row["ReceiptID"].ToString();
                DateTime time = TimeZone.CurrentTimeZone.ToLocalTime((DateTime)row["Time"]);
                string creater = row["EmployeeName"].ToString();
                float dis = 0;
                try
                {
                    dis = float.Parse(row["DiscountValue"].ToString());
                }
                catch
                {

                }
                int total = (int)(Int32.Parse(row["Total"].ToString()) * (1 - dis / 100));
                receiptItems.Add(new ReceiptItem(id, time, creater, total));
            }
            dgReceipt.ItemsSource = receiptItems;
            dgReceipt.Items.Refresh();
        }    

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            DateTime? datepicker = dpDateStart.SelectedDate;
            if (datepicker.ToString() == "")
                start = DateTime.Today;
            else
                start = datepicker.Value;

            datepicker = dpDateEnd.SelectedDate;
            if (datepicker.ToString() == "")
                end = DateTime.Today;
            else
                end = datepicker.Value;

            keyword = tbFind.Text;

            BUS_Receipt busReceipt = new BUS_Receipt();
            int empCount = busReceipt.CountReceipt(start, end, keyword);
            if (empCount % limitRow == 0)
                lblMaxPage.Content = empCount / limitRow;
            else
                lblMaxPage.Content = empCount / limitRow + 1;
            if (currentPage == (int)lblMaxPage.Content)
                btnPageNext.IsEnabled = false;
            else
                btnPageNext.IsEnabled = true;

            currentPage = 1;
            tbNumPage.Text = currentPage.ToString();

            ReloadDGReceipt();
        }

        private void btnWatch_Click(object sender, RoutedEventArgs e)
        {
            string id = ((Button)sender).Tag.ToString();
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Chi tiết hóa đơn",
                Content = new ReceiptDetail(id),
                Width = 450,
                Height = 800,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 450) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 400) / 2,
            };
            window.ShowDialog();

            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string id = ((Button)sender).Tag.ToString();

            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            ((MainWindow)App.Current.MainWindow).Opacity = 0.5;
            ((MainWindow)App.Current.MainWindow).Effect = objBlur;
            Window window = new Window
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.None,
                Title = "Xóa hóa đơn",
                Content = new PopupDeleteConfirm($"Bạn có chắc chắn muốn xóa hóa đơn {id} không?", id, 1),
                Width = 380,
                Height = 210,
                Left = (Application.Current.MainWindow.Left + Application.Current.MainWindow.Width - 380) / 2,
                Top = (Application.Current.MainWindow.Top + Application.Current.MainWindow.Height - 210) / 2,
            };
            window.ShowDialog();

            BUS_Receipt busReceipt = new BUS_Receipt();
            int empCount = busReceipt.CountReceipt(start, end, keyword);
            if (empCount % limitRow == 0)
                lblMaxPage.Content = empCount / limitRow;
            else
                lblMaxPage.Content = empCount / limitRow + 1;
            if (currentPage > (int)lblMaxPage.Content)
            {
                tbNumPage.Text = (--currentPage).ToString();
            }

            if (currentPage == (int)lblMaxPage.Content)
                btnPageNext.IsEnabled = false;
            else
                btnPageNext.IsEnabled = true;

            if (currentPage == 1)
                btnPagePre.IsEnabled = false;
            else
                btnPagePre.IsEnabled = true;

            ReloadDGReceipt();
            ((MainWindow)App.Current.MainWindow).Opacity = 1;
            ((MainWindow)App.Current.MainWindow).Effect = null;
        }

        private void btnPagePre_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                tbNumPage.Text = (--currentPage).ToString();
                btnPageNext.IsEnabled = true;
                ReloadDGReceipt();
            }
            if (currentPage == 1)
                btnPagePre.IsEnabled = false;
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

        private void tbNumPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                int newPage = Int32.Parse(tbNumPage.Text);
                if (newPage > (int)lblMaxPage.Content)
                {
                    MessageBox.Show("Không có trang này!");
                    return;
                }
                currentPage = newPage;
                if (currentPage == 1)
                    btnPagePre.IsEnabled = false;
                else
                    btnPagePre.IsEnabled = true;
                if (currentPage == (int)lblMaxPage.Content)
                    btnPageNext.IsEnabled = false;
                else
                    btnPageNext.IsEnabled = true;

                ReloadDGReceipt();
            }
        }

        private void btnPageNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < (int)lblMaxPage.Content)
            {
                tbNumPage.Text = (++currentPage).ToString();
                btnPagePre.IsEnabled = true;
                ReloadDGReceipt();
            }
            if (currentPage == (int)lblMaxPage.Content)
                btnPageNext.IsEnabled = false;
        }
    }
}
