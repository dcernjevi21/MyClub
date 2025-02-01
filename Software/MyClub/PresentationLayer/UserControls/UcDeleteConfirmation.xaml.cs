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
    /// Interaction logic for UcDeleteConfirmation.xaml
    /// </summary>
    public partial class UcDeleteConfirmation : UserControl
    {
        public event EventHandler<bool> DeleteConfirmed;

        public UcDeleteConfirmation()
        {
            InitializeComponent();
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            DeleteConfirmed?.Invoke(this, true);
            Window.GetWindow(this).Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            DeleteConfirmed?.Invoke(this, false);
            Window.GetWindow(this).Close();
        }
    }
}
