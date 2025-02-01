﻿using BusinessLogicLayer.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for UcMatchManagement.xaml
    /// </summary>
    public partial class UcMatchManagement : UserControl
    {
        private MatchManagementService _matchManagementService = new MatchManagementService();

        public UcMatchManagement()
        {
            InitializeComponent();
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMatches();
        }

        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }

        public void LoadMatches()
        {
            dgCoachGrid.ItemsSource = _matchManagementService.GetMatches();
        }

        public void btnAddMatch_Click(object sender, RoutedEventArgs e)
        {

            GuiManager.OpenContent(new UcAddMatch());
        }
        //Černjević
        public void btnUpdateMatch_Click(object sender, RoutedEventArgs e)
        {
            EntitiesLayer.Entities.Match match = GetMatch();
            if (match != null)
            {
                if (match.MatchDate > DateTime.Now)
                {
                    ShowToast("Cannot update future matches! Only matches that have already been played can be updated.");
                                  
                    return;
                }
                else if(match.Status == "Cancelled")
                {
                    ShowToast("Cannot update postponed matches! Only matches that have already been played can be updated.");
                    return;
                }
                else {
                    GuiManager.OpenContent(new UcUpdateMatch(match));
                }
            }
        }
        //Černjević
        public void btnDeleteMatch_Click(object sender, RoutedEventArgs e)
        {
            EntitiesLayer.Entities.Match match = GetMatch();
            if (match != null)
            {
                // Prikaz poruke s potvrdom brisanja
                MessageBoxResult result = MessageBox.Show(
                    "Jeste li sigurni da želite izbrisati zapis?",
                    "Potvrda brisanja",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                // Ako korisnik odabere 'Yes', izvršava se brisanje
                if (result == MessageBoxResult.Yes)
                {
                    MatchManagementService _matchManagementService = new MatchManagementService();
                    _matchManagementService.RemoveMatch(match);
                }
            }
        }
        //Černjević
        public void btnPostponeMatch_Click(object sender, RoutedEventArgs e)
        {
            EntitiesLayer.Entities.Match match = GetMatch();
            if (match != null)
            {
                GuiManager.OpenContent(new UcPostponeMatch(match));
            }
        }

        public EntitiesLayer.Entities.Match GetMatch()
        {
            return dgCoachGrid.SelectedItem as EntitiesLayer.Entities.Match;
        }

        //Valec
        private void btnAttendance_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var match = button.DataContext as EntitiesLayer.Entities.Match;
            if (match != null)
            {
                var attendanceControl = new UcMatchAttendanceCoach(match);
                GuiManager.OpenContent(attendanceControl);
            }
        }
    }
}
