using BusinessLogicLayer.Services;
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
using EntitiesLayer.Entities;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UcScheduleMatchesUser.xaml
    /// </summary>
    /// 
    ///Černjević kompletno
    public partial class UcScheduleMatchesUser : UserControl
    {
        private MatchManagementService _matchManagementService = new MatchManagementService();
        private TrainingService _trainingService = new TrainingService();
        private int teamId = CurrentUser.User.TeamID.GetValueOrDefault();

        public UcScheduleMatchesUser()
        {
            InitializeComponent();
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _ = LoadMatches(); //fire and forget
        }

        public async Task LoadMatches()
        {
            dgMatchGrid.ItemsSource = await _matchManagementService.GetMatchesByTeamId(teamId);
        }

        private void btnFilterMatches_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            _ = LoadMatches();
        }

        public void btnMarkAttendance_Click(object sender, RoutedEventArgs e)
        {
            Match match = GetMatchAttendance();
            if (match != null)
            {
                if (DateTime.Now < match.MatchDate)
                {
                    GuiManager.OpenContent(new UcMarkAttendance(0, match.MatchID, null, match));
                }
                else
                {
                    MessageBox.Show("Cannot mark attendance for past matches.");
                }
            }
            else
            {
                ShowToast("Please select a match to mark attendance for.");
            }
        }

        public Match GetMatchAttendance()
        {
            return dgMatchGrid.SelectedItem as Match;
        }

        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }
    }
}
