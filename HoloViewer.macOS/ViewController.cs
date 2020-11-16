using AppKit;
using Foundation;
using System;
using Xamarin.Essentials;

namespace HoloViewer.macOS
{
    public partial class ViewController : NSViewController
    {
        [Export(nameof(FeedBackInfo))]
        public FeedBackInfo FeedBackInfo { get; set; } = new FeedBackInfo();

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            AttributedLinkString(TwitterProfilePageUrlLabel, FeedBackInfo.TwitterProfilePageUrl);
            AttributedLinkString(RepositoryPageUrlLabel, FeedBackInfo.RepositoryPageUrl);

            var twitterProfilePageUrlClickGestureRecognizer = new NSClickGestureRecognizer(new Action(ClickedTwitterProfilePageUrlLabel));

            TwitterProfilePageUrlLabel.AddGestureRecognizer(twitterProfilePageUrlClickGestureRecognizer);

            var repositoryPageUrlClickGestureRecognizer = new NSClickGestureRecognizer(new Action(ClickedRepositoryPageUrlLabel));

            RepositoryPageUrlLabel.AddGestureRecognizer(repositoryPageUrlClickGestureRecognizer);

            base.ViewDidLoad();
        }

        public override void ViewWillAppear()
        {
            var appDelegate = NSApplication.SharedApplication.Delegate as AppDelegate;
            var mainWindow = appDelegate.MainWindow;

            nfloat x = (mainWindow.Frame.X + (mainWindow.Frame.Width / 2) - (View.Window.Frame.Width / 2));
            nfloat y = (mainWindow.Frame.Y + (mainWindow.Frame.Height / 2) - (View.Window.Frame.Height / 2));

            View.Window.SetFrame(new CoreGraphics.CGRect(x, y, View.Window.Frame.Width, View.Window.Frame.Height), false);

            base.ViewWillAppear();
        }

        private void AttributedLinkString(NSTextField textField, string text)
        {
            var attributedString = new NSMutableAttributedString(text);
            var range = new NSRange(0, text.Length);

            attributedString.BeginEditing();
            attributedString.AddAttribute(NSStringAttributeKey.ForegroundColor, NSColor.Blue, range);
            attributedString.AddAttribute(NSStringAttributeKey.UnderlineStyle, new NSNumber(1), range);
            attributedString.EndEditing();

            textField.AttributedStringValue = attributedString;
        }

        private async void ClickedTwitterProfilePageUrlLabel()
        {
            // Obsolete
            //Xamarin.Forms.Device.OpenUri(new Uri(FeedBackInfo.TwitterProfilePageUrl));

            // Need Install "Xamarin.Essentials" Package (v1.5.3.2 => v1.6.0-pre4)
            //await Launcher.OpenAsync(FeedBackInfo.TwitterProfilePageUrl);
            await Browser.OpenAsync(FeedBackInfo.TwitterProfilePageUrl);
        }

        private async void ClickedRepositoryPageUrlLabel()
        {
            await Browser.OpenAsync(FeedBackInfo.RepositoryPageUrl);
        }

        partial void ClickedCloseButton(NSButton sender)
        {
            FeedBackDialog.Close(sender);
        }
    }
}
