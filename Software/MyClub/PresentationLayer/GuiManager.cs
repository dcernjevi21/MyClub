using PresentationLayer.Helper;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer
{
    internal static class GuiManager
    {
        public static Window CurrentWindow { get; set; }

        private static UserControl currentContent { get; set; }
        private static UserControl previousContent { get; set; }

        public static void SetMainWindow(Window window)
        {
            CurrentWindow = window;
        }

        public static void OpenContent(UserControl userControl)
        {
            if (CurrentWindow == null) return;

            var contentPanel = GetContentPanel();
            if (contentPanel == null) return;

            previousContent = contentPanel.Content as UserControl;
            contentPanel.Content = userControl;
            currentContent = contentPanel.Content as UserControl;
        }

        public static void CloseContent()
        {
            OpenContent(previousContent);
        }

        private static ContentControl GetContentPanel()
        {
            return CurrentWindow?.FindName("contentPanel") as ContentControl;
        }

        public static void Logout()
        {
            new FrmLogin().Show();
            CurrentWindow.Close();
            CurrentUser.User = null;
        }
    }
}
