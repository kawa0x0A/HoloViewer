using Foundation;

namespace HoloViewer.macOS
{
    [Register(nameof(ApplicationSettings))]
    public class ApplicationSettings : NSObject
    {
        private string startupPageUrl = "";

        private string captureSavePath = "";

        private bool isEnableUpdateCheck = true;

        [Export(nameof(StartupPageUrl))]
        public string StartupPageUrl { get { return startupPageUrl; } set { WillChangeValue(nameof(StartupPageUrl)); startupPageUrl = value; DidChangeValue(nameof(StartupPageUrl)); } }

        [Export(nameof(CaptureSavePath))]
        public string CaptureSavePath { get { return captureSavePath; } set { WillChangeValue(nameof(CaptureSavePath)); captureSavePath = value; DidChangeValue(nameof(CaptureSavePath)); } }

        [Export(nameof(IsEnableUpdateCheck))]
        public bool IsEnableUpdateCheck { get { return isEnableUpdateCheck; } set { WillChangeValue(nameof(IsEnableUpdateCheck)); isEnableUpdateCheck = value; DidChangeValue(nameof(IsEnableUpdateCheck)); } }
    }
}
