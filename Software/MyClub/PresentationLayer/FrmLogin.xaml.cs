using System;
using System.Linq;
using System.Windows;
using DataAccessLayer;
using BusinessLogicLayer;
using System.Security.Cryptography;
using System.Text;
using PresentationLayer.Helper;

namespace PresentationLayer
{
    public partial class FrmLogin : Window
    {
        private MyClubModel _context;

        public FrmLogin()
        {
            InitializeComponent();
            _context = new MyClubModel();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowError("Email and password are required!");
                return;
            }

            if (AuthenticateUser(email, password))
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                OpenMainWindow(email);
                this.Close();
            }
            else
            {
                ShowError("Invalid email or password!");
            }
        }

        private bool AuthenticateUser(string email, string password)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
                if (user != null)
                {
                    CurrentUser.User = user;
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void ShowError(string message)
        {
            lblErrorMessage.Text = message;
            lblErrorMessage.Visibility = Visibility.Visible;
        }

        private void OpenMainWindow(string email)
        {
            switch (CurrentUser.User.RoleID)
            {
                case 1:
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();
                    break;
                case 2:
                    CoachWindow coachWindow = new CoachWindow();
                    coachWindow.Show();
                    break;
                case 3:
                    UserWindow userWindow = new UserWindow();
                    userWindow.Show();
                    break;
                default:
                    MessageBox.Show("User role is not recognized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

            this.Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            FrmRegistration registerForm = new FrmRegistration();
            registerForm.Show();
            this.Close();
        }
    }
}