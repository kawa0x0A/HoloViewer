using System.Windows;

[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.Windows.WindowMode))]

namespace HoloViewer.Windows
{
    public class WindowMode : IWindowMode
    {
        void IWindowMode.WindowMode ()
        {
            Application.Current.MainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            Application.Current.MainWindow.ResizeMode = ResizeMode.CanResize;
        }

        public void FullScreen ()
        {
            Application.Current.MainWindow.WindowStyle = WindowStyle.None;
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            Application.Current.MainWindow.ResizeMode = ResizeMode.NoResize;
        }
    }
}
