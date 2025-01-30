using BusinessLogicLayer.Services;
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
    /// Interaction logic for UcMatchManagement.xaml
    /// </summary>
    public partial class UcMatchManagement : UserControl
    {
        private MatchManagementService _matchManagementService = new MatchManagementService();

        public UcMatchManagement()
        {
            InitializeComponent();
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMatches();
        }

        public void LoadMatches()
        {
            dgCoachGrid.ItemsSource = _matchManagementService.GetMatches();
        }

        public void btnAddMatch_Click(object sender, RoutedEventArgs e)
        {
            //GuiManager.OpenContent(new UcAddMatch(new Match()));
        }

        public void btnUpdateMatch_Click(object sender, RoutedEventArgs e)
        {

        }

        public void btnDeleteMatch_Click(object sender, RoutedEventArgs e)
        {

        }

        public void btnPostponeMatch_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
