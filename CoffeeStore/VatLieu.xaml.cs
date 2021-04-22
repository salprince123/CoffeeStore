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

namespace CoffeeStore
{
    /// <summary>
    /// Interaction logic for VatLieu.xaml
    /// </summary>
    public partial class VatLieu : UserControl
    {
        public class VatLieuObject
        {
            public String maVL { get; set; }
            public String tenVL { get; set; }
            public String donViTinh { get; set; }
        }

        public VatLieu()
        {
            InitializeComponent();
            //this.DataContext = this;
            var list = new ObservableCollection<VatLieuObject>();
            BUS_VatLieu vatlieu= new BUS_VatLieu();
            DataTable temp = vatlieu.selectAll();
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string ma = row["mavatlieu"].ToString();
                string ten = row["tenVatLieu"].ToString();
                string donvi= row["Donvitinh"].ToString();
                list.Add(new VatLieuObject() { maVL = ma, tenVL = ten, donViTinh = donvi });
            }
            this.dataGrid1.ItemsSource = list;
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
