﻿using BusinessLogicLayer;
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
    ///
    ///Černjević kompletno
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
        //TODO: Add 10 last login dates and time
        private void DisplayUserData()
        {
            CurrentUser.User = userProfileService.GetUserByEmail(CurrentUser.User.Email);
            if (CurrentUser.User != null)
            {
                lblName.Content = "Name: " + CurrentUser.User.FirstName + " " + CurrentUser.User.LastName;
                lblEmail.Content = "Email: " + CurrentUser.User.Email;
                lblBirthDate.Content = "Birth date: " + CurrentUser.User.BirthDate;
                lblRoleType.Content = "Role: User";
                imgProfilePicture.Source = ConvertToImage(CurrentUser.User.ProfilePicture);
            }
            else 
            { 
                ShowToast("How did you even get here??"); 
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
            GuiManager.OpenContent(new UcEditProfile(CurrentUser.User));
        }

        private void btnMyAttendances_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcUserMyAttendances());
        }

        private void ShowToast(string message)
        {
            new ToastWindow(message).Show();
        }
    }
}
