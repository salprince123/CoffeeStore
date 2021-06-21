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
    /// Interaction logic for PopupEditMaterial.xaml
    /// </summary>
    public partial class PopupEditMaterial : UserControl
    {
        String name;
        public class MaterialObject
        {
            public String Name { get; set; }
            public String Unit { get; set; }
        }
        public PopupEditMaterial()
        {
            InitializeComponent();
        }
        public PopupEditMaterial(String oldName, String unit)
        {
           
            InitializeComponent();
            MaterialObject temp = new MaterialObject();
            temp.Name = oldName;
            temp.Unit = unit;
            this.name= oldName;
            this.DataContext = temp;
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            BUS_Material material = new BUS_Material();
            bool result = material.Update(this.name,tbName.Text, tbUnit.Text);
            if (result) Window.GetWindow(this).Close(); 
            //MessageBox.Show($"Đã lưu chỉnh sửa ");
            else MessageBox.Show($"Chỉnh sửa không thành công");
            Window.GetWindow(this).Close();
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
