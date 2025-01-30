using BusinessLogicLayer;
using EntitiesLayer.Entities;
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
    /// <summary>
    /// Interaction logic for UcEditProfile.xaml
    /// </summary>
    public partial class UcEditProfile : UserControl
    {
        private User user;

        public UcEditProfile(User fetchedUser)
        {
            InitializeComponent();
            user = fetchedUser;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var userService = new UserProfileServices();
            var user = new User();

            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string email = txtEmail.Text;

            if (email == null || password == null || confirmPassword == null)
            {
                return;
            }

            if (email != null)
            {
                //validate email
                if(userService.ValidateEmail(email))
                {
                    //change email
                    userService.ChangeEmail(user);
                }
                else
                {
                    MessageBox.Show("Email is not valid!");
                    return;
                }
                GuiManager.CloseContent();
            }

            if (password != null && confirmPassword != null)
            {
                if (password != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match!");
                    return;
                }
                else
                {
                    //change password

                    GuiManager.CloseContent();
                }
            }
            return;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
        }
    }
}
