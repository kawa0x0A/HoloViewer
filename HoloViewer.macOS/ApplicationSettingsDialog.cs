using System.Collections.Generic;
using System.Linq;
using AppKit;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.macOS.ApplicationSettingsDialog))]
namespace HoloViewer.macOS
{
    class ApplicationSettingsDialog : IApplicationSettingsDialog
    {
        private ApplicationSettings CurrentApplicationSettings { get; set; } = new ApplicationSettings();

        public bool Show(HoloViewer.ApplicationSettings applicationSettings)
        {
            var windowController = NSStoryboard.FromName("ApplicationSettingsWindow", null).InstantiateControllerWithIdentifier("ApplicationSettingsWindow") as NSWindowController;
            var applicationSettingsViewController = (ApplicationSettingsViewController)windowController.ContentViewController;

            windowController.Window.WillClose += Window_WillClose;

            applicationSettingsViewController.ApplicationSettings.StartupPageUrl = applicationSettings.StartupPageUrl;
            applicationSettingsViewController.ApplicationSettings.CaptureSavePath = applicationSettings.CaptureSavePath;
            applicationSettingsViewController.ApplicationSettings.IsEnableInsertTweetYoutubeTag = applicationSettings.IsEnableAutoInsertHashTagYoutubeTag;
            applicationSettingsViewController.ApplicationSettings.IsUseHashTags = applicationSettings.IsUseHashTags.Select(p => new ApplicationSettings.HashTagSettingsDataSet() { HashTagName = p.Key, IsUseHashTag = p.Value }).ToArray();
            applicationSettingsViewController.ApplicationSettings.IsEnableInsertTweetHoloViewerHashTag = applicationSettings.IsEnableAutoInsertHashTagHoloViewer;
            applicationSettingsViewController.ApplicationSettings.IsEnableUpdateCheck = applicationSettings.IsEnableUpdateCheck;

            NSApplication.SharedApplication.RunModalForWindow(windowController.Window);

            CurrentApplicationSettings = applicationSettingsViewController.ApplicationSettings;

            return applicationSettingsViewController.Result;
        }

        private void Window_WillClose(object sender, System.EventArgs e)
        {
            NSApplication.SharedApplication.StopModal();
            NSApplication.SharedApplication.MainWindow.OrderOut(NSObject.FromObject(sender));
        }

        public HoloViewer.ApplicationSettings GetApplicationSettings()
        {
            return new HoloViewer.ApplicationSettings()
            {
                StartupPageUrl = CurrentApplicationSettings.StartupPageUrl,
                CaptureSavePath = CurrentApplicationSettings.CaptureSavePath,
                IsEnableAutoInsertHashTagYoutubeTag = CurrentApplicationSettings.IsEnableInsertTweetYoutubeTag,
                IsUseHashTags = new Dictionary<string, bool>(CurrentApplicationSettings.IsUseHashTags.Select(p => new KeyValuePair<string, bool>(p.HashTagName, p.IsUseHashTag))),
                IsEnableAutoInsertHashTagHoloViewer = CurrentApplicationSettings.IsEnableInsertTweetHoloViewerHashTag,
                IsEnableUpdateCheck = CurrentApplicationSettings.IsEnableUpdateCheck,
            };
        }
    }
}
