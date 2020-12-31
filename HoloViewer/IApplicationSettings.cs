using System;
using System.Collections.Generic;
using System.Text;

namespace HoloViewer
{
    public interface IApplicationSettings
    {
        protected const string ApplicationSettingFileName = "ApplicationSetting.json";

        bool ExistsApplicationSettingsFile ();

        ApplicationSettings Load ();

        void Save (ApplicationSettings applicationSettings);
    }
}
