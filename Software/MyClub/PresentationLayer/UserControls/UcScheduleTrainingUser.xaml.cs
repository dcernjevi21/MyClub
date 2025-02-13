using BusinessLogicLayer;
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
    public partial class UcScheduleTrainingUser : UserControl
    {
        private MatchManagementService _matchManagementService = new MatchManagementService();
        private TrainingService _trainingService = new TrainingService();
        private int teamId = CurrentUser.User.TeamID.GetValueOrDefault();

        public UcScheduleTrainingUser()
        {
            InitializeComponent();
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTrainings();
        }

        public void LoadTrainings()
        {
            dgTrainingGrid.ItemsSource = _trainingService.GetTrainingsForTeam(teamId); 
        }
       
        private void btnFilterTrainings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            LoadTrainings();
        }

        public void btnMarkAttendance_Click(object sender, RoutedEventArgs e)
        {
            Training training = GetTrainingAttendance();
            if (training != null)
            {
                if (DateTime.Now < training.TrainingDate)
                {
                    GuiManager.OpenContent(new UcMarkAttendance(training.TrainingID, 0, training, null));
                }
                else
                {
                    MessageBox.Show("Cannot mark attendance for past training sessions.");
                }
            }
            else
            {
                ShowToast("Please select a training to mark attendance for.");
            }
        }

        public Training GetTrainingAttendance()
        {
            return dgTrainingGrid.SelectedItem as Training;
        }

        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }
    }
}
