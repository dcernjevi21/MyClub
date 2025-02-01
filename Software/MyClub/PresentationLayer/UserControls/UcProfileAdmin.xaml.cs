using BusinessLogicLayer;
using DataAccessLayer;
using EntitiesLayer.Entities;
using PresentationLayer.Helper;
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

//sve Černjević
namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UcProfileAdmin.xaml
    /// </summary>
    public partial class UcProfileAdmin : UserControl
    {
        private UserProfileServices userProfileService = new UserProfileServices();

        public UcProfileAdmin()
        {
            InitializeComponent();
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayCoachData();
        }

        public void DisplayCoachData()
        {
            lblName.Content = "Name: " + CurrentUser.User.FirstName + " " + CurrentUser.User.LastName;
            lblEmail.Content = "Email: " + CurrentUser.User.Email;
            lblBirthDate.Content = "Birth date: " + CurrentUser.User.BirthDate;
            lblRoleType.Content = "Role: Admin";
            dgCoachGrid.ItemsSource = userProfileService.GetUsersByRoleId(2); //userid 2 is for coach
        }

        public void btnEditCoachProfile_Click(object sender, RoutedEventArgs e)
        {
            User coach = GetSelectedCoach();
            if (coach == null)
            {
                ShowToast("Please select a coach!");
                return;
            }
            else 
            {
                GuiManager.OpenContent(new UcEditProfile(coach));
            }

        }

        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }

        private User GetSelectedCoach()
        {
            return dgCoachGrid.SelectedItem as User;

        }

        public void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcEditProfile(CurrentUser.User));
        }

    }
}
