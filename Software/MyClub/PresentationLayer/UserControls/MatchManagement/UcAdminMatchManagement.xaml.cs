using BusinessLogicLayer.Services;
using PresentationLayer.Helper;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UcMatchManagement.xaml
    /// </summary>
    public partial class UcAdminMatchManagement : UserControl
    {
        private MatchManagementService _matchManagementService = new MatchManagementService();

        public UcAdminMatchManagement()
        {
            InitializeComponent();
        }

        public async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadMatches();
        }

        public async Task LoadMatches()
        {
            var fetchedMatches = await _matchManagementService.GetMatches();
            if (fetchedMatches == null || fetchedMatches.Count == 0)
            {
                MessageBox.Show("There are no data to be shown.");
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
                ShowToast("You cannot add matches if you're not party of a team");
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
                    _ = LoadMatches();
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
            EntitiesLayer.Entities.Match match = GetMatch();
            if (match != null)
            {
                GuiManager.OpenContent(new UcCancelMatch(match));
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

        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }
    }
}
