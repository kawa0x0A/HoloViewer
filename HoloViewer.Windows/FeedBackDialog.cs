using System.Windows;

[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.Windows.FeedBackDialog))]
namespace HoloViewer.Windows
{
    class FeedBackDialog : IFeedBackDialog
    {
        public void ShowDialog ()
        {
            var mainWindow = Application.Current.MainWindow;
            var feedBackWindow = new FeedBackWindow();

            feedBackWindow.Left = mainWindow.Left + ((mainWindow.Width - feedBackWindow.Width) / 2);
            feedBackWindow.Top = mainWindow.Top + ((mainWindow.Height - feedBackWindow.Height) / 2);

            feedBackWindow.ShowDialog();
        }
    }
}
