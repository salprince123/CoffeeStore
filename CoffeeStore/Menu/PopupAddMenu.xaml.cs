using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using Microsoft.Win32;

namespace CoffeeStore.Menu
{
    /// <summary>
    /// Interaction logic for PopupAddMenu.xaml
    /// </summary>
    public partial class PopupAddMenu : UserControl
    {
        BUS_Beverage bus = new BUS_Beverage();
        MainWindow main;
        byte[] imageBytes;
        public PopupAddMenu()
        {
            InitializeComponent();
        }
        public PopupAddMenu(MainWindow window)
        {
            InitializeComponent();
            cbBeverageType.ItemsSource = bus.getBeverageType();            
            main = window;
            byte[] x = File.ReadAllBytes("cafe-latte.jpg");
            imageBytes = x;
        }
        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            tbNameValidation.Text = tbTypeValidation.Text = tbPriceValidation.Text = "";
            if (tbName.Text == "")
            {
                tbNameValidation.Text = "Tên món không được để trống.";
                return;
            }

            if (cbBeverageType.Text == "")
            {
                tbTypeValidation.Text = "Loại món không được để trống.";
                return;
            }

            if (tbPrice.Text == "")
            {
                tbPriceValidation.Text = "Giá không được để trống.";
                return;
            }

            DTO_Beverage beverage = new DTO_Beverage();
            beverage.BeverageID = bus.createID();
            beverage.BeverageName = tbName.Text;
            beverage.BeverageTypeID = bus.getBeverageTypeID(cbBeverageType.Text);
            beverage.Price = Int32.Parse(tbPrice.Text);
            beverage.Link = imageBytes;
            if (bus.createNewBevverage(beverage) > 0)
            {
                MessageBox.Show($"Đã thêm {tbName.Text} vào menu");
                Window.GetWindow(this).Close();
            }
            else
                MessageBox.Show($"Đã có lỗi trong quá trình tạo {tbName.Text}");
        }

        private void tbPrice_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumberCheck.IsNumber(e.Text);
        }

        private void btExit_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void btLoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";            
            if (op.ShowDialog() == true)
            {
                byte[] x = File.ReadAllBytes(op.FileName);
                image.Source = ToImageSource(ConvertByteArrayToImage(x));
                imageBytes = x;
                //image.Source = new BitmapImage(new Uri(op.FileName));
                //selectedImageLink = op.FileName;
            }
        }

        public ImageFormat GetImageFormat(System.Drawing.Image img)
        {
            if (img.RawFormat.Equals(ImageFormat.Jpeg))
                return ImageFormat.Jpeg;
            if (img.RawFormat.Equals(ImageFormat.Bmp))
                return ImageFormat.Bmp;
            if (img.RawFormat.Equals(ImageFormat.Png))
                return ImageFormat.Png;
            if (img.RawFormat.Equals(ImageFormat.Emf))
                return ImageFormat.Emf;
            if (img.RawFormat.Equals(ImageFormat.Exif))
                return ImageFormat.Exif;
            if (img.RawFormat.Equals(ImageFormat.Gif))
                return ImageFormat.Gif;
            if (img.RawFormat.Equals(ImageFormat.Icon))
                return ImageFormat.Icon;
            if (img.RawFormat.Equals(ImageFormat.MemoryBmp))
                return ImageFormat.MemoryBmp;
            if (img.RawFormat.Equals(ImageFormat.Tiff))
                return ImageFormat.Tiff;
            else
                return ImageFormat.Wmf;
        }

        public ImageSource ToImageSource(System.Drawing.Image image)
        {
            BitmapImage bitmap = new BitmapImage();

            using (MemoryStream stream = new MemoryStream())
            {
                // Save to the stream
                image.Save(stream, GetImageFormat(image));

                // Rewind the stream
                stream.Seek(0, SeekOrigin.Begin);

                // Tell the WPF BitmapImage to use this stream
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
            }

            return bitmap;
        }

        public System.Drawing.Image ConvertByteArrayToImage(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            return System.Drawing.Image.FromStream(stream);
        }
    }
}
