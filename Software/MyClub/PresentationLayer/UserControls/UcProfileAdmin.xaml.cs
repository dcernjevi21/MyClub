using BusinessLogicLayer;
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
            dgCoachGrid.ItemsSource = userProfileService.GetUserByRoleId(2);
        }

        public void btnEditCoachProfile_Click(object sender, RoutedEventArgs e)
        {

        }

        public void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcEditProfileUser(CurrentUser.User));
        }

    }
}
