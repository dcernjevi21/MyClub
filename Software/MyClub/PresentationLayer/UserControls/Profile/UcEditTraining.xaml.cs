using BusinessLogicLayer;
using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UcEditTraining.xaml
    /// </summary>
    
    //Valec kompletno
    public partial class UcEditTraining : UserControl
    {
        private Training training;
        private List<Team> teams;
        public event EventHandler<bool> TrainingUpdated;
        public UcEditTraining(Training selectedTraining)
        {
            InitializeComponent();
            training = selectedTraining;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (training == null)
            {
                return;
            }

            dpDate.SelectedDate = training.TrainingDate;
            tbStartTime.Text = training.StartTime.ToString(@"hh\:mm\:ss");
            tbEndTime.Text = training.EndTime.ToString(@"hh\:mm\:ss");

            LoadTeams();
        }

        private void LoadTeams()
        {
            if (CurrentUser.User.RoleID == 2)
            {
                cbTeam.Visibility = Visibility.Collapsed;
                lblTeam.Visibility = Visibility.Collapsed;
            }
            else
            {
                var teamServices = new TeamService();
                teams = teamServices.GetTeams();

                cbTeam.ItemsSource = teams;
                cbTeam.DisplayMemberPath = "Name";
                cbTeam.SelectedValuePath = "TeamID";

                SelectTeam(training.TeamID);
            }
           
        }

        private void SelectTeam(int teamID)
        {
            if (teams == null || teams.Count == 0)
                return;

            var selectedTeam = teams.FirstOrDefault(t => t.TeamID == teamID);
                
            cbTeam.SelectedItem = selectedTeam;
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var userRole = CurrentUser.User.RoleID;
            if (userRole == 1)
            {
                GuiManager.OpenContent(new UcTrainingsAdmin());
            }
            else
            {
                GuiManager.OpenContent(new UcTrainingsCoach());
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateData())
            {
                UpdateTraining();
            }
        }

        private void UpdateTraining()
        {
            training.TrainingDate = dpDate.SelectedDate ?? DateTime.Today;
            training.StartTime = TimeSpan.TryParse(tbStartTime.Text, out var startTime) ? startTime : TimeSpan.Zero;
            training.EndTime = TimeSpan.TryParse(tbEndTime.Text, out var endTime) ? endTime : TimeSpan.Zero;

            if (cbTeam.SelectedItem is Team selectedTeam)
            {
                training.TeamID = selectedTeam.TeamID;
            }
            else
            {
                training.TeamID = (int)CurrentUser.User.TeamID;
            }

            var trainingService = new TrainingService();
            bool isUpdated = trainingService.UpdateTraining(training);

            var userRole = CurrentUser.User.RoleID;

            OpenUserControl(userRole, isUpdated);
        }

        private bool ValidateData()
        {
            bool IsValid = true;
            if (dpDate.SelectedDate == null)
            {
                lblMessageDate.Content = "Please select a date!";
                IsValid = false;
            }
            else lblTeam.Content = "";

            if (!TimeSpan.TryParse(tbStartTime.Text, out var startTime))
            {
                lblMessageStartTime.Content = "Please enter a valid start time!";
                IsValid = false;
            }
            else lblMessageStartTime.Content = "";

            if (!TimeSpan.TryParse(tbEndTime.Text, out var endTime))
            {
                lblMessageEndTime.Content = "Please enter a valid end time!";
                IsValid = false;
            }
            else lblMessageEndTime.Content = "";

            if (startTime >= endTime)
            {
                lblMessageStartTime.Content = "Start time must be before end time!";
                IsValid = false;
            }
            else lblMessageStartTime.Content = "";

            if (CurrentUser.User.RoleID == 1 && cbTeam.SelectedItem == null)
            {
                lblMessageTeam.Content = "Please select a team!";
                IsValid = false;
            }
            else lblMessageTeam.Content = "";

            return IsValid;
        }

        private void OpenUserControl(int? userRole, bool isUpdated)
        {
            if (userRole == 1)
            {
                var trainingAdmin = new UcTrainingsAdmin();
                if (isUpdated)
                {
                    trainingAdmin.ShowMessage("Training successfully updated!", true);
                }
                else
                {
                    trainingAdmin.ShowMessage("Failed to update training!", false);
                }
                GuiManager.OpenContent(trainingAdmin);
            }
            else
            {
                var trainingCoach = new UcTrainingsCoach();
                if (isUpdated)
                {
                    trainingCoach.ShowMessage("Training successfully updated!", true);
                }
                else
                {
                    trainingCoach.ShowMessage("Failed to update training!", false);
                }
                GuiManager.OpenContent(trainingCoach);
            }
        }
    }
}
