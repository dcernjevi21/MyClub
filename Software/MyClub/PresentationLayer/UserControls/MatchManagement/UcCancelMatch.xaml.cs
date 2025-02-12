using BusinessLogicLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for UcPostponeMatch.xaml
    /// </summary>
    ///
    ///Černjević kompletno
    public partial class UcCancelMatch : UserControl
    {
        private EntitiesLayer.Entities.Match match;

        public UcCancelMatch(EntitiesLayer.Entities.Match fetchedMatch)
        {
            InitializeComponent();
            match = fetchedMatch;
        }
        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }

        private void btnCancelMatch_Click(object sender, RoutedEventArgs e)
        {
            var _matchService = new MatchManagementService();
            if (match == null)
            {
                ShowToast("Match not found!");
                GuiManager.CloseContent();
            }

            string explanation = txtExplanation.Text;
            if (string.IsNullOrEmpty(explanation))
            {
                ShowToast("Please fill in all fields.");
                return;
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to cancel this match",
                    "Confirm cancelation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    match.Status = "Cancelled";
                    match.Summary = explanation;
                    bool a = _matchService.UpdateMatch(match);
                    if (a)
                    {
                        ShowToast("Match cancelled successfully!");
                    }
                    else
                    {
                        ShowToast("Match cancelation failed!");
                    }
                } 
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
        }
    }
}
