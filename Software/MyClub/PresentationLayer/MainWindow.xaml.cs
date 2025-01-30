using PresentationLayer.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace PresentationLayer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //černjević
        public void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcProfileUser());
        }

        public void btnTrainings_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.OpenContent(new UcTrainings());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GuiManager.CurrentWindow = this;
        }
    }
}
