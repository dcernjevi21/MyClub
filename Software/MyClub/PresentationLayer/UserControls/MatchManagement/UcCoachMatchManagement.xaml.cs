using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UcMatchManagement.xaml
    /// </summary>
    public partial class UcCoachMatchManagement : UserControl
    {
        private MatchManagementService _matchManagementService = new MatchManagementService();

        public DateTime TodayDate => DateTime.Today;
        private int currentMonth = DateTime.Now.Month;
        private int currentYear = DateTime.Now.Year;
        private int totalMatches = 0;

        public UcCoachMatchManagement()
        {
            InitializeComponent();
        }

        public async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            statsGrid.Visibility = Visibility.Collapsed;

            if (CurrentUser.User.RoleID != 2)
            {
                var attendanceColumn = dgMatchGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "Attendance");
                if (attendanceColumn != null)
                {
                    attendanceColumn.Visibility = Visibility.Collapsed;
                }
            }
            await LoadMatches();
        }

        public async Task LoadMatches()
        {
            var fetchedMatches = new List<Match>();
            if (!CurrentUser.User.TeamID.HasValue && CurrentUser.User.RoleID == 2)
            {
                ShowToast("You aren't assigned to a team.");
                return;
            }

            if (CurrentUser.User.RoleID == 1)
            {
                fetchedMatches = await _matchManagementService.GetMatches();
                
                if (fetchedMatches == null || fetchedMatches.Count == 0)
                {
                    MessageBox.Show("There are no data to be shown.");
                    return;
                }

                UpdateMatchesDisplay();
            }
            else
            {
                int teamId = (int)CurrentUser.User.TeamID;
                fetchedMatches = await _matchManagementService.GetMatchesByTeamId(teamId);
                if (fetchedMatches == null || fetchedMatches.Count == 0)
                {
                    MessageBox.Show("There are no data to be shown.");
                    return;
                }
                foreach (var item in fetchedMatches)
                {
                    var match = item;
                    if (match != null && match.MatchDate < DateTime.Now && match.Status == "Scheduled")
                    {
                        ShowToast("You have matches that have already been played. Please update the RED MARKED matches.");
                    }
                }
                UpdateMatchesDisplay();
            }

            foreach(var item in fetchedMatches)
            {
                if (item != null && item.Status != "Scheduled" && item.Status != "Cancelled")
                {
                    totalMatches++;
                }
            }
        }

        private void UpdateMatchesDisplay()
        {
            var matches = _matchManagementService.FilterMatchesForMonth(currentYear, currentMonth);
            dgMatchGrid.ItemsSource = matches;
            lblCurrentMonth.Visibility = Visibility.Visible;
            lblCurrentMonth.Content = $"{new DateTime(currentYear, currentMonth, 1):MMMM yyyy}";
            btnPreviousMonth.Visibility = Visibility.Visible;
            btnNextMonth.Visibility = Visibility.Visible;
            lblDgHeader.Content = "";
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = dpFilterStartDate.SelectedDate;
            DateTime? endDate = dpFilterEndDate.SelectedDate;
            string selectedStatus = cbFilterStatus.SelectedValue.ToString();

            if ((startDate.HasValue && endDate.HasValue) || (selectedStatus != "- Select a status -"))
            {
                var filteredMatches = _matchManagementService.FilterMatches(startDate, endDate, selectedStatus);

                if (filteredMatches.Count == 0)
                {
                    ShowToast("There are no data to be shown.");
                    return;
                }

                lblCurrentMonth.Visibility = Visibility.Collapsed;
                btnPreviousMonth.Visibility = Visibility.Collapsed;
                btnNextMonth.Visibility = Visibility.Collapsed;

                if (startDate.HasValue && endDate.HasValue)
                {
                    lblDgHeader.Content = $"Filtered matches from {startDate.Value:dd.MM.yyyy} to {endDate.Value:dd.MM.yyyy}";
                    statsGrid.Visibility = Visibility.Collapsed;
                }
                else if (selectedStatus != "- Select a status -")
                {
                    lblDgHeader.Content = $"Filtered trainings with status: {selectedStatus}";
                    if (selectedStatus != "Scheduled" || selectedStatus != "Cancelled")
                    {
                        statsGrid.Visibility = Visibility.Visible;
                        txtTotalEvents.Text = totalMatches.ToString();
                        txtFilterStatus.Text = "Number of " + selectedStatus + "s";
                        txtTotalFilteredEvents.Text = filteredMatches.Count.ToString();
                        double result = (double)filteredMatches.Count / totalMatches * 100;
                        txtAverageRate.Text = result.ToString() + "%";
                    }
                }

                dpFilterStartDate.SelectedDate = null;
                dpFilterEndDate.SelectedDate = null;
                cbFilterStatus.SelectedIndex = 0;

                dgMatchGrid.ItemsSource = filteredMatches;
            }
            else
            {
                ShowToast("Please select a date range or status to filter matches.");
                return;
            }
        }

        private void btnReloadMatches_Click(object sender, RoutedEventArgs e)
        {
            statsGrid.Visibility = Visibility.Collapsed;
            UpdateMatchesDisplay();
        }

        private void btnPreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            currentMonth = (currentMonth == 1) ? 12 : currentMonth - 1;
            if (currentMonth == 12) currentYear--;
            UpdateMatchesDisplay();
        }

        private void btnNextMonth_Click(object sender, RoutedEventArgs e)
        {
            currentMonth = (currentMonth == 12) ? 1 : currentMonth + 1;
            if (currentMonth == 1) currentYear++;
            UpdateMatchesDisplay();
        }

        public void btnAddMatch_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser.User.TeamID != null || CurrentUser.User.RoleID == 1)
            {
                GuiManager.OpenContent(new UcAddMatch());
            }
            else
            {
                ShowToast("You cannot add matches if you're not party of a team");
            }

            UpdateMatchesDisplay();
        }
        //Černjević
        public void btnUpdateMatch_Click(object sender, RoutedEventArgs e)
        {
            Match match = GetMatch();
            if (match != null)
            {
                if (match.Status == "Cancelled")
                {
                    ShowToast("Cannot update cancelled matches! Only matches that have already been played can be updated.");
                    return;
                }
                else if (match.MatchDate > DateTime.Now)
                {
                    GuiManager.OpenContent(new UcEditMatch(match));
                }
                //ako je utakmica prošla, otvori formu za unos rezultata
                else
                {
                    GuiManager.OpenContent(new UcUpdateMatch(match));
                }
                UpdateMatchesDisplay();
            }
            else
            {
                ShowToast("Please select a match.");
                return;
            }
        }
        //Černjević
        public void btnDeleteMatch_Click(object sender, RoutedEventArgs e)
        {
            Match match = GetMatch();
            if (match != null)
            {
                //prikaz poruke s potvrdom brisanja
                MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to delete this match",
                    "Confirm delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    MatchManagementService _matchManagementService = new MatchManagementService();
                    _matchManagementService.RemoveMatch(match);
                    UpdateMatchesDisplay();
                }
                else
                {
                    ShowToast("Delete cancelled.");
                }   
            }
            else
            {
                ShowToast("Please select a match.");
            }
        }
        //Černjević
        public void btnCancelMatch_Click(object sender, RoutedEventArgs e)
        {
            Match match = GetMatch();
            if (match != null)
            {
                GuiManager.OpenContent(new UcCancelMatch(match));
            }
            else
            {
                ShowToast("Please select a match.");
            }
        }

        public Match GetMatch()
        {
            if (dgMatchGrid.SelectedItem == null)
            {
                return null;
            }
            else
            {
                return dgMatchGrid.SelectedItem as Match;
            }
        }

        //Valec
        private void btnAttendance_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var match = button.DataContext as Match;
            if (match != null)
            {
                var attendanceControl = new UcMatchAttendanceCoach(match);
                GuiManager.OpenContent(attendanceControl);
            }
        }

        public void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }
    }
}
