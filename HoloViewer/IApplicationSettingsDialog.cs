using System;
using System.Collections.Generic;
using System.Text;

namespace HoloViewer
{
    public interface IApplicationSettingsDialog
    {
        public bool Show (ApplicationSettings applicationSettings);

        public ApplicationSettings GetApplicationSettings ();
    }
}
