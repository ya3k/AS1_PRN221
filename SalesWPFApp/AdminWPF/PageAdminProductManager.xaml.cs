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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SalesWPFApp.AdminWPF
{
    /// <summary>
    /// Interaction logic for PageAdminProductManager.xaml
    /// </summary>
    public partial class PageAdminProductManager : Page
    {
        private readonly IProductRepository _productRepository;

        public PageAdminProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            InitializeComponent();

            listView.SelectionChanged += ListView_SelectionChanged;


        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = _productRepository.ListProduct();
        }


        public void RefreshListView()
        {
            listView.ItemsSource = _productRepository.ListProduct();
        }

        private void Button_Reload(object sender, RoutedEventArgs e)
        {
            RefreshListView();
        }


        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Do you wan't remove product seledted?", "Remove product", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    //mutiple select deletes
                    List<Product> products = listView.SelectedItems.Cast<Product>().ToList();
                    products.ForEach(product => _productRepository.Delete(product));
                    listView.ItemsSource = products;
                    RefreshListView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            try
            {
                int count = listView.SelectedItems.Count;
                if (count > 0)
                {

                    //mutiple select
                    List<Product> selectedProducts = listView.SelectedItems.Cast<Product>().ToList();
                    foreach (Product product in selectedProducts)
                    {
                        WindowProductCreate adminProductUpdate = new WindowProductCreate(this, product, _productRepository);
                        adminProductUpdate.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a product to edit");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to edit the member: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Insert(object sender, RoutedEventArgs e)
        {
            WindowProductCreate windowProductCreate = new WindowProductCreate(this, null, _productRepository);
            windowProductCreate.ShowDialog();
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            int? id = !String.IsNullOrEmpty(tbProductId.Text) ? int.Parse(tbProductId.Text) : null;
            string? name = tbProductName.Text;
            decimal? unitPrice = !String.IsNullOrEmpty(tbUnitPrice.Text) ? decimal.Parse(tbUnitPrice.Text) : null;
            int? unitsInStock = !String.IsNullOrEmpty(tbUnitsInStock.Text) ? int.Parse(tbUnitsInStock.Text) : null;

            listView.ItemsSource = _productRepository.MultiSearch(id, name, unitPrice, unitsInStock);
        }


        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView? listView = sender as ListView;
            GridView? gridView = listView != null ? listView.View as GridView : null;

            var width = listView != null ? listView.ActualWidth - SystemParameters.VerticalScrollBarWidth : this.Width;

            var column1 = 0.1;
            var column2 = 0.2;
            var column3 = 0.2;
            var column4 = 0.2;
            var column5 = 0.1;
            var column6 = 0.2;

            if (gridView != null && width >= 0)
            {
                gridView.Columns[0].Width = width * column1;
                gridView.Columns[1].Width = width * column2;
                gridView.Columns[2].Width = width * column3;
                gridView.Columns[3].Width = width * column4;
                gridView.Columns[4].Width = width * column5;
                gridView.Columns[5].Width = width * column6;
            }
        }   


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int count = listView.SelectedItems.Count;
            if (count > 0)
            {
                Product selectProduct = (Product)listView.SelectedItem;
                // Set the TextBox values with the selected member's properties
                tbProductId.Text = selectProduct.ProductId.ToString();
                tbProductName.Text = selectProduct.ProductName.ToString();
                tbCategoryId.Text = selectProduct.CategoryId.ToString();
                tbUnitPrice.Text = selectProduct.UnitPrice.ToString();
                tbByWeight.Text = selectProduct.Weight.ToString();
                tbUnitsInStock.Text = selectProduct.UnitsInStock.ToString();


                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnClear.IsEnabled = true;
            }
            else
            {
                btnClear.IsEnabled = false;
                btnEdit.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }

        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            tbProductId.Text = null;
            tbProductName.Text = null;
            tbCategoryId.Text = null;
            tbUnitPrice.Text = null;
            tbByWeight.Text = null;
            tbUnitsInStock.Text = null;

            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnClear.IsEnabled = false;

        }
    }
}
