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
using LoppushokSpeed.DataBase;
using System.IO;

namespace LoppushokSpeed.Pages
{
    /// <summary>
    /// Interaction logic for ProductListPage.xaml
    /// </summary>
    public partial class ProductListPage : Page
    {
        public List<Product> Products { get; set; }
        public List<ProductType> ProductTypes { get; set; }
        public List<Product> ProductsForFilter { get; set; }
        public Dictionary<string, Func<Product, object>> Sortings { get; set; }

        private int page = 1;
        public ProductListPage()
        {
            InitializeComponent();
            Products = DataAccess.GetProducts();
            ProductTypes = DataAccess.GetProductTypes();
            ProductTypes.Insert(0, new ProductType { Name = "Все типы" });

            Sortings = new Dictionary<string, Func<Product, object>>
            {
                {"Наименование по убыванию", x => x.Name },
                {"Наименование по возрастанию", x => x.Name },
                {"Номер цеха по убыванию", x => x.WorkshopNumber },
                {"Номер цеха по возрастанию", x => x.WorkshopNumber },
                {"Стоимость по убыванию", x => x.MinPriceForAgent },
                {"Стоимость по возрастанию", x => x.MinPriceForAgent },
            };
            foreach (var product in Products)
            {
                if (product.ImagePath != null)
                    product.Image = File.ReadAllBytes(@"C:\Users\201913\Desktop"+ product.ImagePath);
                DataAccess.SaveProduct(product);
            }
            DataAccess.RefreshList += DataAccess_RefreshList;


            DataContext = this;
        }

        private void DataAccess_RefreshList()
        {
            Products = DataAccess.GetProducts();
            lvProducts.ItemsSource = Products;
            lvProducts.Items.Refresh();
            page = 1;
            Filter();
        }

        private void SetPageNumbers()
        {
            var pageCount = ProductsForFilter.Count() % 20 == 0? ProductsForFilter.Count() / 20: ProductsForFilter.Count() / 20 + 1;
            spPageNumbers.Children.Clear();

            spPageNumbers.Children.Add(new TextBlock { Text = $"<", });
            for (int i = 0; i< pageCount; i++)
            {
                spPageNumbers.Children.Add(new TextBlock { Text = $"{i + 1}",  });
            }
            spPageNumbers.Children.Add(new TextBlock { Text = $">", });
            foreach (var child in spPageNumbers.Children)
            {
                (child as UIElement).MouseDown += PageNumberClick;
            }
        }

        private void PageNumberClick(object sender, MouseButtonEventArgs e)
        {
            var text = (sender as TextBlock).Text;
            var pageCount = ProductsForFilter.Count() % 20 == 0 ? ProductsForFilter.Count() / 20 : ProductsForFilter.Count() / 20 + 1;
            if (text == "<")
            {
                if(page > 1)
                    page--;
            }
            else if (text == ">")
            {
                if(page < pageCount)
                    page++;
            }
            else
                int.TryParse(text, out page);
            Filter();
        }

        private void Filter()
        {
            var searchText = tbSearch.Text.ToLower();
            var sorting = cbSort.SelectedItem as string;
            var productType = cbFilter.SelectedItem as ProductType;

            ProductsForFilter = Products.FindAll(x => x.Name.ToLower().Contains(searchText));
            if (productType != null && productType.Name != "Все типы")
            {
                ProductsForFilter = ProductsForFilter.FindAll(x => x.ProductType == productType);
            }
            if (sorting != null)
            {
                ProductsForFilter = ProductsForFilter.OrderBy(Sortings[sorting]).ToList();
                if (sorting.Contains("убыванию"))
                    ProductsForFilter.Reverse();
            }

            lvProducts.ItemsSource = ProductsForFilter.Skip((page - 1) * 20).Take(20).ToList();
            lvProducts.Items.Refresh();

            SetPageNumbers();
        }


        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            (new Windows.ProductWindow(new Product())).ShowDialog();
        }

        private void lvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDeleteProduct.IsEnabled = (lvProducts.SelectedItem as Product) != null;
        }

        private void lvProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var product = lvProducts.SelectedItem as Product;
            if (product != null)
                (new Windows.ProductWindow(product)).ShowDialog();
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var product = lvProducts.SelectedItem as Product;
            if (MessageBox.Show("","Удалить продукт?",MessageBoxButton.OKCancel) == MessageBoxResult.OK )
            {
                DataAccess.DeleteProduct(product);
            }
        }
    }
}
