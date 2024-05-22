using DataAccess.Repository;
using SalesWPFApp.AdminWPF;
using SalesWPFApp.UserWPF;
using System;
using System.Windows;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for WindowLogin.xaml
    /// </summary>
    public partial class WindowLogin : Window
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public WindowLogin(IMemberRepository memberRepository, IProductRepository productRepository, IOrderRepository orderRepository)
        {
            InitializeComponent();
            _memberRepository = memberRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = txtBoxUserName.Text;
                string password = pwdBoxPassword.Password;
                var memLogin = _memberRepository.Login(username, password);
                var member = _memberRepository.FindByEmail(username);
                if (memLogin == 0)
                {
                    Cart.Email = username;
                    WindowAdminManager windowAdminManager = new WindowAdminManager(this,_memberRepository, _productRepository, _orderRepository);
                    windowAdminManager.Show();
                    this.Hide();
                }
                else if(memLogin == 1)
                {
                    //Cart.Email = username;
                    //WindowStore windowStore = new WindowStore(this, _productRepository, _memberRepository , _orderRepository);
                    //windowStore.Show();
                    WindowUserProfile windowUserProfile = new WindowUserProfile(_memberRepository, _orderRepository,member);
                    windowUserProfile.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password");
                }

                    
            }
            catch (Exception ex)
            {
              throw ex;
            }
        }
    }
}
