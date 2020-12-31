using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.Windows.ApplicationSettings))]
namespace HoloViewer.Windows
{
    class ApplicationSettings : IApplicationSettings
    {
        public bool ExistsApplicationSettingsFile ()
        {
            return File.Exists(IApplicationSettings.ApplicationSettingFileName);
        }

        public HoloViewer.ApplicationSettings Load ()
        {
            string jsonString = "";

            using (var streamReader = new StreamReader(IApplicationSettings.ApplicationSettingFileName))
            {
                jsonString = streamReader.ReadToEnd();
            }

            return JsonSerializer.Deserialize<HoloViewer.ApplicationSettings>(jsonString);
        }

        public void Save (HoloViewer.ApplicationSettings applicationSettings)
        {
            string jsonString = JsonSerializer.Serialize(applicationSettings);

            using (var streamWriter = new StreamWriter(IApplicationSettings.ApplicationSettingFileName))
            {
                streamWriter.Write(jsonString);
            }
        }
    }
}
