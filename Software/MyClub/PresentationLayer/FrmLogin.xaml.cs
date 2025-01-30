using System;
using System.Linq;
using System.Windows;
using DataAccessLayer;  // Import your DAL
using BusinessLogicLayer;   // Import business logic if applicable
using System.Security.Cryptography;
using System.Text;

namespace PresentationLayer
{
    public partial class FrmLogin : Window
    {
        private MyClubContext _context;

        public FrmLogin()
        {
            InitializeComponent();
            _context = new MyClubContext();
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
                OpenMainWindow();
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
                return user != null;
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

        private void OpenMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            FrmRegistration registerForm = new FrmRegistration();
            registerForm.Show();
            this.Close();
        }
    }
}