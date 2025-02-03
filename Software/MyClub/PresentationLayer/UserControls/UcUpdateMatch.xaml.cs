using BusinessLogicLayer.Services;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Paragraph = iTextSharp.text.Paragraph;
using Rectangle = iTextSharp.text.Rectangle;
using Org.BouncyCastle.Crypto.Utilities;
using System.Diagnostics;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for UcAddMatch.xaml
    /// </summary>
    /// 
    ///Černjević
    public partial class UcUpdateMatch : UserControl
    {
        private EntitiesLayer.Entities.Match match;

        public UcUpdateMatch(EntitiesLayer.Entities.Match fetchedMatch)
        {
            InitializeComponent();
            match = fetchedMatch;
            LoadMatchInfo();
        }

        private void LoadMatchInfo()
        {
            if (match != null)
            {
                txtResult.Text = match.Result;
                txtSummary.Text = match.Summary;
                cmbStatus.SelectedItem = match.Status;
            }
        }

        private void ShowToast(string message)
        {
            ToastWindow toast = new ToastWindow(message);
            toast.Show();
        }

        private void btnUpdateMatch_Click(object sender, RoutedEventArgs e)
        {
            var _matchService = new MatchManagementService();
            if (match == null)
            {
                ShowToast("Match not found!");
                return;
            }

            string result = txtResult.Text;
            string summary = txtSummary.Text;

            string resultPattern = @"^([0-9]|[1-9][0-9])[-:]([0-9]|[1-9][0-9])$";

            if (string.IsNullOrEmpty(summary) || string.IsNullOrEmpty(result))
            {
                ShowToast("Please fill in all fields.");
                return;
            }
            else if (!Regex.IsMatch(result, resultPattern))
            {
                ShowToast("Invalid result format. Please enter in format '0:0', '99:99', etc.");
                return;
            }
            else
            {
                match.Result = result;
                match.Summary = summary;
                match.Status = cmbStatus.SelectedValue.ToString();
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

        private void btnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            GenerateMatchReport(match.MatchID);
        }

        //Valec
        private void GenerateMatchReport(int matchId)
        {
            try
            {
                var matchService = new MatchManagementService();
                var match = matchService.GetMatchById(matchId);

                if (match == null)
                {
                    ShowToast("No match data available!");
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF Files|*.pdf",
                    DefaultExt = ".pdf",
                    FileName = $"MatchReport_{match.MatchDate:yyyyMMdd}.pdf"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                        PdfWriter writer = PdfWriter.GetInstance(document, stream);
                        document.Open();

                        Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK);
                        Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK);
                        Font contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);

                        Paragraph title = new Paragraph("Match Report", titleFont)
                        {
                            Alignment = Element.ALIGN_CENTER,
                            SpacingAfter = 20
                        };
                        document.Add(title);

                        PdfPTable table = new PdfPTable(2) { WidthPercentage = 100 };
                        table.AddCell(new PdfPCell(new Phrase("Opponent:", headerFont)) { Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(match.OpponentTeam, contentFont)) { Border = Rectangle.NO_BORDER });

                        table.AddCell(new PdfPCell(new Phrase("Date:", headerFont)) { Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(match.MatchDate.ToShortDateString(), contentFont)) { Border = Rectangle.NO_BORDER });

                        table.AddCell(new PdfPCell(new Phrase("Start Time:", headerFont)) { Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(match.StartTime.ToString(), contentFont)) { Border = Rectangle.NO_BORDER });

                        table.AddCell(new PdfPCell(new Phrase("Result:", headerFont)) { Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(match.Result, contentFont)) { Border = Rectangle.NO_BORDER });

                        table.AddCell(new PdfPCell(new Phrase("Summary:", headerFont)) { Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(match.Summary, contentFont)) { Border = Rectangle.NO_BORDER });

                        table.AddCell(new PdfPCell(new Phrase("Status:", headerFont)) { Border = Rectangle.NO_BORDER });
                        table.AddCell(new PdfPCell(new Phrase(match.Status, contentFont)) { Border = Rectangle.NO_BORDER });

                        document.Add(table);
                        document.Close();
                    }

                    ShowToast("Match report generated successfully!");
                }
            } catch (Exception ex)
            {
                ShowToast($"Error generating report: {ex.Message}");
            }
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
