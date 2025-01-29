using BusinessLogicLayer.Services;
using DataAccessLayer;
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
using System.Windows.Shapes;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for FrmRegistration.xaml
    /// </summary>
    public partial class FrmRegistration : Window
    {
        private readonly UserService _userService;
        public FrmRegistration()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var newUser = new User
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Email = txtEmail.Text,
                Username = txtUsername.Text,
                Password = txtPassword.Password,
                BirthDate = dpBirthDate.SelectedDate ?? default(DateTime),
                RoleID = cmbRole.SelectedIndex + 1, 
                StatusID = 1,
                TeamID = 1
            };

            bool isRegistered = _userService.RegisterUser(newUser);

            if (isRegistered)
            {
                MessageBox.Show("Registration successful! Your account is pending approval.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Registration failed. Email might already be in use.");
            }
        }
    }
}