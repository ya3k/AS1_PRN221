using BusinessObject.Entity;
using DataAccess.Repository;
using SalesWPFApp.CartWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static System.Collections.Specialized.BitVector32;

namespace SalesWPFApp.UserWPF
{
    /// <summary>
    /// Interaction logic for WindowStore.xaml
    /// </summary>
    public partial class WindowStore : Window
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly WindowLogin windowLogin;
        public WindowStore(WindowLogin windowLogin, IProductRepository productRepository, IMemberRepository memberRepository, IOrderRepository orderRepository)
        {
            InitializeComponent();
            Closing += Home_Closing;
            this.windowLogin = windowLogin;
            _productRepository = productRepository;
            _memberRepository = memberRepository;
            _orderRepository = orderRepository;
            ListProduct.ItemsSource = _productRepository.ListProduct();
            Cart.carts = new List<OrderDetail>();
            UpdateCartQuantity();
        }

        private void Home_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            windowLogin.Show();
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            int? id = null;
            string name = tbProductName.Text;
            decimal? unitPrice = null;
            int? unitsInStock = null;

            // Validate id
            if (!string.IsNullOrEmpty(tbProdcutID.Text))
            {
                if (int.TryParse(tbProdcutID.Text, out int parsedId))
                {
                    id = parsedId;
                }
                else
                {
                    MessageBox.Show("Please enter a valid numeric value for ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Validate unitPrice
            if (!string.IsNullOrEmpty(tbUnitPrice.Text))
            {
                if (decimal.TryParse(tbUnitPrice.Text, out decimal parsedUnitPrice))
                {
                    unitPrice = parsedUnitPrice;
                }
                else
                {
                    MessageBox.Show("Please enter a valid numeric value for Unit Price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Validate unitsInStock
            if (!string.IsNullOrEmpty(tbUnitsInStock.Text))
            {
                if (int.TryParse(tbUnitsInStock.Text, out int parsedUnitsInStock))
                {
                    unitsInStock = parsedUnitsInStock;
                }
                else
                {
                    MessageBox.Show("Please enter a valid numeric value for Units In Stock.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Perform the search
            ListProduct.ItemsSource = _productRepository.MultiSearch(id, name, unitPrice, unitsInStock);
        }


        private void Button_OpenCart(object sender, RoutedEventArgs e)
        {
            WindowCart windowCart = new WindowCart(this, _memberRepository, _orderRepository);
            windowCart.Show();
            this.Hide();
        }

        private void AddToCart(object sender, RoutedEventArgs e)
        {
            Button? button = (Button)sender;
            if(button != null)
            {
                  var tag = button.Tag;
                if(!string.IsNullOrEmpty(tag.ToString()))
                {
                    int id = int.Parse(tag.ToString());
                    Product product = _productRepository.FindById(id);
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        ProductId = product.ProductId,
                        UnitPrice = product.UnitPrice,
                        Quantity = 1,
                        Product = product,
                        Discount = 0
                    };
                    if (Cart.carts == null)
                    {
                        Cart.carts = new List<OrderDetail>{orderDetail};   
                    }
                    else
                    {
                        int index = Cart.carts.FindIndex(x => x.ProductId == id);
                        if (index == -1)
                        {
                            Cart.carts.Add(orderDetail);
                        }
                        else
                        {
                            Cart.carts[index].Quantity++;
                        }
                    }
                }
            }

            UpdateCartQuantity();
        }

        public void UpdateCartQuantity()
        {
            CartCount.Text = Cart.carts.Sum(product => product.Quantity).ToString();
        }

        private void Button_OpenMyOrder(object sender, RoutedEventArgs e)
        {
          
        }
    }
}
