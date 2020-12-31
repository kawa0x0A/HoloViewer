using System.Windows;

namespace HoloViewer
{
    public static class WindowUtility
    {
        public static void SetLocateCenter (Window childWindow)
        {
            var parentWindow = System.Windows.Application.Current.MainWindow;
            double parentWindowWidth = parentWindow.Width;
            double parentWindowHeight = parentWindow.Height;
            double childWindowWidth = childWindow.Width;
            double childWindowHeight = childWindow.Height;

            childWindow.Left = parentWindow.Left + (parentWindowWidth / 2) - (childWindowWidth / 2);
            childWindow.Top = parentWindow.Top + (parentWindowHeight / 2) - (childWindowHeight / 2);
        }
    }
}
