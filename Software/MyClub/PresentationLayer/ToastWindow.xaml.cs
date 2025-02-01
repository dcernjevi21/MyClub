using System;
using System.Windows;
using System.Windows.Threading;

namespace PresentationLayer
{
    public partial class ToastWindow : Window
    {
        public ToastWindow(string message)
        {
            InitializeComponent();
            txtToastMessage.Text = message;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                Close();
            };
            timer.Start();
        }
    }
}
