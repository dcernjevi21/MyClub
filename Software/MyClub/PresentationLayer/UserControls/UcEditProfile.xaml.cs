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
using BusinessLogicLayer.Services;
using DataAccessLayer;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UcEditProfileUser.xaml
    /// </summary>
    /// 
    ///Černjević kompletno
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

        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var userService = new UserProfileServices();
            if (user == null)
            {
                ShowToast("User not found!");
                return;
            }

            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string email = txtEmail.Text;

            //update email
            if (!string.IsNullOrEmpty(email) && email != user.Email)
            {
                if (userService.ValidateEmail(email))
                {
                    //change email
                    user.Email = email;
                    bool a = userService.UpdateUser(user);
                    if (a)
                    {
                        ShowToast("Email updated successfully!");
                    }
                    else
                    {
                        ShowToast("Error updating email!");
                        return;
                    }
                }
                else
                {
                    ShowToast("Email is not valid! Correct format: example@example.com");
                    return;
                }
            }

            //update password
            if (!string.IsNullOrEmpty(password))
            {
                if (userService.ValidatePassword(password))
                {
                    if (password == confirmPassword) // Proveravamo da li je drugačija od stare lozinke
                    {
                        user.Password = password;
                        userService.UpdateUser(user);
                    }
                    else
                    {
                        ShowToast("New password must be different from the old one!");
                        return;
                    }
                }
                else
                {
                    ShowToast("Password must contain at least 8 characters, one uppercase letter, one lowercase letter and one digit!");
                    return;
                }
            }

            //update image
            if (imageBytes != null && imageBytes.Length > 0)
            {
                user.ProfilePicture = imageBytes;
                bool a = userService.UpdateUser(user);
                if (a)
                {
                    ShowToast("Profile picture updated successfully!");
                }
                else
                {
                    ShowToast("Error updating profile picture!");
                }
            }
            else
            {
                ShowToast("Profile picture not updated!");
            }
        
        GuiManager.CloseContent();
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
