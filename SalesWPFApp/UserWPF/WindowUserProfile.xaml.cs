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
    /// Interaction logic for WindowUserProfile.xaml
    /// </summary>
    public partial class WindowUserProfile : Window
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductRepository _productRepository;
        private Member? _member;
        public WindowUserProfile(IMemberRepository memberRepository,IOrderRepository orderRepository, IProductRepository productRepository, IOrderDetailRepository orderDetailRepository, Member member)
        {
            InitializeComponent();
            _memberRepository = memberRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
            this._member = member;
            tbName.Text = _member.Email;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Goto_Profile(object sender, MouseButtonEventArgs e)
        {
            PageMemberProfile pageMemberProfile = new PageMemberProfile(_memberRepository, _member);
            frameAdmin.Content = pageMemberProfile;

        }

        private void Goto_OrderHistory(object sender, MouseButtonEventArgs e)
        {
            PageOrderHistory pageOrderHistory = new PageOrderHistory( _orderRepository, _memberRepository,_productRepository,_orderDetailRepository, _member);
            frameAdmin.Content = pageOrderHistory;
        }

        private void Button_Logout(object sender, RoutedEventArgs e)
        {

        }
    }
}
