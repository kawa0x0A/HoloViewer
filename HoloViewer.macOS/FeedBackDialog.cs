using AppKit;
using Foundation;

[assembly:Xamarin.Forms.Dependency(typeof(HoloViewer.macOS.FeedBackDialog))]
namespace HoloViewer.macOS
{
    class FeedBackDialog : IFeedBackDialog
    {
        public void ShowDialog()
        {
            var windowController = NSStoryboard.FromName("FeedBackWindow", null).InstantiateControllerWithIdentifier("FeedBackWindow") as NSWindowController;

            windowController.Window.WillClose += Window_WillClose;

            NSApplication.SharedApplication.RunModalForWindow(windowController.Window);
        }

        private void Window_WillClose(object sender, System.EventArgs e)
        {
            Close(NSApplication.SharedApplication.MainWindow);
        }

        public static void Close(NSObject sender)
        {
            NSApplication.SharedApplication.StopModal();
            NSApplication.SharedApplication.MainWindow.OrderOut(sender);
        }
    }
}
