using System;
using BusinessLogicLayer;
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
using PresentationLayer.Helper;
using System.Text.RegularExpressions;
using BusinessLogicLayer.Services;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UcAddatch.xaml
    /// </summary>
    /// 
    /// 
    ///Černjević kompletno
    public partial class UcAddMatch : UserControl
    {
        MatchManagementService _matchManagementService = new MatchManagementService();
        TrainingService _trainingService = new TrainingService();
        TeamService _teamService = new TeamService();

        public UcAddMatch()
        {
            InitializeComponent();
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(CurrentUser.User.RoleID == 1)
            {
                LoadTeams();
            }
        }

        private async void LoadTeams()
        {
            try
            {
                var teams = await _teamService.GetTeamsAsync(); 
                cmbTeams.ItemsSource = teams; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load teams: " + ex.Message);
            }
        }


        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }

        private void btnAddMatch_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = dtMatchDate.SelectedDate;
            if (selectedDate == null)
            {
                ShowToast("Please select a match date.");
                return;
            }
            DateTime matchDate = selectedDate.Value;


            string opponentTeam = txtOpponent.Text;
            string location = txtLocation.Text;
            string startTime = txtStartTime.Text;

            if (!TimeSpan.TryParse(startTime, out TimeSpan startTimeParsed))
            {
                ShowToast("Invalid time format. Please enter time in HH:mm format.");
                return;
            }

            int teamId = cmbTeams.SelectedValue as int? ?? 0;

            if (teamId == 0 && CurrentUser.User.RoleID == 1) //ako admin dodaje
            {
                ShowToast("Please select a team.");
                return;
            }

            if (CurrentUser.User.RoleID == 2) //ako coach dodaje 
            {
                teamId = (int)CurrentUser.User.TeamID;
            }

            bool matchExists = _matchManagementService.DoesMatchExist(teamId, matchDate, startTimeParsed);
            bool trainingExists = _trainingService.DoesTrainingExist(teamId, matchDate, startTimeParsed);

            //dodati slanje e maila korisnicima nakon sto se zakaze utakmica

            string timePattern = @"^([01]\d|2[0-3]):[0-5]\d$"; // HH:mm format (24h)
            if (matchExists)
            {
                ShowToast("Match already exists on this date and time.");
                return;
            }
            else if (trainingExists)
            {
                ShowToast("A training already exists on this date and time.");
                return;
            }
            else if (string.IsNullOrEmpty(opponentTeam) || string.IsNullOrEmpty(location) || string.IsNullOrEmpty(startTime) || dtMatchDate.SelectedDate == null)
            {
                ShowToast("Please fill in all fields.");
                return;
            }
            else if (matchDate < DateTime.Now)
            {
                ShowToast("Match date cannot be in the past.");
                return;
            }
            else if (!Regex.IsMatch(startTime, timePattern))
            {
                ShowToast("Invalid time format. Please enter time in HH:mm format.");
                return;
            }
            else
            {
                try
                {
                    var match = new EntitiesLayer.Entities.Match
                    {
                        TeamID = teamId,
                        MatchDate = matchDate,
                        OpponentTeam = opponentTeam,
                        Location = location,
                        StartTime = startTimeParsed,
                        Status = "Scheduled"
                    };
                    _matchManagementService.AddMatch(match);
                }
                catch (Exception ex)
                {
                    ShowToast("Failed to add match: " + ex.Message);
                    return;
                }
            }
            GuiManager.CloseContent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
        }
    }
}
