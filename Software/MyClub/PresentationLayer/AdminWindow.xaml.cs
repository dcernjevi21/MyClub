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

        private void btnManageMemberships_Click(object sender, RoutedEventArgs e)
        {
            UCManageMemberships uCManageMemberships = new UCManageMemberships();
            contentPanel.Content = uCManageMemberships;
        }
    }
}
