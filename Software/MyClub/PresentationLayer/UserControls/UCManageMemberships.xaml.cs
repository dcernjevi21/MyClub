using BusinessLogicLayer.Services;
using DataAccessLayer;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCManageMemberships.xaml
    /// </summary>
    public partial class UCManageMemberships : UserControl
    {
        private readonly MembershipService _membershipService;

        public UCManageMemberships()
        {
            InitializeComponent();
            _membershipService = new MembershipService();
            PopulateMonthFilter();
            LoadMemberships();
        }

        private void SetStatus(string message, bool isError = false)
        {
            lblStatus.Text = message;
            lblStatus.Foreground = isError ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.DarkBlue;
            lblStatus.Visibility = string.IsNullOrEmpty(message) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void LoadMemberships(DateTime? filterMonth = null)
        {
            try
            {
                List<Membership> memberships;
                if (filterMonth.HasValue)
                {
                    memberships = _membershipService.GetMembershipsByMonth(filterMonth.Value).ToList();
                }
                else
                {
                    memberships = _membershipService.GetAllMemberships();
                }
                dataGridUsers.ItemsSource = memberships;
                SetStatus("Memberships loaded successfully.");
            }
            catch (Exception ex)
            {
                SetStatus($"Error loading memberships: {ex.Message}", true);
            }
        }

        private void PopulateMonthFilter()
        {
            int year = DateTime.Now.Year;
            List<DateTime> months = new List<DateTime>();
            for (int m = 1; m <= 12; m++)
            {
                months.Add(new DateTime(year, m, 1));
            }
            cmbMonthFilter.ItemsSource = months;
            cmbMonthFilter.SelectedItem = months.FirstOrDefault();
        }


        private void cmbMonthFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMonthFilter.SelectedItem is DateTime selectedMonth)
            {
                LoadMemberships(selectedMonth);
            }
        }

        private void btnMarkAsPaid_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridUsers.SelectedItem is Membership selectedMembership)
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
                        SetStatus("Membership is already paid.", true);
                    }
                }
                catch (Exception ex)
                {
                    SetStatus($"An error occurred: {ex.Message}", true);
                }
            }
            else
            {
                SetStatus("Please select a membership to mark as paid.", true);
            }
        }

        private void btnMarkAsUnpaid_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridUsers.SelectedItem is Membership selectedMembership)
            {
                try
                {
                    bool success = _membershipService.MarkAsUnpaid(selectedMembership.MembershipID);
                    if (success)
                    {
                        LoadMemberships();
                        SetStatus("Membership marked as unpaid.");
                    }
                    else
                    {
                        SetStatus("Membership is already unpaid..", true);
                    }
                }
                catch (Exception ex)
                {
                    SetStatus($"An error occurred: {ex.Message}", true);
                }
            }
            else
            {
                SetStatus("Please select a membership to mark as unpaid.", true);
            }
        }

        private void btnMarkAllAsNotPaid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool success = _membershipService.MarkAllAsNotPaid();
                if (success)
                {
                    LoadMemberships();
                    SetStatus("All memberships marked as unpaid.");
                }
                else
                {
                    SetStatus("No memberships were updated.", true);
                }
            }
            catch (Exception ex)
            {
                SetStatus($"An error occurred: {ex.Message}", true);
            }
        }


        private void btnDeleteMembership_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridUsers.SelectedItem is Membership selectedMembership)
            {
                try
                {
                    bool success = _membershipService.DeleteMembership(selectedMembership);
                    if (success)
                    {
                        LoadMemberships();
                        SetStatus("Membership deleted successfully.");
                    }
                    else
                    {
                        SetStatus("Failed to delete membership.", true);
                    }
                }
                catch (Exception ex)
                {
                    SetStatus($"An error occurred: {ex.Message}", true);
                }
            }
            else
            {
                SetStatus("Please select a membership to delete.", true);
            }
        }

        private void btnAssignMemberships_Click(object sender, RoutedEventArgs e)
        {
            decimal defaultAmount = 30; 
            try
            {
                bool success = _membershipService.AssignMembershipsToAllMembers(defaultAmount);
                if (success)
                {
                    LoadMemberships();
                    SetStatus("Memberships assigned successfully!");
                }
                else
                {
                    SetStatus("No new memberships were assigned.");
                }
            }
            catch (Exception ex)
            {
                SetStatus($"An error occurred: {ex.Message}", true);
            }
        }
    }
}
