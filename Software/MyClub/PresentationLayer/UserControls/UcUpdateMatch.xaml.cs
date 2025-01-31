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
    /// Interaction logic for UcAddMatch.xaml
    /// </summary>
    public partial class UcUpdateMatch : UserControl
    {
        public UcUpdateMatch()
        {
            InitializeComponent();
        }

        private void btnUpdateMatch_Click(object sender, RoutedEventArgs e)
        {

            GuiManager.CloseContent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            GuiManager.CloseContent();
        }
    }
}
