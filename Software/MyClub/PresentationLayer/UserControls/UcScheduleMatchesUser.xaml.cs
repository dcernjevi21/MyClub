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
        private int teamId = CurrentUser.User.TeamID.GetValueOrDefault();

        private List<Match> allMatches = new List<Match>(); //Cache 
        private int currentMonth = DateTime.Now.Month;
        private int currentYear = DateTime.Now.Year;

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
            var fetchedMatches = await _matchManagementService.GetMatchesByTeamId(teamId);
            if (fetchedMatches == null || fetchedMatches.Count == 0)
            {
                MessageBox.Show("There are no data to be shown.");
                return;
            }

            allMatches = fetchedMatches;
            FilterMatchesByMonth();
        }
        private void FilterMatchesByMonth()
        {
            var filteredMatches = allMatches
                .Where(m => m.MatchDate.Year == currentYear && m.MatchDate.Month == currentMonth).OrderBy(t => t.MatchDate).ToList();

            dgMatchGrid.ItemsSource = filteredMatches;

            lblCurrentMonth.Content = $"{new DateTime(currentYear, currentMonth, 1):MMMM yyyy}"; // Postavljanje naslova
        }


        private async void btnFilterMatches_Click(object sender, RoutedEventArgs e)
        {
            List<Match> fetchedMatches = null;
            int teamId = CurrentUser.User.TeamID.Value;
            //prepraviti da ne dohvaca svaki put novo
            if (dpFilterStartDate.SelectedDate != null && dpFilterEndDate.SelectedDate != null)
            {
                fetchedMatches = await _matchManagementService.GetMatchesByDate(teamId, dpFilterStartDate.SelectedDate.Value, dpFilterEndDate.SelectedDate.Value);
            }
            else if (cbFilterStatus.SelectedValue != null)
            {
                if (cbFilterStatus.SelectedValue.ToString() == "Scheduled")
                {
                    fetchedMatches = await _matchManagementService.GetMatchesByStatus(teamId, "Scheduled");
                }
                else if (cbFilterStatus.SelectedValue.ToString() == "Cancelled")
                {
                    fetchedMatches = await _matchManagementService.GetMatchesByStatus(teamId, "Cancelled");
                }
                else if (cbFilterStatus.SelectedValue.ToString() == "Win")
                {
                    fetchedMatches = await _matchManagementService.GetMatchesByStatus(teamId, "Win");
                }
                else if (cbFilterStatus.SelectedValue.ToString() == "Draw")
                {
                    fetchedMatches = await _matchManagementService.GetMatchesByStatus(teamId, "Draw");
                }
                else if (cbFilterStatus.SelectedValue.ToString() == "Lost")
                {
                    fetchedMatches = await _matchManagementService.GetMatchesByStatus(teamId, "Lost");
                }
                else
                {
                    ShowToast("Please select a valid status to filter the matches.");
                    return;
                }
            }
            else
            {
                ShowToast("Please select a date or status to filter the matches.");
                return;
            }

            if (fetchedMatches == null || fetchedMatches.Count == 0)
            {
                MessageBox.Show("There are no data to be shown.");
                return;
            }
            else
            {
                dgMatchGrid.ItemsSource = fetchedMatches;
            }
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

        private void btnPreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            if (currentMonth == 1)
            {
                currentMonth = 12;
                currentYear--;
            }
            else
            {
                currentMonth--;
            }
            FilterMatchesByMonth();
        }

        private void btnNextMonth_Click(object sender, RoutedEventArgs e)
        {
            if (currentMonth == 12)
            {
                currentMonth = 1;
                currentYear++;
            }
            else
            {
                currentMonth++;
            }
            FilterMatchesByMonth();
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
