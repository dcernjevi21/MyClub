﻿using BusinessLogicLayer.Services;
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
    /// Interaction logic for UcUserMyAttendances.xaml
    /// </summary>
    
    //Valec kompletno
    public partial class UcUserMyAttendances : UserControl
    {
        private readonly AttendanceService attendanceService = new AttendanceService();

        public UcUserMyAttendances()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var userId = CurrentUser.User.UserID;
            var attendances = attendanceService.GetUserAttendances(userId);

            int totalEvents = attendances.Count;
            int presentCount = attendanceService.CountAttendancesByStatus(attendances, 4);
            int absentCount = attendanceService.CountAttendancesByStatus(attendances, 5);
            int excusedCount = attendanceService.CountAttendancesByStatus(attendances, 6);

            txtTotalEvents.Text = totalEvents.ToString();
            txtPresentPercentage.Text = totalEvents > 0
                ? $"{(presentCount * 100.0 / totalEvents):F1}%"
                : "0%";
            txtAbsences.Text = absentCount.ToString();
            txtExcused.Text = excusedCount.ToString();

            var attendanceViewModels = attendances.Select(a => new
            {
                EventDate = a.Training?.TrainingDate ?? a.Match?.MatchDate,
                EventType = a.Training != null ? "Training" : "Match",
                EventTime = a.Training?.StartTime ?? a.Match?.StartTime,
                Status = a.Status,
                Notes = a.Notes
            }).ToList();

            dgAttendances.ItemsSource = attendanceViewModels;
        }
    }
}
