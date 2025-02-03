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

        //černjević
        private void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcProfileUser());
        }
        //Valec
        private void btnAttendances_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcAttendancesUser());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GuiManager.CurrentWindow = this;
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
    }
}
