using BusinessLogicLayer;
using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UcEditTraining.xaml
    /// </summary>
    public partial class UcEditTraining : UserControl
    {
        private Training training;
        private List<Team> teams;

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
            var teamServices = new TeamService();
            teams = teamServices.GetTeams();

            cbTeam.ItemsSource = teams;
            cbTeam.DisplayMemberPath = "Name";
            cbTeam.SelectedValuePath = "TeamID";

            SelectTeam(training.TeamID);
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
            if (cbTeam.SelectedItem is Team selectedTeam)
            {
                training.TrainingDate = dpDate.SelectedDate ?? DateTime.Today;
                training.StartTime = TimeSpan.TryParse(tbStartTime.Text, out var startTime) ? startTime : TimeSpan.Zero;
                training.EndTime = TimeSpan.TryParse(tbEndTime.Text, out var endTime) ? endTime : TimeSpan.Zero;
                training.TeamID = selectedTeam.TeamID;

                var trainingService = new TrainingService();
                bool isUpdated = trainingService.UpdateTraining(training);

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
        }


        private int getSelectedTeamID()
        {
            if (cbTeam.SelectedValue == null)
            {
                MessageBox.Show("Please select a team.");
                return -1;
            }

            return (int)cbTeam.SelectedValue;
        }
    }
}
