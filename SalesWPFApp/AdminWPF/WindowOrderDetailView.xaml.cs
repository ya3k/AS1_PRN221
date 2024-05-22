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
    /// Interaction logic for WindowOrderDetailView.xaml
    /// </summary>
    public partial class WindowOrderDetailView : Window
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductRepository _productRepository;
        Order? _order;
        public WindowOrderDetailView(IOrderDetailRepository orderDetailRepository, IProductRepository productRepository, Order order)
        {
            InitializeComponent();
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
            _order = order;
            tbHeader.Text = $"Order Detail of Order ID: {_order.OrderId}";
            listView.SelectionChanged += ListView_SelectionChanged;
            listViewProducts.SelectionChanged += ListViewProduct_SelectionChanged;
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = _orderDetailRepository.FindByOrderId(_order.OrderId);

            listViewProducts.ItemsSource = _productRepository.ListProduct();
        }

        private void RefreshListView()
        {
            listView.ItemsSource = _orderDetailRepository.FindByOrderId(_order.OrderId);
        }

        private void Button_AddProductToOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listViewProducts.SelectedItems.Count > 0)
                {
                    Product selectedProduct = (Product)listViewProducts.SelectedItem;

                    // Validate Quantity
                    if (!int.TryParse(tbPQuantity.Text, out int quantity) || quantity <= 0)
                    {
                        MessageBox.Show("Please enter a valid quantity.");
                        return;
                    }

                    // Validate Discount
                    if (!double.TryParse(tbPDiscount.Text, out double discount) || discount < 0 || discount > 1)
                    {
                        MessageBox.Show("Please enter a valid discount (between 0 and 1).");
                        return;
                    }

                    var orderDetail = _orderDetailRepository.FindByOrderIdAndProductId(_order.OrderId, selectedProduct.ProductId);

                    if (orderDetail == null)
                    {
                        OrderDetail newOrderDetail = new OrderDetail
                        {
                            OrderId = _order.OrderId,
                            ProductId = selectedProduct.ProductId,
                            UnitPrice = selectedProduct.UnitPrice,
                            Quantity = quantity,
                            Discount = discount
                        };
                        _orderDetailRepository.AddOrderDetail(newOrderDetail);
                    }
                    else
                    {
                        orderDetail.Quantity += quantity;
                        orderDetail.Discount += discount;
                        _orderDetailRepository.UpdateOrderDetail(orderDetail);
                    }

                    RefreshListView();
                }
                else
                {
                    MessageBox.Show("Please select a product to add.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView? listView = sender as ListView;
            GridView? gridView = listView != null ? listView.View as GridView : null;

            if (listView != null && gridView != null)
            {
                double width = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;

                if (width > 0)
                {
                    double column1 = 0.2;
                    double column2 = 0.2;
                    double column3 = 0.2;
                    double column4 = 0.2;
                    double column5 = 0.2;

                    gridView.Columns[0].Width = width * column1;
                    gridView.Columns[1].Width = width * column2;
                    gridView.Columns[2].Width = width * column3;
                    gridView.Columns[3].Width = width * column4;
                    gridView.Columns[4].Width = width * column5;
                }
            }
        }

        private void ListViewProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int count = listViewProducts.SelectedItems.Count;
            if (count > 0)
            {
                Product selectedProduct = (Product)listViewProducts.SelectedItem;
                tbPProductId.Text = selectedProduct.ProductId.ToString();
                tbPProductName.Text = selectedProduct.ProductName;
                //tbPCategoryId.Text = selectedProduct.CategoryId.ToString();
                //tbPByWeight.Text = selectedProduct.Weight.ToString();
                //tbPUnitsInStock.Text = selectedProduct.UnitsInStock.ToString();

                tbPUnitPrice.Text = selectedProduct.UnitPrice.ToString("F2");
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int count = listView.SelectedItems.Count;
            if (count > 0)
            {
                OrderDetail selectedOrderDetail = (OrderDetail)listView.SelectedItem;

                var product1 = _productRepository.FindById(selectedOrderDetail.ProductId);
                tbOrderId.Text = selectedOrderDetail.OrderId.ToString();
                tbProductName.Text = product1.ProductName.ToString();
                tbUnitPrice.Text = selectedOrderDetail.UnitPrice.ToString("F2");
                tbQuantity.Text = selectedOrderDetail.Quantity.ToString();
                tbDiscount.Text = selectedOrderDetail.Discount.ToString("F2");

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
            tbOrderId.Clear();
            tbProductName.Clear();
            tbUnitPrice.Clear();
            tbQuantity.Clear();
            tbDiscount.Clear();
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            int count = listView.SelectedItems.Count;
            if (count > 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Do you wan't remove seledted?", "Remove OrderDetail", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    OrderDetail selectedOrderDetail = (OrderDetail)listView.SelectedItem;
                    _orderDetailRepository.RemoveOrderDetail(selectedOrderDetail);
                    MessageBox.Show("Order Detail deleted successfully.");
                    RefreshListView();
                }
              
            }
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            int count = listView.SelectedItems.Count;
            if (count > 0)
            {
                OrderDetail selectedOrderDetail = (OrderDetail)listView.SelectedItem;

                // Validate Quantity
                if (!int.TryParse(tbQuantity.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Please enter a valid quantity.");
                    return;
                }

                // Validate Discount
                if (!double.TryParse(tbDiscount.Text, out double discount) || discount < 0 || discount > 1)
                {
                    MessageBox.Show("Please enter a valid discount (between 0 and 1).");
                    return;
                }

                selectedOrderDetail.Quantity = quantity;
                selectedOrderDetail.Discount = discount;

                _orderDetailRepository.UpdateOrderDetail(selectedOrderDetail);
                MessageBox.Show("Order Detail updated successfully.");
                RefreshListView();
            }
        }

      
        private void Button_Reload(object sender, RoutedEventArgs e)
        {
           RefreshListView();
        }
    }
}
