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

            applicationSettingsWindow.StartUpPageUrl = applicationSettings.StartupPageUrl;
            applicationSettingsWindow.IsEnableUpdateCheck = applicationSettings.IsEnableUpdateCheck;

            applicationSettingsWindow.ShowDialog();

            CurrentApplicationSettings = new ApplicationSettings()
            {
                StartupPageUrl = applicationSettingsWindow.StartUpPageUrl,
                IsEnableUpdateCheck = applicationSettingsWindow.IsEnableUpdateCheck,
            };

            return applicationSettingsWindow.Result;
        }

        public ApplicationSettings GetApplicationSettings ()
        {
            return CurrentApplicationSettings;
        }
    }
}
