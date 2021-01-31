using System;
using System.Threading.Tasks;
using Microsoft.MobileBlazorBindings.Elements;
using AppKit;
using Foundation;
using WebKit;

[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.macOS.TwitterDialog))]
namespace HoloViewer.macOS
{
    public class TwitterDialog : ITwitterDialog
    {
        public async Task<string[]> GetHashTags(BlazorWebView blazorWebView)
        {
            var hashTag = await WebView.ExecuteJavascript(blazorWebView, ITwitterDialog.GetHashTagsScriptPath);

            return TwitterUtility.ConvertStringToHashTagArray(hashTag);
        }

        public async Task Tweet(string[] hashTags)
        {
            var windowController = NSStoryboard.FromName("WebViewWindow", null).InstantiateControllerWithIdentifier("WebViewWindow") as NSWindowController;
            var webview = (WKWebView)windowController.Window.ContentViewController.View.Subviews[0];

            windowController.Window.WillClose += Window_WillClose;

            webview.LoadRequest(new NSUrlRequest(new NSUrl(TwitterUtility.CreateTweetUrl(hashTags))));

            NSApplication.SharedApplication.RunModalForWindow(windowController.Window);
        }

        private void Window_WillClose(object sender, EventArgs e)
        {
            NSApplication.SharedApplication.StopModal();
            NSApplication.SharedApplication.MainWindow.OrderOut(NSObject.FromObject(sender));
        }
    }
}
