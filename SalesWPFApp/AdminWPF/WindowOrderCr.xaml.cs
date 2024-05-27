using BusinessObject.Entity;
using DataAccess.Repository;
using Microsoft.IdentityModel.Tokens;
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
    /// Interaction logic for WindowOrderCr.xaml
    /// </summary>
    public partial class WindowOrderCr : Window
    {
        private readonly PageAdminOrderManager _pageAdminOrderManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        private readonly IMemberRepository _memberRepository;
        private Order? order;
        private IEnumerable<Product>? _products;
        public WindowOrderCr(PageAdminOrderManager pageAdminOrderManager, IOrderRepository orderRepository, IProductRepository productRepository,IOrderDetailRepository orderDetailRepository, IMemberRepository memberRepository, Order order, IEnumerable<Product>? products)
        {
            InitializeComponent();
            _pageAdminOrderManager = pageAdminOrderManager;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
            _memberRepository = memberRepository;
            this.order = order;
            _products = products;
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (order != null)
            {
                txtBoxOrderId.Text = order.OrderId.ToString();
                datePickerOrderDate.SelectedDate = order.OrderDate;
                datePickerRequiredDate.SelectedDate = order.RequiredDate;
                datePickerShippedDate.SelectedDate = order.ShippedDate;
                txtBoxFreight.Text = order.Freight.ToString();
                tbMemberEmail.Text = order.Member.Email.ToString();

                //combobox
                comboBoxMember.Visibility = Visibility.Collapsed;
                comboBoxMember.IsEnabled = false;

                //lb
                lbMemberEmail.Visibility = Visibility.Visible;
                tbMemberEmail.Visibility = Visibility.Visible;

                txtBoxOrderId.Visibility = Visibility.Visible;
                labelOrderId.Visibility = Visibility.Visible;
                btn_Create.Content = "Update";
            }
            else if (order == null)
            {
                comboBoxMember.ItemsSource = _memberRepository.GetAllMembers();
                comboBoxMember.DisplayMemberPath = "Email";
                comboBoxMember.SelectedValuePath = "MemberId";

                datePickerOrderDate.SelectedDate = DateTime.Now;
                datePickerRequiredDate.SelectedDate = DateTime.Now;
                datePickerShippedDate.SelectedDate = DateTime.Now;
                txtBoxFreight.Text = null;

                lbMemberEmail.Visibility = Visibility.Visible;
                RefreshListProduct();
            }

        }

        private void Button_NewOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                var memID = (Member)comboBoxMember.SelectedItem;
                if (memID == null)
                {
                    MessageBox.Show("Invalid member selected.");
                    return;
                }
                Order o = new Order
                {
                    MemberId =memID.MemberId,
                    Freight = 10,
                    OrderDate = DateTime.Now,
                    RequiredDate = DateTime.Now.AddDays(5),
                    ShippedDate = DateTime.Now.AddDays(1),
                    OrderDetails = new List<OrderDetail>()
                };
                foreach (Product product in productList.Items)
                {
                    if (product.Quantity > 0)
                    {
                        OrderDetail oD = new OrderDetail
                        {
                            ProductId = product.ProductId,
                            Discount = 0,
                            Quantity = product.Quantity,
                            UnitPrice = product.UnitPrice
                        };
                        o.OrderDetails.Add(oD);
                    }
                }
                _orderRepository.Add(o);
                RefreshListProduct();
                RefreshQuantity();
                MessageBox.Show("Order Success!!!", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Order failed: {ex.Message}", "Error");
            }
        }

        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            RefreshQuantity();
        }

        private void productList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            Product product = button.DataContext as Product;
            if (product != null && product.Quantity > 0)
            {
                product.Quantity--;
                productList.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Not enough product");
            }
        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            Product product = button.DataContext as Product;
            if (product != null && product.Quantity <= product.UnitsInStock)
            {
                product.Quantity++;
                productList.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Not enough product");
            }
        }


        private void RefreshListProduct()
        {
            if(_products == null)
            {
                productList.ItemsSource = _productRepository.ListProduct();

            }
            
        }

        private void RefreshQuantity()
        {
            foreach(Product p in productList.Items.OfType<Product>())
            {
                p.Quantity = 0;
            }
            productList.Items.Refresh();    

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           
            var search = searchBox.Text;
            if (search.IsNullOrEmpty())
            {
                productList.ItemsSource = _productRepository.ListProduct();
            }
            else
            {
                IEnumerable<Product> products = _productRepository.FindByName(search);
                productList.ItemsSource = products;
            }
               
        }
    }
}
