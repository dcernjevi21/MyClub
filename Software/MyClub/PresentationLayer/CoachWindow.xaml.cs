using PresentationLayer.Helper;
using PresentationLayer.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        //Valec
        private void btnMTrainings_Click(object sender, RoutedEventArgs e)
        {
            UcTrainingsCoach ucTrainings = new UcTrainingsCoach();
            contentPanel.Content = ucTrainings;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.Logout();
        }
        //Valec
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

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1) DocumentationHelper.OpenUserDocumentation();
        }

        private void CoachWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1) OpenUserDocumentation();
        }

        private void OpenUserDocumentation()
        {
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string projectPath = Directory.GetParent(basePath).Parent.Parent.FullName;
                string filePath = System.IO.Path.Combine(projectPath, "Resources", "MyClub-coach.pdf");

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("The specified file was not found.", filePath);
                }

                byte[] pdf = File.ReadAllBytes(filePath);
                using (MemoryStream ms = new MemoryStream(pdf))
                using (FileStream f = new FileStream("help-coach.pdf", FileMode.OpenOrCreate))
                {
                    ms.WriteTo(f);
                }
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
