using BusinessLogicLayer.Services;
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
    /// Interaction logic for UcMarkAttendance.xaml
    /// </summary>
    /// 
    ///Černjević kompletno
    public partial class UcMarkAttendance : UserControl
    {
        private Attendance attendance;
        private Attendance existingAttendance;
        private Match match;
        private Training training;
        AttendanceService _attendanceService = new AttendanceService();
        int trainingId;
        int matchId;

        public UcMarkAttendance(int trainingId, int matchId, Training training, Match match)
        {
            InitializeComponent();
            this.trainingId = trainingId;
            this.matchId = matchId;
            this.match = match;
            this.training = training;
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lblDate.Content = training != null ? "Date: " + training.TrainingDate.ToShortDateString() : "Date: " + match.MatchDate.ToShortDateString();
            lblStartTime.Content = training != null ? "Start time: " + training.StartTime.ToString() : "Start time: " + match.StartTime.ToString();
        }

        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }

        public void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (trainingId == 0 && matchId == 0)
            {
                ShowToast("Error! Please reopen form.");
                GuiManager.CloseContent();
            }

            attendance = new Attendance();
            existingAttendance = new Attendance();
            string selectedStatus = (cbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (selectedStatus == "+")
            {
                attendance.StatusID = (int)UserStatus.Present;
            }
            else if (selectedStatus == "-")
            {
                attendance.StatusID = (int)UserStatus.Absent;
            }
            else
            {
                ShowToast("Please select status.");
                return;
            }

            attendance.TrainingID = trainingId != 0 ? trainingId : (int?)null;
            attendance.MatchId = matchId != 0 ? matchId : (int?)null;
            attendance.UserID = CurrentUser.User.UserID;
            //baca error ak vec postoji attendance u bazi
            existingAttendance = _attendanceService.GetAttendances().FirstOrDefault(x => x.UserID == CurrentUser.User.UserID);
            if (existingAttendance != null)
            {
                _attendanceService.UpdateAttendance(attendance);
            }
            else 
            {
                _attendanceService.AddAttendance(attendance);
            }
            GuiManager.CloseContent();
        }

        public void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
        }
    }
}
