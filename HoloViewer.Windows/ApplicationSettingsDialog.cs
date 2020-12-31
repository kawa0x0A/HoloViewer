[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.Windows.ApplicationSettingsDialog))]
namespace HoloViewer.Windows
{
    class ApplicationSettingsDialog : IApplicationSettingsDialog
    {
        private HoloViewer.ApplicationSettings CurrentApplicationSettings { get; set; }

        public bool Show (HoloViewer.ApplicationSettings applicationSettings)
        {
            var applicationSettingsWindow = new ApplicationSettingsWindow();

            WindowUtility.SetLocateCenter(applicationSettingsWindow);

            applicationSettingsWindow.StartUpPageUrl = applicationSettings.StartupPageUrl;
            applicationSettingsWindow.IsEnableUpdateCheck = applicationSettings.IsEnableUpdateCheck;

            applicationSettingsWindow.ShowDialog();

            CurrentApplicationSettings = new HoloViewer.ApplicationSettings()
            {
                StartupPageUrl = applicationSettingsWindow.StartUpPageUrl,
                IsEnableUpdateCheck = applicationSettingsWindow.IsEnableUpdateCheck,
            };

            return applicationSettingsWindow.Result;
        }

        public HoloViewer.ApplicationSettings GetApplicationSettings ()
        {
            return CurrentApplicationSettings;
        }
    }
}
