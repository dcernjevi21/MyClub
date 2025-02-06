using BusinessLogicLayer.Services;
using PresentationLayer.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
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

        public async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadMatches();
        }

        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }

        public async Task LoadMatches()
        {
            if (!CurrentUser.User.TeamID.HasValue)
            {
                ShowToast("You aren't assigned to a team.");
                return;
            }


            int teamId = (int)CurrentUser.User.TeamID;
            
            var fetchedMatches = await _matchManagementService.GetMatchesByTeamId(teamId);
            if (fetchedMatches == null || fetchedMatches.Count == 0)
            {
                MessageBox.Show("Nema dostupnih podataka za prikaz.");
                return;
            }
            dgCoachGrid.ItemsSource = fetchedMatches;
        }

        public void btnAddMatch_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser.User.TeamID != null || CurrentUser.User.RoleID == 1)
            {
                GuiManager.OpenContent(new UcAddMatch());
            }
            else
            {
                ShowToast("Ne mozete dodati utakmicu jer niste dio nekog tima");
            }
        }
        //Černjević
        public void btnUpdateMatch_Click(object sender, RoutedEventArgs e)
        {
            EntitiesLayer.Entities.Match match = GetMatch();
            if (match != null)
            {
                if (match.Status == "Cancelled")
                {
                    ShowToast("Cannot update postponed matches! Only matches that have already been played can be updated.");
                    return;
                }
                else if (match.MatchDate > DateTime.Now)
                {
                    ShowToast("Cannot update future matches! Only matches that have already been played can be updated.");

                    return;
                }
                else
                {
                    GuiManager.OpenContent(new UcUpdateMatch(match));
                }
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
            EntitiesLayer.Entities.Match match = GetMatch();
            if (match != null)
            {
                // Prikaz poruke s potvrdom brisanja
                MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to delete this match",
                    "Confirm delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                // Ako korisnik odabere 'Yes', izvršava se brisanje
                if (result == MessageBoxResult.Yes)
                {
                    MatchManagementService _matchManagementService = new MatchManagementService();
                    _matchManagementService.RemoveMatch(match);
                    LoadMatches();
                }
            }
            else
            {
                ShowToast("Please select a match.");
            }
        }
        //Černjević
        public void btnPostponeMatch_Click(object sender, RoutedEventArgs e)
        {
            EntitiesLayer.Entities.Match match = GetMatch();
            if (match != null)
            {
                GuiManager.OpenContent(new UcPostponeMatch(match));
            }
            else
            {
                ShowToast("Please select a match.");
            }
        }

        public EntitiesLayer.Entities.Match GetMatch()
        {
            if (dgCoachGrid.SelectedItem == null)
            {
                return null;
            }
            else
            {
                return dgCoachGrid.SelectedItem as EntitiesLayer.Entities.Match;
            }
        }

        //Valec
        private void btnAttendance_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var match = button.DataContext as EntitiesLayer.Entities.Match;
            if (match != null)
            {
                var attendanceControl = new UcMatchAttendanceCoach(match);
                GuiManager.OpenContent(attendanceControl);
            }
        }
    }
}
