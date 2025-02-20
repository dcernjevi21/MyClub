using System;
using BusinessLogicLayer;
using System.Windows;
using System.Windows.Controls;
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
            cmbTeams.IsEnabled = CurrentUser.User?.RoleID != 2;
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

            string timePattern = @"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$";

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
            
            if (matchDate < DateTime.Now || matchDate == DateTime.Now)
            {
                ShowToast("Match date cannot be in the past or today.");
                return;
            }
            if (!Regex.IsMatch(startTime, timePattern))
            {
                ShowToast("Invalid time format. Please enter time in HH:mm format.");
                return;
            }

            bool matchExists = _matchManagementService.DoesMatchExist(teamId, matchDate, startTimeParsed);
            bool trainingExists = _trainingService.DoesTrainingExist(teamId, matchDate, startTimeParsed);

            //dodati slanje e maila korisnicima nakon sto se zakaze utakmica

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

        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }
    }
}
