using PresentationLayer.UserControls;
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
using System.Windows.Shapes;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for CoachWindow.xaml
    /// </summary>
    public partial class CoachWindow : Window
    {
        public CoachWindow()
        {
            InitializeComponent();
            GuiManager.SetMainWindow(this);
        }

        private void btnMatchManagement_Click(object sender, RoutedEventArgs e)
        {
            UcMatchManagement ucMatchManagement = new UcMatchManagement();
            contentPanel.Content = ucMatchManagement;
        }

        private void btnMTrainings_Click(object sender, RoutedEventArgs e)
        {
            UcTrainingsCoach ucTrainings = new UcTrainingsCoach();
            contentPanel.Content = ucTrainings;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.Logout();
        }

        private void btnAttendanceReport_Click(object sender, RoutedEventArgs e)
        {
            UcAttendanceReport ucAttendance = new UcAttendanceReport();
            contentPanel.Content = ucAttendance;
        }

        private void btnPlayerEvaluation_Click(object sender, RoutedEventArgs e)
        {
            UCCoachEvaluation ucCoachEvaluation = new UCCoachEvaluation();
            contentPanel.Content = ucCoachEvaluation;
        }
    }
}
