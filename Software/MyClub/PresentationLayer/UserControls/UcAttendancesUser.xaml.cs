using BusinessLogicLayer.Services;
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
    /// Interaction logic for UcAttendancesUser.xaml
    /// </summary>
    public partial class UcAttendancesUser : UserControl
    

        public UcAttendancesUser()
        {
            InitializeComponent();
        
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTrainings();

            LoadMatches();
        }

        public void LoadTrainings()
        {
            dgTrainingGrid.ItemsSource = _trainingManagementService.GetTrainings();
        }
        public void LoadMatches()
        {
            // Load matches
        }

        public void btnMarkAttendance_Click(object sender, RoutedEventArgs e)
        {
            // Mark attendance
        }
    }
}
