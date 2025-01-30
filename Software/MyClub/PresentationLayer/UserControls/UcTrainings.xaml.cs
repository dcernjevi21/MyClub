using BusinessLogicLayer;
using EntitiesLayer.Entities;
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
    /// Interaction logic for UcTrainings.xaml
    /// </summary>
    public partial class UcTrainings : UserControl
    {
        private TrainingService services = new TrainingService();
        public UcTrainings()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var selectedTraining = dgTrainings.SelectedItem as Training;
            if(selectedTraining != null)
            {
                bool isSuccessful = services.RemoveTraining(selectedTraining);
                GuiManager.OpenContent(new UcTrainings());
            }

        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedTraining = dgTrainings.SelectedItem as Training;
            UcEditTraining editControl = new UcEditTraining(selectedTraining);
            GuiManager.OpenContent(editControl);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ShowAllTrainings();
        }

        private void ShowAllTrainings()
        {
            var allTrainings = services.GetTrainings();
            dgTrainings.ItemsSource = allTrainings;
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcAddTraining());
        }
    }
}

