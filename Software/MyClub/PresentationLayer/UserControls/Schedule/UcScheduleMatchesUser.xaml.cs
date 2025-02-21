using BusinessLogicLayer.Services;
using PresentationLayer.Helper;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        private int currentMonth = DateTime.Now.Month;
        private int currentYear = DateTime.Now.Year;
        private int totalMatches = 0;

        public UcScheduleMatchesUser()
        {
            InitializeComponent();
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            statsGrid.Visibility = Visibility.Collapsed;
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

            foreach (var item in fetchedMatches)
            {
                if (item.Status != "Scheduled" && item.Status != "Cancelled")
                {
                    totalMatches++;
                }
            }

            FilterMatchesByMonth();
        }

        private void FilterMatchesByMonth()
        {
            var matches = _matchManagementService.FilterMatchesForMonth(currentYear, currentMonth);
            dgMatchGrid.ItemsSource = matches;
            lblCurrentMonth.Visibility = Visibility.Visible;
            lblCurrentMonth.Content = $"{new DateTime(currentYear, currentMonth, 1):MMMM yyyy}";
            btnPreviousMonth.Visibility = Visibility.Visible;
            btnNextMonth.Visibility = Visibility.Visible;
            lblDgHeader.Content = "Matches in " + currentMonth + "." + currentYear + ".";
        }

        private void btnFilterMatches_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = dpFilterStartDate.SelectedDate;
            DateTime? endDate = dpFilterEndDate.SelectedDate;
            string selectedStatus = cbFilterStatus.SelectedValue.ToString();

            if((startDate.HasValue && endDate.HasValue) || (selectedStatus != "- Select a status -"))
            {
                if (startDate.HasValue && startDate.Value > endDate.Value)
                {
                    ShowToast("Start date cannot be greater than end date.");
                    return;
                }


                var filteredMatches = _matchManagementService.FilterMatches(startDate, endDate, selectedStatus);

                if (filteredMatches.Count == 0)
                {
                    ShowToast("There are no data to be shown.");
                    return;
                }

                lblCurrentMonth.Visibility = Visibility.Collapsed;
                btnPreviousMonth.Visibility = Visibility.Collapsed;
                btnNextMonth.Visibility = Visibility.Collapsed;

                if(startDate.HasValue && endDate.HasValue)
                {
                    lblDgHeader.Content = $"Filtered matches from {startDate.Value:dd.MM.yyyy} to {endDate.Value:dd.MM.yyyy}";
                    statsGrid.Visibility = Visibility.Collapsed;
                }
                else if (selectedStatus != "- Select a status -")
                {
                    lblDgHeader.Content = $"Filtered matches with status: {selectedStatus}";
                    if(selectedStatus != "Scheduled" && selectedStatus != "Cancelled")
                    {
                        statsGrid.Visibility = Visibility.Visible;
                        txtTotalEvents.Text = totalMatches.ToString();
                        txtFilterStatus.Text = "Number of " + selectedStatus + "s";
                        txtTotalFilteredEvents.Text = filteredMatches.Count.ToString();
                        double result = (double)filteredMatches.Count / totalMatches * 100;
                        txtAverageRate.Text = result.ToString() + "%";
                    }
                    else
                    {
                        statsGrid.Visibility = Visibility.Collapsed;
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

        private void btnPreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            currentMonth = (currentMonth == 1) ? 12 : currentMonth - 1;
            if (currentMonth == 12) currentYear--;
            FilterMatchesByMonth();
        }

        private void btnNextMonth_Click(object sender, RoutedEventArgs e)
        {
            currentMonth = (currentMonth == 12) ? 1 : currentMonth + 1;
            if (currentMonth == 1) currentYear++;
            FilterMatchesByMonth();
        }

        public void btnMarkAttendance_Click(object sender, RoutedEventArgs e)
        {
            if (dgMatchGrid.SelectedItem is Match selectedMatch)
            {
                if (DateTime.Now < selectedMatch.MatchDate)
                {
                    GuiManager.OpenContent(new UcMarkAttendance(0, selectedMatch.MatchID, null, selectedMatch));
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


        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            statsGrid.Visibility = Visibility.Collapsed;
            currentMonth = DateTime.Now.Month;
            currentYear = DateTime.Now.Year;
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
