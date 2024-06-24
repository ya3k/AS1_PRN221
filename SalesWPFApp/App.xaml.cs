using DataAccess.Repository;
using Microsoft.Extensions.DependencyInjection;
using SalesWPFApp.AdminWPF;
using SalesWPFApp.UserWPF;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 


    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

            services.AddSingleton<WindowAdminManager>();
            services.AddSingleton<WindowLogin>();

            services.AddSingleton<WindowUserProfile>();
            services.AddSingleton<PageOrderHistory>();
            services.AddSingleton<PageMemberProfile>();

            services.AddSingleton<WindowOrderCr>();
            services.AddSingleton<PageReportSales>();
            services.AddSingleton<PageAdminMemberManager>();
            services.AddSingleton<PageAdminProductManager>();
            services.AddSingleton<PageAdminOrderManager>();

            services.AddSingleton<WindowOrderDetailView>();
            services.AddSingleton<WindowMemberCreate>();
            services.AddSingleton<WindowProductCreate>();
            services.AddSingleton<WindowOrderCreate>();
            services.AddSingleton<WindowStore>();

            

        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var windowLogin = _serviceProvider.GetService<WindowLogin>();
            if(windowLogin != null)
            {
                windowLogin.Show();
            }
        }
    }

}
