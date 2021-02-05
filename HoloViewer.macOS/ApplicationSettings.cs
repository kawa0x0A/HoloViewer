using Foundation;

namespace HoloViewer.macOS
{
    [Register(nameof(ApplicationSettings))]
    public class ApplicationSettings : NSObject
    {
        [Register(nameof(HashTagSettingsDataSet))]
        public class HashTagSettingsDataSet : NSObject
        {
            private bool isUseHashTag;

            private string hashTagName;

            [Export(nameof(IsUseHashTag))]
            public bool IsUseHashTag { get { return isUseHashTag; } set { WillChangeValue(nameof(IsUseHashTag)); isUseHashTag = value; DidChangeValue(nameof(IsUseHashTag)); } }

            [Export(nameof(HashTagName))]
            public string HashTagName { get { return hashTagName; } set { WillChangeValue(nameof(HashTagName)); hashTagName = value; DidChangeValue(nameof(HashTagName)); } }
        }

        private string startupPageUrl = "";

        private string captureSavePath = "";

        private bool isEnableInsertTweetYoutubeTag = true;

        private HashTagSettingsDataSet[] isUseHashTags = new HashTagSettingsDataSet[0];

        private bool isEnableInsertTweetHoloViewerHashTag = false;

        private bool isEnableUpdateCheck = true;

        [Export(nameof(StartupPageUrl))]
        public string StartupPageUrl { get { return startupPageUrl; } set { WillChangeValue(nameof(StartupPageUrl)); startupPageUrl = value; DidChangeValue(nameof(StartupPageUrl)); } }

        [Export(nameof(CaptureSavePath))]
        public string CaptureSavePath { get { return captureSavePath; } set { WillChangeValue(nameof(CaptureSavePath)); captureSavePath = value; DidChangeValue(nameof(CaptureSavePath)); } }

        [Export(nameof(IsEnableInsertTweetYoutubeTag))]
        public bool IsEnableInsertTweetYoutubeTag { get { return isEnableInsertTweetYoutubeTag; } set { WillChangeValue(nameof(IsEnableInsertTweetYoutubeTag)); isEnableInsertTweetYoutubeTag = value; DidChangeValue(nameof(IsEnableInsertTweetYoutubeTag)); } }

        [Export(nameof(IsUseHashTags))]
        public HashTagSettingsDataSet[] IsUseHashTags { get { return isUseHashTags; } set { WillChangeValue(nameof(IsUseHashTags)); isUseHashTags = value; DidChangeValue(nameof(IsUseHashTags)); } }

        [Export(nameof(IsEnableInsertTweetHoloViewerHashTag))]
        public bool IsEnableInsertTweetHoloViewerHashTag { get { return isEnableInsertTweetHoloViewerHashTag; } set { WillChangeValue(nameof(IsEnableInsertTweetHoloViewerHashTag)); isEnableInsertTweetHoloViewerHashTag = value; DidChangeValue(nameof(IsEnableInsertTweetHoloViewerHashTag)); } }

        [Export(nameof(IsEnableUpdateCheck))]
        public bool IsEnableUpdateCheck { get { return isEnableUpdateCheck; } set { WillChangeValue(nameof(IsEnableUpdateCheck)); isEnableUpdateCheck = value; DidChangeValue(nameof(IsEnableUpdateCheck)); } }
    }
}
