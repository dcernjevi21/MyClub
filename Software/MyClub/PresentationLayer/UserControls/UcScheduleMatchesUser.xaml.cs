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
            await _matchManagementService.GetMatchesByTeamId(teamId);
            UpdateMatchesDisplay();
        }

        private void UpdateMatchesDisplay()
        {
            var matches = _matchManagementService.GetMatchesForMonth(currentYear, currentMonth);
            dgMatchGrid.ItemsSource = matches;
            lblCurrentMonth.Visibility = Visibility.Visible;
            lblCurrentMonth.Content = $"{new DateTime(currentYear, currentMonth, 1):MMMM yyyy}";
            btnPreviousMonth.Visibility = Visibility.Visible;
            btnNextMonth.Visibility = Visibility.Visible;
            lblDgHeader.Content = "";
        }

        private void btnFilterMatches_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = dpFilterStartDate.SelectedDate;
            DateTime? endDate = dpFilterEndDate.SelectedDate;
            string selectedStatus = cbFilterStatus.SelectedValue.ToString();

            if((startDate.HasValue && endDate.HasValue) || (selectedStatus != "- Select a status -"))
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

                if(startDate.HasValue && endDate.HasValue)
                {
                    lblDgHeader.Content = $"Filtered matches from {startDate.Value:dd.MM.yyyy} to {endDate.Value:dd.MM.yyyy}";
                }
                else if (selectedStatus != "- Select a status -")
                {
                    lblDgHeader.Content = $"Filtered matches with status: {selectedStatus}";
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
            UpdateMatchesDisplay();
        }

        private void btnNextMonth_Click(object sender, RoutedEventArgs e)
        {
            currentMonth = (currentMonth == 12) ? 1 : currentMonth + 1;
            if (currentMonth == 1) currentYear++;
            UpdateMatchesDisplay();
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
            _ = LoadMatches();
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
