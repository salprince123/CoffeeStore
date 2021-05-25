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
    public partial class PopupDiscountAdd : UserControl
    {
        BUS_Discount busDiscount;
        public PopupDiscountAdd()
        {
            InitializeComponent();
            busDiscount = new BUS_Discount();
        }
        
        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (checkCondition())
            {
                DTO_Discount discount = new DTO_Discount();
                discount.DiscountID = busDiscount.ID();
                discount.DiscountName = tbName.Text;
                discount.DiscountValue = float.Parse(tbPrice.Text);
                discount.StartDate = dpStartDate.SelectedDate.Value.ToString("dd/MM/yyyy");
                discount.EndDate = dpEndDate.SelectedDate.Value.ToString("dd/MM/yyyy");
                if (busDiscount.createNewDiscount(discount) > 0)
                {
                    MessageBox.Show("Thành công");
                }
                else
                    MessageBox.Show("Thất bại" + busDiscount.ID());
            }
            else
                MessageBox.Show("Tên discount, giá trị discount, ngày bắt đầu và ngày kết thúc là bắt buộc");

        }
        private bool checkCondition()
        {
            return (tbName.Text != "" && tbPrice.Text != "" && dpStartDate.SelectedDate != null && dpEndDate.SelectedDate != null && dpStartDate.SelectedDate < dpEndDate.SelectedDate);
        }

        private void tbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberCheck.IsNumber(e.Text);
        }
    }
}