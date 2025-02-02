using BusinessLogicLayer.Services;
using DataAccessLayer;
using EntitiesLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class UCRegistrationRequests : UserControl
    {
        private readonly UserService _userService;
        private ObservableCollection<User> _users;

        public UCRegistrationRequests()
        {
            InitializeComponent();
            _userService = new UserService();
            LoadPendingUsers();
        }
        private void LoadPendingUsers()
        {

            var pendingUsers = _userService.GetPendingUsers();
            _users = new ObservableCollection<User>(pendingUsers);
            dataGridUsers.ItemsSource = _users;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var user = button?.DataContext as User;
            if (user != null)
            {
                bool success = _userService.AcceptUser(user);
                if (success)
                {
                    LoadPendingUsers(); 
                }
                else
                {
                    MessageBox.Show("Failed to accept user.");
                }
            }
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var user = button?.DataContext as User;
            if (user != null)
            {
                bool success = _userService.RejectUser(user);
                if (success)
                {
                    LoadPendingUsers(); // Reload the list after rejection
                }
                else
                {
                    MessageBox.Show("Failed to reject user.");
                }
            }
        }
    }
}