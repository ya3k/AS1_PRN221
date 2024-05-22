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

namespace SalesWPFApp.UserWPF
{
    /// <summary>
    /// Interaction logic for WindowNewOrder.xaml
    /// </summary>
    public partial class WindowNewOrder : Window
    {
        PageOrderHistory _pageOrderHistory;
        private readonly IOrderRepository _orderRepository;
        private readonly IMemberRepository _memberRepository;
        private Order? _order;
        private Member? _member;
        public WindowNewOrder(PageOrderHistory pageOrderHistory, IOrderRepository orderRepository, IMemberRepository memberRepository, Order order, Member? member)
        {
            InitializeComponent();
            _pageOrderHistory = pageOrderHistory;
            _orderRepository = orderRepository;
            _memberRepository = memberRepository;
            this._order = order;
            this._member = member;
            tbMemberEmail.Text = this._member.Email;
        }

        private void Button_CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate member
                if (_member == null)
                {
                    MessageBox.Show("Please select a member.");
                    return;
                }

                // Validate order date
                if (!DateTime.TryParse(datePickerOrderDate.Text, out DateTime orderDate))
                {
                    MessageBox.Show("Please enter a valid order date.");
                    return;
                }

                // Validate required date
                if (!DateTime.TryParse(datePickerRequiredDate.Text, out DateTime requiredDate))
                {
                    MessageBox.Show("Please enter a valid required date.");
                    return;
                }

                // Validate shipped date
                if (!DateTime.TryParse(datePickerShippedDate.Text, out DateTime shippedDate))
                {
                    MessageBox.Show("Please enter a valid shipped date.");
                    return;
                }

                // Validate freight
                if (!decimal.TryParse(txtBoxFreight.Text, out decimal freight) || freight < 0)
                {
                    MessageBox.Show("Please enter a valid positive freight value.");
                    return;
                }

                if (_order == null)
                {
                    var memEmail = tbMemberEmail.Text;
                    var memid = _member.MemberId;
                    Order order = new Order
                    {
                        MemberId = memid,
                        OrderDate = orderDate,
                        RequiredDate = requiredDate,
                        ShippedDate = shippedDate,
                        Freight = freight
                    };
                    _orderRepository.Add(order);
                    MessageBox.Show("Order created successfully");
                    _pageOrderHistory.RefreshListView();
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
