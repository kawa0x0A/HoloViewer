using System;
using System.IO;
using System.Text.Json;
using Xamarin.Forms;

namespace HoloViewer
{
    public class ApplicationSettings
    {
        private const string ApplicationSettingFileName = "ApplicationSetting.json";

        public string StartupPageUrl { get; set; } = "";

        public bool IsEnableUpdateCheck { get; set; } = true;

        public static bool ExistsApplicationSettingsFile()
        {
            return File.Exists(ApplicationSettingFileName);
        }

        public static ApplicationSettings Load()
        {
            var jsonString = "";

            using (var streamReader = new StreamReader(ApplicationSettingFileName))
            {
                jsonString = streamReader.ReadToEnd();
            }

            return JsonSerializer.Deserialize<ApplicationSettings>(jsonString);
        }

        public static void Save(ApplicationSettings applicationSettings)
        {
            var jsonString = JsonSerializer.Serialize(applicationSettings);

            using (var streamWriter = new StreamWriter(ApplicationSettingFileName))
            {
                streamWriter.Write(jsonString);
            }
        }
    }
}
