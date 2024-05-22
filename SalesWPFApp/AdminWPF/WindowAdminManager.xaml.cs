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
    /// Interaction logic for WindowAdminManager.xaml
    /// </summary>
    public partial class WindowAdminManager : Window
    {
        private WindowLogin _windowLogin;
        private readonly IMemberRepository _memberRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        public WindowAdminManager(WindowLogin windowLogin, IMemberRepository memberRepository, IProductRepository productRepository, IOrderRepository orderRepository)
        {

            InitializeComponent();
            _windowLogin = windowLogin;
            _memberRepository = memberRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;

        }
        private void Button_Logout(object sender, RoutedEventArgs e)
        {
            this.Hide();
            _windowLogin.Show();
        }

        private void Goto_AdminMemberManager(object sender, RoutedEventArgs e)
        {
            PageAdminMemberManager pageAdminMemberManager = new PageAdminMemberManager(_memberRepository);
            frameAdmin.Content = pageAdminMemberManager;
        }

        private void Goto_AdminProductManager(object sender, RoutedEventArgs e)
        {
            PageAdminProductManager pageAdminProductManager = new PageAdminProductManager(_productRepository);
            frameAdmin.Content = pageAdminProductManager;
        }

        private void Goto_AdminOrderManager(object sender, RoutedEventArgs e)
        {
            PageAdminOrderManager pageAdminOrderManager = new PageAdminOrderManager(_orderRepository, _memberRepository);
            frameAdmin.Content = pageAdminOrderManager;
        }

    }
}
