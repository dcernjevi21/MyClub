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
using System.Windows.Navigation;
using System.Windows.Shapes;

//černjević
namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UcProfile.xaml
    /// </summary>
    public partial class UcProfile : UserControl
    {
        private User user;

        private UserProfileServices userProfileService = new UserProfileServices();

        public UcProfile()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayUserData();
        }

        private void DisplayUserData()
        {
            //testni podataka
            var users = userProfileService.GetUserByEmail("admin@gmail.com");
            if (users != null && users.Count > 0)
            {
                var user = users.First();
                if(user.RoleID == 1) {
                    lblRoleType.Content = "Admin";
                }
                else if (user.RoleID == 2)
                {
                    lblRoleType.Content = "Coach";
                }
                else
                {
                    lblRoleType.Content = "User";
                }
                lblFirstName.Content = user.FirstName;
                lblLastName.Content = user.LastName;
                lblEmail.Content = user.Email;
                lblBirthDate.Content = user.BirthDate.ToString();
            }
        }

        private void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcEditProfile(user));
        }
    }
}
