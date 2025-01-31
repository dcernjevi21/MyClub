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
    /// Interaction logic for UcTrainingsCoach.xaml
    /// </summary>
    public partial class UcTrainingsCoach : UserControl
    {
        private TrainingService services = new TrainingService();
        public UcTrainingsCoach()
        {
            InitializeComponent();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var selectedTraining = dgTrainings.SelectedItem as Training;
            if (selectedTraining != null)
            {
                bool isSuccessful = services.RemoveTraining(selectedTraining);
                GuiManager.OpenContent(new UcTrainingsAdmin());
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedTraining = dgTrainings.SelectedItem as Training;
            UcEditTraining editControl = new UcEditTraining(selectedTraining);
            GuiManager.OpenContent(editControl);
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcAddTraining());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ShowAllTrainings();
        }

        private void ShowAllTrainings()
        {
            var teamId = (int)CurrentUser.User.TeamID;
            var trainings = services.GetTrainingsForTeam(teamId);
            dgTrainings.ItemsSource = trainings;
        }
    }
}
