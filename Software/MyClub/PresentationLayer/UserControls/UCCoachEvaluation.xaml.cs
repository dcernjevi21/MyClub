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
        private List<User> _athletes;
        private List<AthleteEvaluation> _currentEvaluations;

        public UCCoachEvaluation()
        {
            InitializeComponent();
            _coachEvaluationService = new CoachEvaluationService();
            _evaluationService = new AthleteEvaluationService();
            LoadAthletes();
            cmbAthletes.SelectionChanged += cmbAthletes_SelectionChanged;
            sldRating.ValueChanged += (s, e) =>
            {
                txtRatingValue.Text = sldRating.Value.ToString("0");
            };
        }

        private void LoadAthletes()
        {
            if (CurrentUser.User != null)
            {
                int teamId = (int)CurrentUser.User.TeamID;
                _athletes = _coachEvaluationService.GetPlayersForTeam(teamId);
                cmbAthletes.ItemsSource = _athletes;
                cmbAthletes.DisplayMemberPath = "FirstName";
                if (_athletes.Any())
                    cmbAthletes.SelectedIndex = 0;
            }
            else
            {
                SetStatus("No coach logged in.", true);
            }
        }

        private void cmbAthletes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAthletes.SelectedItem is User selectedAthlete)
            {
                LoadEvaluations(selectedAthlete.UserID);
            }
        }

        private void LoadEvaluations(int userId)
        {
            try
            {
                _currentEvaluations = _evaluationService.GetEvaluationsForAthlete(userId);
                dataGridEvaluations.ItemsSource = _currentEvaluations;
                SetStatus("Evaluations loaded successfully.");
            }
            catch (Exception ex)
            {
                SetStatus($"Error loading evaluations: {ex.Message}", true);
            }
        }

        private void btnAddEvaluation_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAthletes.SelectedItem is User selectedAthlete)
            {
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
                    SetStatus("Evaluation added successfully.");
                    LoadEvaluations(selectedAthlete.UserID);
                    ClearEvaluationInput();
                }
                else
                {
                    SetStatus("Failed to add evaluation.", true);
                }
            }
            else
            {
                SetStatus("Please select an athlete.", true);
            }
        }

        private void btnModifyEvaluation_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridEvaluations.SelectedItem is AthleteEvaluation selectedEvaluation)
            {
                decimal rating = (decimal)sldRating.Value;
                selectedEvaluation.Mark = rating;
                selectedEvaluation.Comment = txtComment.Text.Trim();
                bool success = _evaluationService.UpdateEvaluation(selectedEvaluation);
                if (success)
                {
                    SetStatus("Evaluation modified successfully.");
                    if (cmbAthletes.SelectedItem is User selectedAthlete)
                        LoadEvaluations(selectedAthlete.UserID);
                    ClearEvaluationInput();
                }
                else
                {
                    SetStatus("Failed to modify evaluation.", true);
                }
            }
            else
            {
                SetStatus("Please select an evaluation to modify.", true);
            }
        }

        private void btnDeleteEvaluation_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridEvaluations.SelectedItem is AthleteEvaluation selectedEvaluation)
            {
                bool success = _evaluationService.DeleteEvaluation(selectedEvaluation);
                if (success)
                {
                    SetStatus("Evaluation deleted successfully.");
                    if (cmbAthletes.SelectedItem is User selectedAthlete)
                        LoadEvaluations(selectedAthlete.UserID);
                }
                else
                {
                    SetStatus("Failed to delete evaluation.", true);
                }
            }
            else
            {
                SetStatus("Please select an evaluation to delete.", true);
            }
        }

        private void ClearEvaluationInput()
        {
            sldRating.Value = 5;
            txtComment.Clear();
        }

        private void SetStatus(string message, bool isError = false)
        {
            lblStatus.Text = message;
            lblStatus.Foreground = isError ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.DarkBlue;
            lblStatus.Visibility = string.IsNullOrEmpty(message) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}