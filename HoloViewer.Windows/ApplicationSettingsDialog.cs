[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.Windows.ApplicationSettingsDialog))]
namespace HoloViewer.Windows
{
    class ApplicationSettingsDialog : IApplicationSettingsDialog
    {
        private ApplicationSettings CurrentApplicationSettings { get; set; }

        public bool Show (ApplicationSettings applicationSettings)
        {
            var applicationSettingsWindow = new ApplicationSettingsWindow();

            WindowUtility.SetLocateCenter(applicationSettingsWindow);

            applicationSettingsWindow.CurrentApplicationSettingsDataSet.StartUpPageUrl = applicationSettings.StartupPageUrl;
            applicationSettingsWindow.CurrentApplicationSettingsDataSet.CaptureSavePath = applicationSettings.CaptureSavePath;
            applicationSettingsWindow.CurrentApplicationSettingsDataSet.IsEnableAutoInsertHashTagYoutubeTag = applicationSettings.IsEnableAutoInsertHashTagYoutubeTag;
            applicationSettingsWindow.CurrentApplicationSettingsDataSet.IsEnableAutoInsertHashTagHoloViewer = applicationSettings.IsEnableAutoInsertHashTagHoloViewer;
            applicationSettingsWindow.CurrentApplicationSettingsDataSet.IsEnableUpdateCheck = applicationSettings.IsEnableUpdateCheck;

            applicationSettingsWindow.ShowDialog();

            CurrentApplicationSettings = new ApplicationSettings()
            {
                StartupPageUrl = applicationSettingsWindow.CurrentApplicationSettingsDataSet.StartUpPageUrl,
                CaptureSavePath = applicationSettingsWindow.CurrentApplicationSettingsDataSet.CaptureSavePath,
                IsEnableAutoInsertHashTagYoutubeTag = applicationSettingsWindow.CurrentApplicationSettingsDataSet.IsEnableAutoInsertHashTagYoutubeTag,
                IsEnableAutoInsertHashTagHoloViewer = applicationSettingsWindow.CurrentApplicationSettingsDataSet.IsEnableAutoInsertHashTagHoloViewer,
                IsEnableUpdateCheck = applicationSettingsWindow.CurrentApplicationSettingsDataSet.IsEnableUpdateCheck,
            };

            return applicationSettingsWindow.Result;
        }

        public ApplicationSettings GetApplicationSettings ()
        {
            return CurrentApplicationSettings;
        }
    }
}
