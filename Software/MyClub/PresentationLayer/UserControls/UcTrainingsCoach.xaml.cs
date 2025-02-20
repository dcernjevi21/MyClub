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
    
    //Valec kompletno
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
                var confirmDialog = new UcDeleteConfirmation();
                var window = new Window
                {
                    Content = confirmDialog,
                    WindowStyle = WindowStyle.None,
                    ResizeMode = ResizeMode.NoResize,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    SizeToContent = SizeToContent.WidthAndHeight
                };

                confirmDialog.DeleteConfirmed += (s, confirmed) =>
                {
                    if (confirmed)
                    {
                        bool isSuccessful = services.RemoveTraining(selectedTraining);
                        if (isSuccessful)
                        {
                            ShowMessage("Training successfully deleted!");
                            ShowAllTrainings();
                        }
                        else ShowMessage("Failed to remove training!", false);
                    }
                };

                window.ShowDialog();
            }
            else
            {
                ShowMessage("Please select a training to remove.", false);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedTraining = dgTrainings.SelectedItem as Training;
            if (selectedTraining != null)
            {
                UcEditTraining editControl = new UcEditTraining(selectedTraining);
                GuiManager.OpenContent(editControl);
            }
            else
            {
                ShowMessage("Please select a training to update.", false);
            }
        }

        public void ShowMessage(string message, bool isSuccess = true)
        {
            lblMessage.Content = message;
            lblMessage.Foreground = isSuccess ? Brushes.Green : Brushes.Red;
            lblMessage.Visibility = Visibility.Visible;
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            timer.Tick += (s, e) =>
            {
                lblMessage.Visibility = Visibility.Collapsed;
                timer.Stop();
            };
            timer.Start();
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

        private void btnAttendance_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var training = button.DataContext as Training;
            if (training != null)
            {
                var attendanceControl = new UcTrainingAttendanceCoach(training);
                GuiManager.OpenContent(attendanceControl);
            }
        }
    }
}
