using BusinessLogicLayer;
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
    /// Interaction logic for UcAttendancesUser.xaml
    /// </summary>
    /// 
    ///Černjević kompletno
    public partial class UcScheduleTrainingUser : UserControl
    {
        private TrainingService _trainingService = new TrainingService();
        private int teamId = CurrentUser.User.TeamID.GetValueOrDefault();

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

            FilterTrainingsByMonth();
        }
        private void FilterTrainingsByMonth()
        {
            var trainings = _trainingService.GetTrainingsForMonth(currentYear, currentMonth);
            dgTrainingGrid.ItemsSource = trainings;

            lblCurrentMonth.Content = $"{new DateTime(currentYear, currentMonth, 1):MMMM yyyy}";

            lblCurrentMonth.Visibility = Visibility.Visible;
            btnPreviousMonth.Visibility = Visibility.Visible;
            btnNextMonth.Visibility = Visibility.Visible;
            lblDgHeader.Content = "Trainings in " + currentMonth + "." + currentYear + ".";
        }

        private void btnFilterTrainings_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = dpFilterStartDate.SelectedDate;
            DateTime? endDate = dpFilterEndDate.SelectedDate;
            //string selectedStatus = cbFilterStatus.SelectedValue.ToString();

            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate.Value > endDate.Value)
                {
                    ShowToast("Start date cannot be greater than end date.");
                    return;
                }
                else
                {
                    var filteredMatches = _trainingService.FilterTrainings(startDate, endDate);

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
                        lblDgHeader.Content = $"Filtered trainings from {startDate.Value:dd.MM.yyyy} to {endDate.Value:dd.MM.yyyy}";
                    }

                    dpFilterStartDate.SelectedDate = null;
                    dpFilterEndDate.SelectedDate = null;

                    dgTrainingGrid.ItemsSource = filteredMatches;
                }
            }
            else
            {
                ShowToast("Please select a date range or status to filter matches.");
                return;
            }
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            currentMonth = DateTime.Now.Month;
            currentYear = DateTime.Now.Year;
            FilterTrainingsByMonth();
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
                    ShowToast("Cannot mark attendance for past training sessions.");
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
