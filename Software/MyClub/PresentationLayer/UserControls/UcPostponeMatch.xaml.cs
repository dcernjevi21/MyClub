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
    public partial class UcPostponeMatch : UserControl
    {
        private EntitiesLayer.Entities.Match match;

        public UcPostponeMatch(EntitiesLayer.Entities.Match fetchedMatch)
        {
            InitializeComponent();
            match = fetchedMatch;
        }
        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }

        private void btnPostponeMatch_Click(object sender, RoutedEventArgs e)
        {
            var _matchService = new MatchManagementService();
            if (match == null)
            {
                ShowToast("Match not found!");
                return;
            }

            string explanation = txtExplanation.Text;
            if (string.IsNullOrEmpty(explanation))
            {
                ShowToast("Please fill in all fields.");
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
                    ShowToast("Match updated successfully!");
                }
                else
                {
                    ShowToast("Match update failed!");
                }
            }

            GuiManager.CloseContent();
        }

            private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
        }

        //Černjević
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == KeySplineConverter.F1) OpenUserDocumentation();
        }
        /*
        private void OpenUserDocumentation()
        {
            try
            {
                //https://www.c-sharpcorner.com/forums/opening-pdf-files-from-resources

                byte[] PDF = Properties.Resources.FintessSys___korisnici;
                MemoryStream ms = new MemoryStream(PDF);
                FileStream f = new FileStream("help-members.pdf", FileMode.OpenOrCreate);

                ms.WriteTo(f);

                f.Close();
                ms.Close();

                Process.Start("help-members.pdf");
            }
            catch (Exception)
            {
                MessageBox.Show("Nije moguće otvoriti dokumentaciju!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/
    }
}
