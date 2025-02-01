using BusinessLogicLayer;
using EntitiesLayer.Entities;
using PresentationLayer.Helper;
using System;
using System.Collections.Generic;
using System.IO;
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

//Černjević
namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UcProfile.xaml
    /// </summary>
    public partial class UcProfileUser : UserControl
    {
        private UserProfileServices userProfileService = new UserProfileServices();

        public UcProfileUser()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayUserData();
        }
        //dodati popis zadnjih 10 prijava
        private void DisplayUserData()
        {
            //testni podataka
            var users = userProfileService.GetUserByEmail(CurrentUser.User.Email);
            CurrentUser.User = users.FirstOrDefault();
            if (users != null && users.Count > 0)
            {
                var user = users.First();
                lblFirstName.Content = user.FirstName;
                lblLastName.Content = user.LastName;
                lblEmail.Content = user.Email;
                lblBirthDate.Content = user.BirthDate.ToString();
                lblRoleType.Content = "User";
                imgProfilePicture.Source = ConvertToImage(user.ProfilePicture);
            }
        }

        private BitmapImage ConvertToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;

            BitmapImage image = new BitmapImage();
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                image.BeginInit();
                image.StreamSource = ms;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
            }
            return image;
        }

        private void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcEditProfileUser(CurrentUser.User));
        }

        private void btnMyAttendances_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcUserAttendances());
        }
    }
}
