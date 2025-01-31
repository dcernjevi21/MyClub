using BusinessLogicLayer;
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
    public partial class UcAddTraining : UserControl
    {
        public UcAddTraining()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var training = new Training
            {
                TrainingDate = dpDate.SelectedDate.Value,
                StartTime = TimeSpan.Parse(tbStartTime.Text),
                EndTime = TimeSpan.Parse(tbEndTime.Text),
                TeamID = (int)cbTeam.SelectedValue
            };

            var trainingService = new TrainingService();
            trainingService.AddTraining(training);

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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTeams();
        }

        private void LoadTeams()
        {
            var trainingServices = new TrainingService();
            var teams = trainingServices.GetTeams();

            cbTeam.ItemsSource = teams;
            cbTeam.DisplayMemberPath = "Name";
            cbTeam.SelectedValuePath = "TeamID";
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

        private void btnCancel_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
