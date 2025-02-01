using BusinessLogicLayer.Services;
using DataAccessLayer;
using PresentationLayer.Helper;
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

namespace PresentationLayer.UserControls
{
    public partial class UCUserMemberships : UserControl
    {
        private readonly MembershipService _membershipService;
        public UCUserMemberships()
        {
            InitializeComponent();
            _membershipService = new MembershipService();
            LoadMemberships();
        }

        private void LoadMemberships()
        {
            try
            {
                var memberships = _membershipService.GetAllMemberships()
                    .Where(m => m.UserID == CurrentUser.User.UserID)
                    .ToList();
                dataGridMemberships.ItemsSource = memberships;
            }
            catch (Exception ex)
            {
                SetStatus($"Error loading memberships: {ex.Message}", isError: true);
            }
        }

        private void SetStatus(string message, bool isError = false)
        {
            lblStatus.Text = message;
            lblStatus.Foreground = isError ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.DarkBlue;
            lblStatus.Visibility = string.IsNullOrEmpty(message) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void btnPayMembership_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridMemberships.SelectedItem is Membership selectedMembership)
            {
                try
                {
                    bool success = _membershipService.MarkAsPaid(selectedMembership.MembershipID);
                    if (success)
                    {
                        LoadMemberships(); 
                        SetStatus("Membership marked as paid.");
                    }
                    else
                    {
                        SetStatus("Membership for selected month is already paid for.", isError: true);
                    }
                }
                catch (Exception ex)
                {
                    SetStatus($"An error occurred: {ex.Message}", isError: true);
                }
            }
            else
            {
                SetStatus("Please select a membership to pay.", isError: true);
            }
        }
    }
}
   