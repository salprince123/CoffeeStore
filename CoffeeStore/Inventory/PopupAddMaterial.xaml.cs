using CoffeeStore.BUS;
using System;
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

namespace CoffeeStore.Inventory
{
    /// <summary>
    /// Interaction logic for PopupAddMaterial.xaml
    /// </summary>
    public partial class PopupAddMaterial : UserControl
    {
        public PopupAddMaterial()
        {
            InitializeComponent();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
            

        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            BUS_Material material = new BUS_Material();
            if(material.Create(tbName.Text, tbUnit.Text))
            MessageBox.Show($"Đã thêm vật liệu {tbName.Text}, đơn vị tính {tbUnit.Text}");
            else MessageBox.Show($"Vat lieu da ton tai");
            Window.GetWindow(this).Close();
        }
    }
}
