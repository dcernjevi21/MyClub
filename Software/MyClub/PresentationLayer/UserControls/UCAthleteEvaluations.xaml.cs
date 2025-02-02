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
using LiveCharts;
using LiveCharts.Wpf;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCAthleteEvaluations.xaml
    /// </summary>
    public partial class UCAthleteEvaluations : UserControl
    {
        private readonly AthleteEvaluationService _evaluationService;
        public Func<double, string> DateFormatter { get; set; }


        public UCAthleteEvaluations()
        {
            InitializeComponent();
            _evaluationService = new AthleteEvaluationService();
            DateFormatter = value => new DateTime((long)value).ToString("d");
            DataContext = this;
            LoadEvaluations();
            LoadChart();
        }

        private void LoadEvaluations()
        {
            try
            {
                if (CurrentUser.User != null)
                {
                    var evaluations = _evaluationService.GetEvaluationsForAthlete(CurrentUser.User.UserID).ToList();
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

        private void LoadChart()
        {
            try
            {
                if (CurrentUser.User != null)
                {
                    var evaluations = _evaluationService.GetEvaluationsForAthlete(CurrentUser.User.UserID).OrderBy(e => e.Date).ToList();
                    if (evaluations.Any())
                    {
                        var marks = evaluations.Select(e => (double)e.Mark).ToArray();
                        var dates = evaluations.Select(e => e.Date.Ticks).ToArray();

                        chartMarks.Series = new SeriesCollection
                        {
                            new LineSeries
                            {
                                Title = "Mark",
                                Values = new ChartValues<double>(marks),
                                PointGeometry = DefaultGeometries.Circle,
                                PointGeometrySize = 10
                            }
                        };


                        chartMarks.AxisX.Clear();
                        chartMarks.AxisX.Add(new Axis
                        {
                            Title = "Date",
                            Labels = evaluations.Select(e => e.Date.ToShortDateString()).ToList()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                SetStatus($"Error loading chart: {ex.Message}", true);
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