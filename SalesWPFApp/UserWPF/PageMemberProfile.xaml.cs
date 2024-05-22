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

namespace SalesWPFApp.UserWPF
{
    /// <summary>
    /// Interaction logic for PageMemberProfile.xaml
    /// </summary>
    public partial class PageMemberProfile : Page
    {
        private readonly IMemberRepository _memberRepository;
        private Member? _member;
        public PageMemberProfile(IMemberRepository memberRepository, Member member)

        {
            InitializeComponent();
            _memberRepository = memberRepository;
            this._member = member;
            tbMemberName.Text = _member.Email;
            Loaded += PageMemberProfile_Loaded;
        }

        private void PageMemberProfile_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMemberObject();
        }


        private void LoadMemberObject()
        {
            txtBoxEmail.Text = _member.Email;
            txtBoxCompanyName.Text = _member.CompanyName;
            txtBoxCity.Text = _member.City;
            txtBoxCountry.Text = _member.Country;
            txtBoxPassword.Text = _member.Password;
            txtBoxId.Text = _member.MemberId.ToString();
            //txtBoxId.Visibility = Visibility.Visible;
            //labelId.Visibility = Visibility.Visible;

            txtBoxId.Visibility = Visibility.Collapsed;
            labelId.Visibility = Visibility.Collapsed;
            btn.Content = "Update";
            this.Height = 550;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            _member.Email = txtBoxEmail.Text;
            _member.CompanyName = txtBoxCompanyName.Text;
           _member.City = txtBoxCity.Text;
            _member.Country = txtBoxCountry.Text;
            _member.Password = txtBoxPassword.Text;
            _memberRepository.Update(_member);
            MessageBox.Show("Update success");
            RefreshMember();

        }

        private void RefreshMember()
        {
            _member = _memberRepository.FindById(_member.MemberId);
            LoadMemberObject();
        }


        }
}
