using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
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

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UcAttendancesUser.xaml
    /// </summary>
    /// 
    ///Černjević kompletno
    public partial class UcAttendancesUser : UserControl
    {
        private MatchManagementService _matchManagementService = new MatchManagementService();
        private int teamId = CurrentUser.User.TeamID.Value;

        public UcAttendancesUser()
        {
            InitializeComponent();
        
        }

        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTrainings();

            LoadMatches();
        }

        public void LoadTrainings()
        {
            //dgTrainingGrid.ItemsSource = _trainingManagementService.GetTrainingsByTeamId((int)CurrentUser.User.TeamID);
        }
        public void LoadMatches()
        {
            dgMatchGrid.ItemsSource = _matchManagementService.GetMatchesByTeamId((int)CurrentUser.User.TeamID);
        }

        public void btnMarkAttendance_Click(object sender, RoutedEventArgs e)
        {
            Match match = GetMatchAttendance();
            Training training = GetTrainingAttendance();
            if (match == null && training != null)
            {
                GuiManager.OpenContent(new UcMarkAttendance(training.TrainingID, 0, training, null));
            }
            else if (match != null && training == null)
            {
                GuiManager.OpenContent(new UcMarkAttendance(0, match.MatchID, null, match));
            }
            else
            {
                ShowToast("Please select a match or training to mark attendance for.");
            }

        }

        public Match GetMatchAttendance()
        {
            return dgMatchGrid.SelectedItem as Match;
        }
        public Training GetTrainingAttendance()
        {
            return dgTrainingGrid.SelectedItem as Training;
        }
    }
}
