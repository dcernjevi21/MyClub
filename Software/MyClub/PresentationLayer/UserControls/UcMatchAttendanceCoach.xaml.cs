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
    /// Interaction logic for UcMatchAttendanceCoach.xaml
    /// </summary>

    //Valec kompletno
    public partial class UcMatchAttendanceCoach : UserControl
    {
        private readonly EntitiesLayer.Entities.Match _match;
        private readonly AttendanceService _attendanceService = new AttendanceService();
        private readonly UserService _userService = new UserService();
        private List<AttendanceViewModel> _attendanceData;

        public UcMatchAttendanceCoach(EntitiesLayer.Entities.Match match)
        {
            InitializeComponent();
            _match = match;
            LoadHeaderInfo();
            InitializeStatusComboBox();
            LoadAttendanceData();
        }

        private void LoadHeaderInfo()
        {
            txtDate.Text = _match.MatchDate.ToShortDateString();
            txtTime.Text = _match.StartTime.ToString();
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
            var teamUsers = _userService.GetUsersFromTeam(_match.TeamID);
            var existingAttendances = _attendanceService.GetAttendancesByMatchId(_match.MatchID);

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
                    var existingAttendance = new EntitiesLayer.Entities.Attendance
                    {
                        AttendanceID = attendance.AttendanceID.Value,
                        MatchId = _match.MatchID,
                        UserID = attendance.User.UserID,
                        StatusID = attendance.StatusID,
                        Notes = attendance.Notes
                    };

                    var originalAttendance = _attendanceService.GetAttendancesByMatchId(_match.MatchID)
                        .FirstOrDefault(a => a.AttendanceID == attendance.AttendanceID);

                    if (originalAttendance != null &&
                        (originalAttendance.StatusID != existingAttendance.StatusID || originalAttendance.Notes != existingAttendance.Notes))
                    {
                        result = _attendanceService.UpdateAttendance(existingAttendance);
                    }
                }
                else
                {
                    var newAttendance = new EntitiesLayer.Entities.Attendance
                    {
                        MatchId = _match.MatchID,
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

            var matchManagement = new UcMatchManagement();
            if (isSuccess)
            {
                matchManagement.ShowToast("Attendance marked successfully!");
            }
            else
            {
                matchManagement.ShowToast("Failed to mark attendance!");
            }
            GuiManager.OpenContent(matchManagement);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcMatchManagement());
        }
    }
}
