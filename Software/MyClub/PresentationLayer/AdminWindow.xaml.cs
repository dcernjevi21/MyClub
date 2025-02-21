using PresentationLayer.Helper;
using PresentationLayer.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            GuiManager.SetMainWindow(this);
        }
        //černjević
        public void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcProfileAdmin());
        }
        //černjević
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GuiManager.CurrentWindow = this;
        }
        private void btnRegistrationRequests_Click(object sender, RoutedEventArgs e)
        {
            UCRegistrationRequests ucRegistrationRequests = new UCRegistrationRequests();
            contentPanel.Content = ucRegistrationRequests;
        }
        private void btnMatchesAdmin_Click(object sender, RoutedEventArgs e)
        {
            UcCoachMatchManagement ucMatchesAdmin = new UcCoachMatchManagement();
            contentPanel.Content = ucMatchesAdmin;
        }

        //valec
        private void btnTrainingsAdmin_Click(object sender, RoutedEventArgs e)
        {
            UcTrainingsAdmin ucTrainings = new UcTrainingsAdmin();
            contentPanel.Content = ucTrainings;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.Logout();
        }

        private void btnManageMemberships_Click(object sender, RoutedEventArgs e)
        {
            UCManageMemberships uCManageMemberships = new UCManageMemberships();
            contentPanel.Content = uCManageMemberships;
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1) DocumentationHelper.OpenUserDocumentation();
        }

        private void AdminWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1) OpenUserDocumentation();
        }

        private void OpenUserDocumentation()
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string projectPath = Directory.GetParent(basePath).Parent.Parent.FullName;
                string filePath = System.IO.Path.Combine(projectPath, "Resources", "MyClub-admin.pdf");

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("The specified file was not found.", filePath);
                }

                byte[] pdf = File.ReadAllBytes(filePath);
                using (MemoryStream ms = new MemoryStream(pdf))
                using (FileStream f = new FileStream("help-admin.pdf", FileMode.OpenOrCreate))
                {
                    ms.WriteTo(f);
                }
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
