using BusinessLogicLayer.Services;
using DataAccessLayer;
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
    /// Interaction logic for UCCoachEvaluation.xaml
    /// </summary>
    public partial class UCCoachEvaluation : UserControl
    {
        private readonly CoachEvaluationService _coachEvaluationService;
        private readonly AthleteEvaluationService _evaluationService;

        public UCCoachEvaluation()
        {
            InitializeComponent();
            _coachEvaluationService = new CoachEvaluationService();
            _evaluationService = new AthleteEvaluationService();
            LoadAthletes();
            sldRating.ValueChanged += (s, e) =>
            {
                txtRatingValue.Text = sldRating.Value.ToString("0");
            };
        }

        private void LoadAthletes()
        {
            // Use the current coach's team ID from the presentation layer
            if (CurrentUser.User == null)
            {
                cmbAthletes.ItemsSource = new List<User>();
                return;
            }
            int teamId = (int)CurrentUser.User.TeamID;
            var players = _coachEvaluationService.GetPlayersForTeam(teamId);
            cmbAthletes.ItemsSource = players;
            cmbAthletes.DisplayMemberPath = "FirstName"; // Optionally use a full name property
            if (players.Any())
                cmbAthletes.SelectedIndex = 0;
        }

        private void btnSubmitEvaluation_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAthletes.SelectedItem is User selectedAthlete)
            {
                // Slider ensures rating is between 1 and 10.
                decimal rating = (decimal)sldRating.Value;

                AthleteEvaluation evaluation = new AthleteEvaluation
                {
                    UserID = selectedAthlete.UserID,
                    Mark = rating,
                    Comment = txtComment.Text.Trim(),
                    Date = DateTime.Now
                };

                bool success = _evaluationService.AddEvaluation(evaluation);
                if (success)
                {
                    SetStatus("Evaluation submitted successfully.");
                    // Optionally clear the fields
                    sldRating.Value = 5;
                    txtComment.Clear();
                }
                else
                {
                    SetStatus("Failed to submit evaluation.", true);
                }
            }
            else
            {
                SetStatus("Please select an athlete.", true);
            }
        }

        private void SetStatus(string message, bool isError = false)
        {
            lblStatus.Text = message;
            lblStatus.Foreground = isError ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.DarkBlue;
            lblStatus.Visibility = string.IsNullOrEmpty(message) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}