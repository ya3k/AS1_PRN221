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
    /// Interaction logic for PageReportSales.xaml
    /// </summary>
    public partial class PageReportSales : Page
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        public PageReportSales(IProductRepository productRepository, IOrderDetailRepository orderDetailRepository)
        {
            InitializeComponent();
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            listOrderDetail.ItemsSource = _orderDetailRepository.ReportSales();    
        }


        public void RefreshListView()
        {
            listOrderDetail.ItemsSource = _orderDetailRepository.ReportSales();

        }

        private void Button_Reload(object sender, RoutedEventArgs e)
        {
            RefreshListView();
        }



        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is ListView listView && listView.View is GridView gridView)
            {
                var width = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;

                var columnWidths = new double[] { 0.1, 0.2, 0.2, 0.2, 0.1, 0.2 };
                for (int i = 0; i < gridView.Columns.Count; i++)
                {
                    gridView.Columns[i].Width = width * columnWidths[i];
                }
            }
        }


        //private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    int count = listView.SelectedItems.Count;
        //    if (count > 0)
        //    {
        //        Product selectProduct = (Product)listView.SelectedItem;
        //        // Set the TextBox values with the selected member's properties
        //        tbProductId.Text = selectProduct.ProductId.ToString();
        //        tbProductName.Text = selectProduct.ProductName.ToString();
        //        tbCategoryId.Text = selectProduct.CategoryId.ToString();
        //        tbUnitPrice.Text = selectProduct.UnitPrice.ToString();
        //        tbByWeight.Text = selectProduct.Weight.ToString();
        //        tbUnitsInStock.Text = selectProduct.UnitsInStock.ToString();


               
        //        btnClear.IsEnabled = true;
        //    }
        //    else
        //    {
        //        btnClear.IsEnabled = false;
               
        //    }
        //}

        //private void Button_Clear(object sender, RoutedEventArgs e)
        //{
        //    tbProductId.Text = null;
        //    tbProductName.Text = null;
        //    tbCategoryId.Text = null;
        //    tbUnitPrice.Text = null;
        //    tbByWeight.Text = null;
        //    tbUnitsInStock.Text = null;

          
        //    btnClear.IsEnabled = false;

        //}

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            var startDate = dpStartDate.SelectedDate;
            var endDate = dpEndDate.SelectedDate;
            try
            {
                if (startDate == null || endDate == null)
                {
                    MessageBox.Show("Please select start date and end date");
                    return;
                }
                if (startDate > endDate)
                {
                    MessageBox.Show("Start date must be less than end date");
                    return;
                }
                listOrderDetail.ItemsSource = _orderDetailRepository.FindByDate(startDate.Value, endDate.Value);

            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            dpEndDate.SelectedDate = null;
            dpStartDate.SelectedDate = null;
        }
    }

}
