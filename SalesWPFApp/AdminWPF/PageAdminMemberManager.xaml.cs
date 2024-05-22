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
    /// Interaction logic for PageAdminMemberManager.xaml
    /// </summary>
    public partial class PageAdminMemberManager : Page
    {
        private readonly IMemberRepository _memberRepository;
        public PageAdminMemberManager(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
            InitializeComponent();
            listView.SelectionChanged += ListView_SelectionChanged;

        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = _memberRepository.GetAllMembers();

        }

        private void Button_Reload(object sender, RoutedEventArgs e)
        {
            RefreshListView();
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Do you wan't remove member seledted?", "Remove member", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    //mutiple select deletes
                    List<Member> members = listView.SelectedItems.Cast<Member>().ToList();
                    members.ForEach(member => _memberRepository.Delete(member));
                    listView.ItemsSource = members;
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
                    List<Member> selectedMembers = listView.SelectedItems.Cast<Member>().ToList();
                    foreach (Member member in selectedMembers)
                    {
                        WindowMemberCreate adminMemberCreate = new WindowMemberCreate(this, member, _memberRepository);
                        adminMemberCreate.ShowDialog();
                    }

                    //var member = (Member)listView.SelectedItem;
                    //WindowMemberCreate windowMemberCreate = new WindowMemberCreate(this, member, _memberRepository);
                    //windowMemberCreate.ShowDialog();

                }
                else
                {
                    MessageBox.Show("Please select a member to edit.", "No Member Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to edit the member: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button_Insert(object sender, RoutedEventArgs e)
        {
            WindowMemberCreate windowMemberCreate = new WindowMemberCreate(this, null, _memberRepository);
            windowMemberCreate.ShowDialog();

        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {

        }

        public void RefreshListView()
        {
            listView.ItemsSource = _memberRepository.GetAllMembers();
        }




        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView? listView = sender as ListView;
            GridView? gridView = listView != null ? listView.View as GridView : null;

            var width = listView != null ? listView.ActualWidth - SystemParameters.VerticalScrollBarWidth : this.Width;

            var column1 = 0.1;
            var column2 = 0.2;
            var column3 = 0.2;
            var column4 = 0.2;
            var column5 = 0.1;
            var column6 = 0.2;

            if (gridView != null && width >= 0)
            {
                gridView.Columns[0].Width = width * column1;
                gridView.Columns[1].Width = width * column2;
                gridView.Columns[2].Width = width * column3;
                gridView.Columns[3].Width = width * column4;
                gridView.Columns[4].Width = width * column5;
                gridView.Columns[5].Width = width * column6;
            }
        }



        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int count = listView.SelectedItems.Count;
            if (count > 0)
            {
                Member selectedMember = (Member)listView.SelectedItem;

                // Set the TextBox values with the selected member's properties
                tbMemberId.Text = selectedMember.MemberId.ToString();
                tbMemberEmail.Text = selectedMember.Email;
                tbCompanyName.Text = selectedMember.CompanyName;
                tbCity.Text = selectedMember.City;
                tbCountry.Text = selectedMember.Country;


                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnEdit.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
        }




    }
}
