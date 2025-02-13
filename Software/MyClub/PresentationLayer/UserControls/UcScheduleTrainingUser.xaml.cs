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
        private TrainingService _trainingService = new TrainingService();
        private int teamId = CurrentUser.User.TeamID.GetValueOrDefault();

        private List<Training> allTrainings = new List<Training>(); //Cache 
        private int currentMonth = DateTime.Now.Month;
        private int currentYear = DateTime.Now.Year;

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
            var fetchedTrainings = _trainingService.GetTrainingsForTeam(teamId);
            if (fetchedTrainings == null || fetchedTrainings.Count == 0)
            {
                MessageBox.Show("There are no data to be shown.");
                return;
            }

            allTrainings = fetchedTrainings;
            FilterTrainingsByMonth();
        }
        private void FilterTrainingsByMonth()
        {
            var filteredTrainings = allTrainings
                .Where(m => m.TrainingDate.Year == currentYear && m.TrainingDate.Month == currentMonth).OrderBy(t => t.TrainingDate).ToList();

            dgTrainingGrid.ItemsSource = filteredTrainings;

            lblCurrentMonth.Content = $"{new DateTime(currentYear, currentMonth, 1):MMMM yyyy}";
        }

        private void btnFilterTrainings_Click(object sender, RoutedEventArgs e)
        {
            if (dpFilterStartDate.SelectedDate != null && dpFilterEndDate.SelectedDate != null)
            {
                //fetchedTrainings = _trainingService.GetTrainingsByDate(teamId, dpFilterStartDate.SelectedDate.Value, dpFilterEndDate.SelectedDate.Value);
            }
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
            FilterTrainingsByMonth();
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
            FilterTrainingsByMonth();
        }

        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }
    }
}
