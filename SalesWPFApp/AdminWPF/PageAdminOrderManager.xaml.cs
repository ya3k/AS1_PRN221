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
    /// Interaction logic for PageAdminOrderManager.xaml
    /// </summary>
    public partial class PageAdminOrderManager : Page
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        public PageAdminOrderManager(IOrderRepository orderRepository, IMemberRepository memberRepository, IProductRepository productRepository, IOrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            InitializeComponent();
            listView.SelectionChanged += ListView_SelectionChanged;
            _memberRepository = memberRepository;
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = _orderRepository.List();
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime? startDate = dpOrderDate.SelectedDate;
                DateTime? endDate = dpShippedDate.SelectedDate;

                // Check if both start date and end date are selected
                if (startDate == null || endDate == null)
                {
                    MessageBox.Show("Please select both start date and end date.");
                    return;
                }

                // Check if start date is before end date
                if (startDate > endDate)
                {
                    MessageBox.Show("Start date cannot be after end date.");
                    return;
                }

                // Retrieve orders within the specified date range
                listView.ItemsSource = _orderRepository.FindAllByStartTimeAndEndTime(startDate.Value, endDate.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }


        public void RefreshListView()
        {
            listView.ItemsSource = _orderRepository.List();
        }

        private void Button_Reload(object sender, RoutedEventArgs e)
        {
            RefreshListView();
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Do you wan't remove order seledted?", "Remove order", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    List<Order> orders = listView.SelectedItems.Cast<Order>().ToList();
                    foreach (Order order in orders)
                    {
                        _orderRepository.Remove(order);
                    }
                    listView.ItemsSource = orders;
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
                    List<Order> selectedOrders = listView.SelectedItems.Cast<Order>().ToList();
                    foreach (Order order in selectedOrders)
                    {
                        WindowOrderCreate windowOrderCreate = new WindowOrderCreate(this, _orderRepository, _memberRepository, order);
                        windowOrderCreate.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Please select order to edit");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Insert(object sender, RoutedEventArgs e)
        {
            WindowOrderCreate windowOrderCreate = new WindowOrderCreate(this, _orderRepository, _memberRepository, null);
            windowOrderCreate.ShowDialog();
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
                    double column1 = 0.1;
                    double column2 = 0.2;
                    double column3 = 0.2;
                    double column4 = 0.2;
                    double column5 = 0.1;
                    double column6 = 0.2;

                    gridView.Columns[0].Width = width * column1;
                    gridView.Columns[1].Width = width * column2;
                    gridView.Columns[2].Width = width * column3;
                    gridView.Columns[3].Width = width * column4;
                    gridView.Columns[4].Width = width * column5;
                    gridView.Columns[5].Width = width * column6;
                }
            }
        }


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int count = listView.SelectedItems.Count;
            if (count > 0)
            {
                Order selectedOrder = (Order)listView.SelectedItem;
                // Set the TextBox values with the selected order's properties
                tbOrderId.Text = selectedOrder.OrderId.ToString();
                tbMemberId.Text = selectedOrder.MemberId.ToString();
                dpOrderDate.SelectedDate = selectedOrder.OrderDate;
                dpRequiredDate.SelectedDate = selectedOrder.RequiredDate;
                dpShippedDate.SelectedDate = selectedOrder.ShippedDate;
                tbFreight.Text = selectedOrder.Freight?.ToString("F2") ?? string.Empty;

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
            tbMemberId.Clear();
            dpOrderDate.SelectedDate = null;
            dpRequiredDate.SelectedDate = null;
            dpShippedDate.SelectedDate = null;
            tbFreight.Clear();
        }

        private void Button_ViewDetail(object sender, RoutedEventArgs e)
        {
            int count = listView.SelectedItems.Count;
            if (count > 0)
            {
                //mutiple select
                List<Order> selectedOrders = listView.SelectedItems.Cast<Order>().ToList();

                foreach (Order order in selectedOrders)
                {
                    WindowOrderDetailView windowOrderDetailView = new WindowOrderDetailView(_orderDetailRepository,_productRepository, order);
                    windowOrderDetailView.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Please select order to view detail");
            }

         

        }
    }
}
