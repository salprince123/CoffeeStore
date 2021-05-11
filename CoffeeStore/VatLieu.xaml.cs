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
        public class MaterialObject
        {
            public String ID { get; set; }
            public String Name { get; set; }
            public String Unit { get; set; }
        }

        public VatLieu()
        {
            InitializeComponent();
            //this.DataContext = this;
            this.LoadData();
        }
        public void LoadData()
        {
            var list = new ObservableCollection<MaterialObject>();
            BUS_Material material = new BUS_Material();
            DataTable temp = material.selectAll();
            Console.WriteLine(temp.Rows.Count);
            foreach (DataRow row in temp.Rows)
            {
                string id = row["MaterialID"].ToString();
                string name = row["MaterialName"].ToString();
                string unit = row["Unit"].ToString();
                list.Add(new MaterialObject() { ID = id, Name = name, Unit = unit });
            }
            this.dataGrid1.ItemsSource = list;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            BUS_Material material = new BUS_Material();
            material.Create(tbMaterialName.Text, tbUnit.Text);
            this.LoadData();
            MessageBox.Show($"Đã thêm vật liệu {tbMaterialName.Text}, đơn vị tính {tbUnit.Text}");
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            BUS_Material material = new BUS_Material();
            bool result =material.Delete(tbMaterialID.Text);
            if(result)
                MessageBox.Show($"Đã xóa thành công vật liệu {tbMaterialID.Text}");
            else MessageBox.Show($"Xóa không thành công");
            this.LoadData();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            
            BUS_Material material = new BUS_Material();
            bool result = material.Update(tbMaterialName.Text, tbUnit.Text);
            if (result)
                MessageBox.Show($"Đã lưu chỉnh sửa ");
            else MessageBox.Show($"Chỉnh sửa không thành công");
            this.LoadData();
        }
        private void dataGrid1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MaterialObject row = (MaterialObject)dataGrid1.SelectedItem;
            tbMaterialID.Text = row.ID;
            tbMaterialName.Text = row.Name;
            tbUnit.Text = row.Unit;
        }
    }
}
