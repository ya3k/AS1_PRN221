﻿using BusinessObject.Entity;
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

namespace SalesWPFApp.UserWPF
{
    /// <summary>
    /// Interaction logic for PageOrderHistory.xaml
    /// </summary>
    public partial class PageOrderHistory : Page
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMemberRepository _memberRepository;
        Member? _member;
        public PageOrderHistory(IOrderRepository orderRepository, IMemberRepository memberRepository, Member? member)
        {
            InitializeComponent();
            _orderRepository = orderRepository;
            listView.SelectionChanged += ListView_SelectionChanged;
            _memberRepository = memberRepository;
            _member = member;
            Loaded += Page_Loaded;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = _orderRepository.FindByUserID(_member.MemberId);
        }


        private void Button_Reload(object sender, RoutedEventArgs e)
        {
            RefreshListView();
        }


         private void RefreshListView()
        {
            listView.ItemsSource = _orderRepository.FindByUserID(_member.MemberId);

        }
      

        private void Button_Edit(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Insert(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = dpOrderDate.SelectedDate == null ? null : dpOrderDate.SelectedDate.Value.Date;
            DateTime? endDate = dpShippedDate.SelectedDate == null ? null : dpShippedDate.SelectedDate.Value.Date;
            listView.ItemsSource = _orderRepository.FindAllByStartTimeAndEndTime(startDate.Value, endDate.Value);
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
                tbFreight.Text = selectedOrder.Freight?.ToString() ?? string.Empty;

                btnEdit.IsEnabled = true;
                btnClear.IsEnabled = true;
            }
            else
            {
                btnClear.IsEnabled = false;
                btnEdit.IsEnabled = false;
            }
        }

    }
}