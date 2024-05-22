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
    /// Interaction logic for WindowMemberCreate.xaml
    /// </summary>
    public partial class WindowMemberCreate : Window
    {
        private readonly PageAdminMemberManager _pageAdminMemberManager;
        private readonly IMemberRepository _memberRepository;
        private Member? member;
        public WindowMemberCreate(PageAdminMemberManager pageAdminMemberManager, Member member,IMemberRepository memberRepository)
        {
            InitializeComponent();
            this._pageAdminMemberManager = pageAdminMemberManager;
            this._memberRepository = memberRepository;
            this.member = member;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (member != null)
            {
                txtBoxEmail.Text = member.Email;
                txtBoxCompanyName.Text = member.CompanyName;
                txtBoxCity.Text = member.City;
                txtBoxCountry.Text = member.Country;
                txtBoxPassword.Text = member.Password;
                txtBoxId.Text = member.MemberId.ToString();
                txtBoxId.Visibility = Visibility.Visible;
                labelId.Visibility = Visibility.Visible;
                btn.Content = "Update";
                this.Height = 550;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (member == null)
                {
                    Member member = new Member
                    {
                        Email = txtBoxEmail.Text,
                        CompanyName = txtBoxCompanyName.Text,
                        City = txtBoxCity.Text,
                        Country = txtBoxCountry.Text,
                        Password = txtBoxPassword.Text
                    };
                    _memberRepository.Add(member);
                }
                else
                {
                    member.Email = txtBoxEmail.Text;
                    member.CompanyName = txtBoxCompanyName.Text;
                    member.City = txtBoxCity.Text;
                    member.Country = txtBoxCountry.Text;
                    member.Password = txtBoxPassword.Text;
                    _memberRepository.Update(member);
                }
                _pageAdminMemberManager.RefreshListView();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
