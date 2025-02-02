using BusinessLogicLayer.Services;
using DataAccessLayer;
using EntitiesLayer.Entities;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace PresentationLayer
{
    public partial class FrmRegistration : Window
    {
        private readonly UserService _userService;

        public FrmRegistration(UserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            lblErrorMessage.Visibility = Visibility.Hidden;

            string errorMessage = ValidateFields();

            if (!string.IsNullOrEmpty(errorMessage))
            {
                lblErrorMessage.Text = errorMessage;
                lblErrorMessage.Visibility = Visibility.Visible;
                return;
            }

            var newUser = new User
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Email = txtEmail.Text,
                Username = txtUsername.Text,
                Password = txtPassword.Password,
                BirthDate = dpBirthDate.SelectedDate ?? default(DateTime),
                RoleID = cmbRole.SelectedIndex + 1,
                StatusID = 1, //PENDING STATUS
                TeamID = 1
            };

            string registrationError;
            bool isRegistered = _userService.RegisterUser(newUser, out registrationError);

            if (isRegistered)
            {
                MessageBox.Show("Registration successful! Your account is pending approval.");
                FrmLogin loginForm = new FrmLogin();
                loginForm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(registrationError);
            }
        }

        private string ValidateFields()
        {

            if (string.IsNullOrEmpty(txtFirstName.Text))
            {
                return "First Name is required.";
            }

            if (string.IsNullOrEmpty(txtLastName.Text))
            {
                return "Last Name is required.";
            }

            if (string.IsNullOrEmpty(txtEmail.Text) )
            {
                return "Email is required.";
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                return "Email format is not valid.";
            }

            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                return "Username is required.";
            }


            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                return "Password is required.";
            }

            if (dpBirthDate.SelectedDate == null)
            {
                return "Birthdate is required.";
            }


            if (cmbRole.SelectedIndex == -1)
            {
                return "Role is required.";
            }

            return string.Empty;  
        }

        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        private void imgBack_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FrmLogin loginForm = new FrmLogin();
            loginForm.Show();
            this.Close();
        }
    }
}
