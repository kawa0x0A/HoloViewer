using System.IO;
using System.Text.Json;
using Xamarin.Forms;

namespace HoloViewer
{
    public class ApplicationSettings
    {
        private const string ApplicationSettingFileName = "ApplicationSetting.json";

        public string StartupPageUrl { get; set; } = "";

        public string CaptureSavePath { get; set; } = "";

        public bool IsEnableUpdateCheck { get; set; } = true;

        private static string GetApplicationSettingsFilePath()
        {
            switch(Device.RuntimePlatform)
            {
                case Device.WPF: return ApplicationSettingFileName;
                case Device.macOS: return Path.Combine($"/Users/{System.Environment.UserName}/Library/Preferences/HoloViewer/", ApplicationSettingFileName);
            }

            return "";
        }

        public static bool ExistsApplicationSettingsFile()
        {
            return File.Exists(GetApplicationSettingsFilePath());
        }

        public static ApplicationSettings Load()
        {
            var jsonString = "";

            using (var streamReader = new StreamReader(GetApplicationSettingsFilePath()))
            {
                jsonString = streamReader.ReadToEnd();
            }

            return JsonSerializer.Deserialize<ApplicationSettings>(jsonString);
        }

        public static void Save(ApplicationSettings applicationSettings)
        {
            string directoryPath = Path.GetDirectoryName(GetApplicationSettingsFilePath());

            if((directoryPath != "") && (!Directory.Exists(directoryPath)))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var jsonString = JsonSerializer.Serialize(applicationSettings);

            using (var streamWriter = new StreamWriter(GetApplicationSettingsFilePath()))
            {
                streamWriter.Write(jsonString);
            }
        }
    }
}
