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
                if (order == null)
                {
                    var memEmail = comboBoxMember.Text;
                    var memid = _memberRepository.FindByEmail(memEmail);
                    Order order = new Order
                    {
                        MemberId = memid.MemberId,
                        OrderDate = DateTime.Parse(datePickerOrderDate.Text),
                        RequiredDate = DateTime.Parse(datePickerRequiredDate.Text),
                        ShippedDate = DateTime.Parse(datePickerShippedDate.Text),
                        Freight = decimal.Parse(txtBoxFreight.Text)
                    };
                    _orderRepository.Add(order);
                }
                else
                {
                    order.OrderDate = DateTime.Parse(datePickerOrderDate.Text);
                    order.RequiredDate = DateTime.Parse(datePickerRequiredDate.Text);
                    order.ShippedDate = DateTime.Parse(datePickerShippedDate.Text);
                    order.Freight = decimal.Parse(txtBoxFreight.Text);
                    _orderRepository.Update(order);
                }
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
