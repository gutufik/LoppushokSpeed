using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using LoppushokSpeed.DataBase;
using Microsoft.Win32;

namespace LoppushokSpeed.Windows
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        public Product Product { get; set; }
        public List<ProductType> ProductTypes { get; set; }
        public ProductWindow(Product product)
        {
            InitializeComponent();
            Product = product;
            ProductTypes = DataAccess.GetProductTypes();
            DataContext = this;
        }

        private void btnChangeImage_Click(object sender, RoutedEventArgs e)
        {
            var fileDilaog = new OpenFileDialog();
            if (fileDilaog.ShowDialog().Value != null)
            {
                imgProduct.Source = new BitmapImage(new Uri(fileDilaog.FileName));
                Product.Image = File.ReadAllBytes(fileDilaog.FileName);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DataAccess.SaveProduct(Product);
            Close();
        }
    }
}
