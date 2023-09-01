using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HoloViewer
{
    public partial class HashTag : ObservableObject
    {
        [ObservableProperty]
        private bool isUseHashTag;

        [ObservableProperty]
        private string hashTagName;
    }

    public partial class ApplicationSettings : ObservableObject
    {
        private const string ApplicationSettingFileName = "ApplicationSetting.json";

        public static ApplicationSettings Current { get; private set; } = new();

        [ObservableProperty]
        private AppTheme theme = AppTheme.Light;

        [ObservableProperty]
        private string startupPageUrl = "";

        [ObservableProperty]
        private string captureSavePath = ScreenCapture.GetCaptureDirectoryFullPath();

        [ObservableProperty]
        private bool isEnableAutoInsertHashTagYoutubeTag = true;

        [ObservableProperty]
        private ObservableCollection<HashTag> hashTags = new();

        [ObservableProperty]
        private bool isEnableAutoInsertHashTagHoloViewer = false;

        [ObservableProperty]
        private bool isEnableUpdateCheck = true;

        private static string GetApplicationSettingsFilePath()
        {
#if WINDOWS
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ApplicationSettingFileName);
#elif MACCATALYST || MACOS
            return Path.Combine($"/Users/{Environment.UserName}/Library/Preferences/HoloViewer/", ApplicationSettingFileName);
#else
            return "";
#endif
        }

        public static bool ExistsApplicationSettingsFile()
        {
            return File.Exists(GetApplicationSettingsFilePath());
        }

        public static void Load()
        {
            using var streamReader = new StreamReader(GetApplicationSettingsFilePath());

            var jsonString = streamReader.ReadToEnd();

            try
            {
                Current = JsonSerializer.Deserialize<ApplicationSettings>(jsonString);
            }
            catch (Exception)
            {
                streamReader.Close();

                File.Delete(GetApplicationSettingsFilePath());
            }
        }

        public static void Save()
        {
            string directoryPath = Path.GetDirectoryName(GetApplicationSettingsFilePath());

            if ((directoryPath != "") && (!Directory.Exists(directoryPath)))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var jsonString = JsonSerializer.Serialize(Current);

            using var streamWriter = new StreamWriter(GetApplicationSettingsFilePath());

            streamWriter.Write(jsonString);
        }
    }
}
