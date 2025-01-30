using BusinessLogicLayer;
using EntitiesLayer.Entities;
using Microsoft.Win32;
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
using Microsoft.Win32;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UcEditProfile.xaml
    /// </summary>
    public partial class UcEditProfile : UserControl
    {
        private User user;
        byte[] imageBytes;

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
            var user = CurrentUser.User;
            user.UserID = user.UserID;
            if (user == null)
            {
                MessageBox.Show("User not found!");
                return;
            }

            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string email = txtEmail.Text;

            if (!string.IsNullOrEmpty(email) && email != user.Email)
            {
                if (userService.ValidateEmail(email))
                {
                    //change email
                    user.Email = email;
                    bool a = userService.UpdateUser(user);
                    if (a)
                    {
                        MessageBox.Show("Email updated successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Error updating email!");
                    }
                    GuiManager.CloseContent();
                }
                else
                {
                    MessageBox.Show("Email is not valid!");
                    return;
                }
            }

            if (!string.IsNullOrEmpty(password) && password == confirmPassword)
            {
                if (password != user.Password) // Proveravamo da li je drugačija od stare lozinke
                {
                    user.Password = password;
                    userService.UpdateUser(user);
                    GuiManager.CloseContent();
                }
                else
                {
                    MessageBox.Show("New password must be different from the old one!");
                    return;
                }
            }

            if(imageBytes != null)
            {
                user.ProfilePicture = imageBytes;
                bool a = userService.UpdateUser(user);
                if(a)
                {
                    MessageBox.Show("Profile picture updated successfully!");
                }
                else { MessageBox.Show("Error updating profile picture!"); }
                GuiManager.CloseContent();
            }
            return;
        }

        private void btnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif, *.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string imagePath = openFileDialog.FileName;
                    imageBytes = File.ReadAllBytes(imagePath);

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new MemoryStream(imageBytes);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    imgProfile.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
        }
    }
}
