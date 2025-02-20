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
    /// Interaction logic for UcTrainingAttendanceCoach.xaml
    /// </summary>

    //Valec kompletno
    public partial class UcTrainingAttendanceCoach : UserControl
    {
        private readonly Training _training;
        private readonly AttendanceService _attendanceService = new AttendanceService();
        private readonly UserService _userService = new UserService();
        private List<AttendanceViewModel> _attendanceData;
        public UcTrainingAttendanceCoach(Training training)
        {
            InitializeComponent();
            _training = training;
            LoadHeaderInfo();
            InitializeStatusComboBox();
            LoadAttendanceData();
        }

        private void LoadHeaderInfo()
        {
            txtDate.Text = _training.TrainingDate.ToShortDateString();
            txtTime.Text = $"{_training.StartTime} - {_training.EndTime}";
        }

        private void InitializeStatusComboBox()
        {
            var statusColumn = (DataGridComboBoxColumn)dgAttendance.Columns.FirstOrDefault(c => c.Header.ToString() == "Status");
            if (statusColumn != null)
            {
                var statusItems = new List<KeyValuePair<int, string>>
                {
                    new KeyValuePair<int, string>(4, "Present"),
                    new KeyValuePair<int, string>(5, "Absent"),
                    new KeyValuePair<int, string>(6, "Excused")
                };

                statusColumn.ItemsSource = statusItems;
                statusColumn.SelectedValueBinding = new Binding("StatusID");
                statusColumn.DisplayMemberPath = "Value";
                statusColumn.SelectedValuePath = "Key";
            }
        }

        private void LoadAttendanceData()
        {
            var teamUsers = _userService.GetUsersFromTeam(_training.TeamID);
            var existingAttendances = _attendanceService.GetExistingTrainingAttendances(_training.TrainingID, _training.TeamID);

            _attendanceData = teamUsers.Select(user =>
            {
                var existing = existingAttendances.FirstOrDefault(a => a.UserID == user.UserID);
                return new AttendanceViewModel
                {
                    User = user,
                    StatusID = existing?.StatusID ?? 4,
                    Notes = existing?.Notes,
                    IsExistingAttendance = existing != null,
                    AttendanceID = existing?.AttendanceID
                };
            }).ToList();

            dgAttendance.ItemsSource = _attendanceData;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool isSuccess = true;
            foreach (var attendance in _attendanceData)
            {
                bool result = true;
                if (attendance.IsExistingAttendance)
                {
                    var existingAttendance = new Attendance
                    {
                        AttendanceID = attendance.AttendanceID.Value,
                        TrainingID = _training.TrainingID,
                        UserID = attendance.User.UserID,
                        StatusID = attendance.StatusID,
                        Notes = attendance.Notes
                    };

                    var originalAttendance = _attendanceService.GetExistingTrainingAttendances(_training.TrainingID, _training.TeamID)
                        .FirstOrDefault(a => a.AttendanceID == attendance.AttendanceID);

                    if (originalAttendance != null &&
                        (originalAttendance.StatusID != existingAttendance.StatusID || originalAttendance.Notes != existingAttendance.Notes))
                    {
                        result = _attendanceService.UpdateAttendance(existingAttendance);
                    }
                }
                else
                {
                    var newAttendance = new Attendance
                    {
                        TrainingID = _training.TrainingID,
                        UserID = attendance.User.UserID,
                        StatusID = attendance.StatusID,
                        Notes = attendance.Notes
                    };
                    result = _attendanceService.AddAttendance(newAttendance);
                }

                if (!result)
                {
                    isSuccess = false;
                }
            }

            var trainingsCoach = new UcTrainingsCoach();
            if (isSuccess)
            {
                trainingsCoach.ShowMessage("Attendance marked successfully!", true);
            }
            else
            {
                trainingsCoach.ShowMessage("Failed to mark attendance!", false);
            }
            GuiManager.OpenContent(trainingsCoach);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcTrainingsCoach());
        }
    }
}
