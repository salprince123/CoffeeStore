﻿using System;
using System.Collections.Generic;
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
using CoffeeStore.BUS;
using CoffeeStore.DTO;
namespace CoffeeStore.Discount
{
    /// <summary>
    /// Interaction logic for PopupDiscountEdit.xaml
    /// </summary>
    public partial class PopupDiscountEdit : UserControl
    {
        string ID;
        BUS_Discount busDiscount;
        MainWindow mainWindow;
        public PopupDiscountEdit()
        {
            InitializeComponent();
            busDiscount = new BUS_Discount();
        }
        public PopupDiscountEdit(string id, string name, string startdate, string enddate, string value, MainWindow window)
        {
            InitializeComponent();
            busDiscount = new BUS_Discount();
            ID = id;
            tbName.Text = name;
            tbStartDate.SelectedDate = DateTime.ParseExact(startdate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            tbEndDate.SelectedDate = DateTime.ParseExact(enddate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            MessageBox.Show(enddate + "   " + tbEndDate.Text);
            tbPrice.Text = value;
            tbDescription.Text = "";
            this.mainWindow = window;
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (checkCondition())
            {
                DTO_Discount discount = new DTO_Discount();
                discount.DiscountID = ID;
                discount.DiscountName = tbName.Text;
                discount.DiscountValue = float.Parse(tbPrice.Text);
                discount.StartDate = tbStartDate.Text;
                discount.EndDate = tbEndDate.Text;
                if (busDiscount.editDiscount(discount) > 0)
                {
                    MessageBox.Show("Thành công");
                }
                else
                    MessageBox.Show("Thất bại");
                var screen = new DiscountList(mainWindow);
                if (screen != null)
                {
                    this.mainWindow.StackPanelMain.Children.Clear();
                    this.mainWindow.StackPanelMain.Children.Add(screen);
                }
            }
            else
                MessageBox.Show("Tên discount, giá trị discount, ngày bắt đầu và ngày kết thúc là bắt buộc");

        }
        private bool checkCondition()
        {
            return (tbName.Text != "" && tbPrice.Text != "" && tbStartDate.Text != "" && tbEndDate.Text != "" && DateTime.Parse(tbStartDate.Text) < DateTime.Parse(tbEndDate.Text));
        }

        private void tbPrice_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberCheck.IsNumber(e.Text);
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
