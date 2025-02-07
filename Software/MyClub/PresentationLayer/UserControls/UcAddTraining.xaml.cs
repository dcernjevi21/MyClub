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
    /// Interaction logic for UcAddTraining.xaml
    /// </summary>

    //Valec kompletno
    public partial class UcAddTraining : UserControl
    {
        public UcAddTraining()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateData())
            {
                var training = new Training
                {
                    TrainingDate = dpDate.SelectedDate.Value,
                    StartTime = TimeSpan.Parse(tbStartTime.Text),
                    EndTime = TimeSpan.Parse(tbEndTime.Text),
                    TeamID = (int)cbTeam.SelectedValue
                };

                var trainingService = new TrainingService();
                bool IsAdded = trainingService.AddTraining(training);

                var userRole = CurrentUser.User.RoleID;

                OpenUserControl(userRole, IsAdded);
            }
        }

        private void OpenUserControl(int? userRole, bool IsAdded)
        {
            if (userRole == 1)
            {
                var trainingAdmin = new UcTrainingsAdmin();
                if (IsAdded)
                {
                    trainingAdmin.ShowMessage("Training successfully added!", true);
                }
                else
                {
                    trainingAdmin.ShowMessage("Failed to add training!", false);
                }
                GuiManager.OpenContent(trainingAdmin);
            }
            else
            {
                var trainingCoach = new UcTrainingsCoach();
                if (IsAdded)
                {
                    trainingCoach.ShowMessage("Training successfully added!", true);
                }
                else
                {
                    trainingCoach.ShowMessage("Failed to add training!", false);
                }
                GuiManager.OpenContent(trainingCoach);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTeams();
        }

        private void LoadTeams()
        {
            var teamsServices = new TeamService();
            var teams = teamsServices.GetTeams();

            if (CurrentUser.User.RoleID == 2)
            {
                teams = teams.Where(t => t.TeamID == CurrentUser.User.TeamID).ToList();
                cbTeam.ItemsSource = teams;
                cbTeam.DisplayMemberPath = "Name";
                cbTeam.SelectedValuePath = "TeamID";
                cbTeam.IsEnabled = false;
                cbTeam.SelectedIndex = 0;
            }
            else
            {
                cbTeam.ItemsSource = teams;
                cbTeam.DisplayMemberPath = "Name";
                cbTeam.SelectedValuePath = "TeamID";
            }
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

        private bool ValidateData()
        {
            bool isValid = true;
            if (dpDate.SelectedDate == null)
            {
                lblMessageDate.Content = "Please select date!";
                isValid = false;
            }
            else lblMessageDate.Content = "";

            if (tbStartTime.Text == "")
            {
                lblMessageStartTime.Content = "Please select start time!";
                isValid = false;
            }
            else lblMessageStartTime.Content = "";

            if (tbEndTime.Text == "")
            {
                lblMessageEndTime.Content = "Please select end time!";
                isValid = false;
            }
            else lblMessageEndTime.Content = "";

            if (cbTeam.SelectedValue == null)
            {
                lblMessageTeam.Content = "Please select team!";
                isValid = false;
            }
            else lblMessageTeam.Content = "";

            return isValid;
        }
    }
}
