using BusinessLogicLayer.Services;
using DataAccessLayer;
using EntitiesLayer.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.UserControls
{
    public partial class UCManageMemberships : UserControl
    {
        private readonly MembershipService _membershipService;

        public UCManageMemberships()
        {
            InitializeComponent();
            _membershipService = new MembershipService();
            LoadMemberships();
        }

        private void LoadMemberships()
        {
            try
            {
                var memberships = _membershipService.GetAllMemberships().ToList();
                dataGridUsers.ItemsSource = memberships;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading memberships: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdateMembership_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridUsers.SelectedItem is Membership selectedMembership)
            {
                try
                {
                    bool success = _membershipService.UpdateMembership(selectedMembership);
                    if (success)
                    {
                        MessageBox.Show("Membership updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadMemberships();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update membership.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a membership to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnMarkAllAsNotPaid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool success = _membershipService.MarkAllAsNotPaid();
                if (success)
                {
                    MessageBox.Show("All memberships marked as unpaid.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadMemberships();
                }
                else
                {
                    MessageBox.Show("No memberships were updated.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        MessageBox.Show("Membership marked as paid.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadMemberships();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update membership.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a membership to mark as paid.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnAssignMemberships_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool success = _membershipService.AssignMembershipsToAllMembers(30);
                if (success)
                {
                    MessageBox.Show("Memberships assigned successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadMemberships();
                }
                else
                {
                    MessageBox.Show("No new memberships were assigned.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
