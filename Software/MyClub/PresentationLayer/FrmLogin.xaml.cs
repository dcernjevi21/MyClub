using System;
using System.Windows;
using BusinessLogicLayer.Services;
using PresentationLayer.Helper;

namespace PresentationLayer
{
    public partial class FrmLogin : Window
    {
        private readonly UserService _userService;

        public FrmLogin()
        {
            InitializeComponent();
            _userService = new UserService();
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

            var user = _userService.AuthenticateUser(email, password);
            if (user != null)
            {
                CurrentUser.User = user;
                OpenMainWindow();
                Close();
            }
            else
            {
                ShowError("Invalid email or password!");
            }
        }

        private void ShowError(string message)
        {
            lblErrorMessage.Text = message;
            lblErrorMessage.Visibility = Visibility.Visible;
        }

        private void OpenMainWindow()
        {
            switch (CurrentUser.User.RoleID)
            {
                case 1:
                    new AdminWindow().Show();
                    break;
                case 2:
                    new CoachWindow().Show();
                    break;
                case 3:
                    new UserWindow().Show();
                    break;
                default:
                    MessageBox.Show("User role is not recognized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
            Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            FrmRegistration registerForm = new FrmRegistration(_userService);
            registerForm.Show();
            Close();
        }
    }
}
