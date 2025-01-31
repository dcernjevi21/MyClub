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
    public partial class UcPostponeMatch : UserControl
    {
        private EntitiesLayer.Entities.Match match;

        public UcPostponeMatch(EntitiesLayer.Entities.Match fetchedMatch)
        {
            InitializeComponent();
            match = fetchedMatch;
        }

        private void btnPostponeMatch_Click(object sender, RoutedEventArgs e)
        {
            var _matchService = new MatchManagementService();
            if (match == null)
            {
                MessageBox.Show("Match not found!");
                return;
            }

            string explanation = txtExplanation.Text;
            if (string.IsNullOrEmpty(explanation))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }
            else
            {
                //ne upise cancelled u bazu
                match.Status = "Cancelled";
                match.Summary = explanation;
                bool a = _matchService.UpdateMatch(match);
                if (a)
                {
                    MessageBox.Show("Match updated successfully!");
                }
                else
                {
                    MessageBox.Show("Match update failed!");
                }
            }

            GuiManager.CloseContent();
        }

            private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
        }
    }
}
