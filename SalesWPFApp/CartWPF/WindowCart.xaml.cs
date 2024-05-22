using BusinessObject.Entity;
using DataAccess.Repository;
using SalesWPFApp.UserWPF;
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
using static System.Collections.Specialized.BitVector32;

namespace SalesWPFApp.CartWPF
{
    /// <summary>
    /// Interaction logic for WindowCart.xaml
    /// </summary>
    public partial class WindowCart : Window
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly WindowStore windowStore;  
        public WindowCart(WindowStore windowStore, IMemberRepository memberRepository, IOrderRepository orderRepository)

        {

            InitializeComponent();
            this.windowStore = windowStore;
            _memberRepository = memberRepository;
           _orderRepository = orderRepository;
            listView.ItemsSource = Cart.carts.ToList();
            if(Cart.carts == null)
            {
                Cart.carts = new List<OrderDetail>();
            }
            updateCarts();
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView? listView = sender as ListView;
            GridView? gridView = listView != null ? listView.View as GridView : null;

            var width = listView != null ? listView.ActualWidth - SystemParameters.VerticalScrollBarWidth : this.Width;

            var column1 = 0.2;
            var column2 = 0.2;
            var column3 = 0.2;
            var column4 = 0.1;
            var column5 = 0.2;
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

        private void updateCarts()
        {
            listView.ItemsSource = Cart.carts.ToList();
            TxtBoxTotalPrice.Text = Cart.carts.Sum(cart => (cart.Quantity * cart.UnitPrice) - (cart.Quantity * cart.UnitPrice * (decimal)cart.Discount)).ToString();
            windowStore.UpdateCartQuantity();
        }

        private void Button_Plus(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                var tag = button.Tag;
                if (!string.IsNullOrEmpty(tag.ToString()))
                {
                    int id = int.Parse(tag.ToString());
                    int index = Cart.carts.FindIndex(cart => cart.ProductId == id);
                    if (index != -1)
                    {
                        Cart.carts[index].Quantity += 1;
                        updateCarts();
                    }
                }
            }
        }

        private void Button_Minus(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                var tag = button.Tag;
                if (!string.IsNullOrEmpty(tag.ToString()))
                {
                    int id = int.Parse(tag.ToString());
                    int index = Cart.carts.FindIndex(cart => cart.ProductId == id);
                    if (index != -1)
                    {
                        Cart.carts[index].Quantity -= 1;
                        if (Cart.carts[index].Quantity <= 0)
                        {
                            Cart.carts.RemoveAt(index);
                        }
                        updateCarts();
                    }
                }
            }
        }

        private void Button_Remove(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                var tag = button.Tag;
                if (!string.IsNullOrEmpty(tag.ToString()))
                {
                    int id = int.Parse(tag.ToString());
                    int index = Cart.carts.FindIndex(cart => cart.ProductId == id);
                    if (index != -1)
                    {
                        Cart.carts.RemoveAt(index);
                        updateCarts();
                    }
                }
            }
        }

        private void Button_Checkout(object sender, RoutedEventArgs e)
        {
            DateTime? requiredDate = RequiredDate.SelectedDate == null ? null : RequiredDate.SelectedDate.Value.Date;
            DateTime? shippedDate = ShippedDate.SelectedDate == null ? null : ShippedDate.SelectedDate.Value.Date;
            Member member = _memberRepository.FindByEmail(Cart.Email);
            if (member == null)
            {
                MessageBox.Show("Not found user");
                this.Close();
                windowStore.Close();
                return;
            }
            List<OrderDetail> orderDetails = Cart.carts.Select(cart =>
            {
                cart.Product = null;
                return cart;
            }).ToList();
            Order order = new Order();
            order.OrderDate = DateTime.Now;
            order.OrderDetails = orderDetails;
            order.MemberId = member.MemberId;
            order.Freight = Cart.Freight;
            order.RequiredDate = requiredDate;
            order.ShippedDate = shippedDate;
            //_orderRepository.Add(order);
            Cart.carts = new List<OrderDetail>();
            updateCarts();
        }


    }
}
