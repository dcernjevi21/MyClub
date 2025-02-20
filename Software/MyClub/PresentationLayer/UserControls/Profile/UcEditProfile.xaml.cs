using BusinessLogicLayer;
using EntitiesLayer.Entities;
using Microsoft.Win32;
using PresentationLayer.Helper;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DataAccessLayer;
using System.Data.SqlTypes;

namespace PresentationLayer.UserControls
{
    public partial class UcEditProfile : UserControl
    {
        private User user;
        private byte[] imageBytes;
        private readonly UserProfileServices userService;

        public UcEditProfile(User fetchedUser)
        {
            InitializeComponent();
            user = fetchedUser;
            userService = new UserProfileServices();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtEmail.Text = user.Email;
            imgProfile.Source = ConvertToImage(user.ProfilePicture);
            txtFirstName.Text = user.FirstName;
            txtLastName.Text = user.LastName;
        }

        private void ShowToast(string message)
        {
            new ToastWindow(message).Show();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (user == null)
            {
                ShowToast("User not found!");
                return;
            }

            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string newEmail = txtEmail.Text;
            string newPassword = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            bool isUpdated = false;

            if (!string.IsNullOrEmpty(firstName) && firstName != user.FirstName)
            {
                if (userService.ValidateName(firstName))
                {
                    user.FirstName = firstName;
                    isUpdated = true;
                }
                else
                {
                    ShowToast($"{firstName} is not valid.");
                    return;
                }
            }
            else if(string.IsNullOrEmpty(firstName))
            {
                ShowToast("Please fill in all mandatory fields.");
                return;
            }

            if(!string.IsNullOrEmpty(lastName) && lastName != user.LastName)
            {
                if(userService.ValidateName(lastName))
                {
                    user.LastName = lastName;
                    isUpdated = true;
                }
                else
                {
                    ShowToast($"{lastName} is not valid.");
                    return;
                }
            }
            else if(string.IsNullOrEmpty(lastName))
            {
                ShowToast("Please fill in all mandatory fields.");
                return;
            }

            if (!string.IsNullOrEmpty(newEmail) && newEmail != user.Email)
            {
                if (userService.GetUserByEmail(newEmail) != null)
                {
                    ShowToast("A user already exist with the given email.");
                    return;
                }
                else
                {
                    if (!userService.ValidateEmail(newEmail))
                    {
                        ShowToast("Invalid email format! Correct format: example@example.com");
                        return;
                    }
                    user.Email = newEmail;
                    isUpdated = true;
                }
            }
            else if (string.IsNullOrEmpty(newEmail))
            {
                ShowToast("Please fill in all mandatory fields.");
                return;
            }

            if (!string.IsNullOrEmpty(newPassword))
            {
                if (!userService.ValidatePassword(newPassword))
                {
                    ShowToast("Password must contain at least 8 characters, one uppercase letter, one lowercase letter and one digit!");
                    return;
                }
                if (newPassword != confirmPassword)
                {
                    ShowToast("Passwords do not match!");
                    return;
                }
                user.Password = newPassword;
                isUpdated = true;
            }

            if (imageBytes != null && imageBytes.Length > 0)
            {
                user.ProfilePicture = imageBytes;
                isUpdated = true;
            }

            // Ažuriranje korisnika samo ako je bilo promjena
            if (isUpdated)
            {
                bool updateSuccess = userService.UpdateUser(user);
                ShowToast(updateSuccess ? "Profile updated successfully!" : "Error updating profile!");
                GuiManager.CloseContent();
            }
            else
            {
                ShowToast("No changes detected.");
                GuiManager.CloseContent();
            }
        }

        private void btnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif, *.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp"
            };

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
    }
}
