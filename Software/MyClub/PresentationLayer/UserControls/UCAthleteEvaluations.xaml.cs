using BusinessLogicLayer.Services;
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
    /// Interaction logic for UCAthleteEvaluations.xaml
    /// </summary>
    public partial class UCAthleteEvaluations : UserControl
    {
        private readonly AthleteEvaluationService _evaluationService;

        public UCAthleteEvaluations()
        {
            InitializeComponent();
            _evaluationService = new AthleteEvaluationService();
            LoadEvaluations();
        }
    private void LoadEvaluations()
        {
            try
            {
                if (CurrentUser.User != null)
                {
                    var evaluations = _evaluationService.GetEvaluationsForAthlete(CurrentUser.User.UserID);
                    dataGridEvaluations.ItemsSource = evaluations;
                    SetStatus("Evaluations loaded successfully.");
                }
                else
                {
                    SetStatus("No logged-in user found.", true);
                }
            }
            catch (Exception ex)
            {
                SetStatus($"Error loading evaluations: {ex.Message}", true);
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