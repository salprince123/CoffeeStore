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

namespace CoffeeStore.IncomeAndPayment
{
    /// <summary>
    /// Interaction logic for ReceiptDetail.xaml
    /// </summary>
    public partial class ReceiptDetail : UserControl
    {
        public ReceiptDetail()
        {
            InitializeComponent();
            dataGridReceiptDetail.LoadingRow += new EventHandler<DataGridRowEventArgs>(datagrid_LoadingRow);
        }
        void datagrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
            e.Row.Height = 40;
        }
    }
}
