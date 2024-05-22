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
    /// Interaction logic for WindowOrderCreate.xaml
    /// </summary>
    public partial class WindowOrderCreate : Window
    {
        private readonly PageAdminOrderManager _pageAdminOrderManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IMemberRepository _memberRepository;
        private Order? order;

        public WindowOrderCreate(PageAdminOrderManager pageAdminOrderManager, IOrderRepository orderRepository, IMemberRepository memberRepository, Order order)
        {
            InitializeComponent();
            this._pageAdminOrderManager = pageAdminOrderManager;
            this._orderRepository = orderRepository;
            this._memberRepository = memberRepository;
            this.order = order;
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
                btnCreateOrder.Content = "Update";
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

            }

        }

        private void Button_CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate member selection
                if (string.IsNullOrEmpty(comboBoxMember.Text))
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

                // Check if creating a new order or updating an existing one
                if (order == null)
                {
                    // Create a new order
                    var memEmail = comboBoxMember.Text;
                    var mem = _memberRepository.FindByEmail(memEmail);
                    if (mem == null)
                    {
                        MessageBox.Show("Invalid member selected.");
                        return;
                    }

                    Order newOrder = new Order
                    {
                        MemberId = mem.MemberId,
                        OrderDate = orderDate,
                        RequiredDate = requiredDate,
                        ShippedDate = shippedDate,
                        Freight = freight
                    };
                    _orderRepository.Add(newOrder);
                }
                else
                {
                    // Update existing order
                    order.OrderDate = orderDate;
                    order.RequiredDate = requiredDate;
                    order.ShippedDate = shippedDate;
                    order.Freight = freight;
                    _orderRepository.Update(order);
                }

                // Refresh the list view and close the window
                _pageAdminOrderManager.RefreshListView();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
