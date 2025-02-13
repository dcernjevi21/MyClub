using PresentationLayer.Helper;
using PresentationLayer.UserControls;
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
using System.IO;
using System.Diagnostics;
using EntitiesLayer.Entities;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
            GuiManager.SetMainWindow(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GuiManager.CurrentWindow = this;
        }

        //černjević
        private void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcProfileUser());
        }

        private void btnTrainingSchedule_Click(object sender, RoutedEventArgs e)
        {
            //Černjević
            if (CurrentUser.User.TeamID == null)
            {
                ShowToast("You aren't assigned to a team.");
                GuiManager.CloseContent();
                return;
            }
            //Valec
            else
            {
                GuiManager.OpenContent(new UcScheduleTrainingUser());
            } 
        }
        //Černjević
        private void btnMatchSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser.User.TeamID == null)
            {
                ShowToast("You aren't assigned to a team.");
                GuiManager.CloseContent();
                return;
            }
            else
            {
                GuiManager.OpenContent(new UcScheduleMatchesUser());
            }
        }

        private void btnMyMembership_Click(object sender, RoutedEventArgs e)
        {
            UCUserMemberships ucUserMemberships = new UCUserMemberships();
            contentPanel.Content = ucUserMemberships;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.Logout();
        }

        private void btnMyEvaluations_Click(object sender, RoutedEventArgs e)
        {
            UCAthleteEvaluations ucAthleteEvaluations = new UCAthleteEvaluations();
            contentPanel.Content = ucAthleteEvaluations;
        }
        //Černjević
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1) DocumentationHelper.OpenUserDocumentation();
        }

        private void UserWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1) OpenUserDocumentation();
        }

        private void OpenUserDocumentation()
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string projectPath = Directory.GetParent(basePath).Parent.Parent.FullName;
                string filePath = System.IO.Path.Combine(projectPath, "Resources", "MyClub-user.pdf");

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("The specified file was not found.", filePath);
                }

                byte[] pdf = File.ReadAllBytes(filePath);
                using (MemoryStream ms = new MemoryStream(pdf))
                using (FileStream f = new FileStream("help-user.pdf", FileMode.OpenOrCreate))
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

        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }
    }
}
