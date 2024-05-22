using BusinessObject.Entity;
using DataAccess.Repository;
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
using System.Windows.Shapes;

namespace SalesWPFApp.AdminWPF
{
    /// <summary>
    /// Interaction logic for WindowProductCreate.xaml
    /// </summary>
    public partial class WindowProductCreate : Window
    {
        private readonly PageAdminProductManager _pageAdminProductManager;
        private readonly IProductRepository _productRepository;
        private Product? product;
        public WindowProductCreate(PageAdminProductManager pageAdminProductManager, Product product, IProductRepository productRepository)
        {
            InitializeComponent();
            this._pageAdminProductManager = pageAdminProductManager;
            this._productRepository = productRepository;
            this.product = product;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (product != null)
            {
                txtBoxProductName.Text = product.ProductName;
                txtBoxUnitPrice.Text = product.UnitPrice.ToString();
                txtBoxUnitsInStock.Text = product.UnitsInStock.ToString();
                txtBoxWeight.Text = product.Weight.ToString();
                txtBoxCategoryId.Text = product.CategoryId.ToString();
                txtBoxProductId.Visibility = Visibility.Visible;
                labelProductId.Visibility = Visibility.Visible;
                btnCreate.Content = "Update";
                this.Height = 550;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (product == null)
                {
                    Product product = new Product
                    {
                        ProductName = txtBoxProductName.Text,
                        CategoryId = int.Parse(txtBoxCategoryId.Text),
                        Weight = float.Parse(txtBoxWeight.Text),
                        UnitPrice = decimal.Parse(txtBoxUnitPrice.Text),
                        UnitsInStock = int.Parse(txtBoxUnitsInStock.Text)
                    };
                    _productRepository.Add(product);
                }
                else
                {
                    product.ProductName = txtBoxProductName.Text;
                    product.CategoryId = int.Parse(txtBoxCategoryId.Text);
                    product.Weight = float.Parse(txtBoxWeight.Text);
                    product.UnitPrice = decimal.Parse(txtBoxUnitPrice.Text);
                    product.UnitsInStock = int.Parse(txtBoxUnitsInStock.Text);
                    _productRepository.Update(product);
                }
                _pageAdminProductManager.RefreshListView();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
