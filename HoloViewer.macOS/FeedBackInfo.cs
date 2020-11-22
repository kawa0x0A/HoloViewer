using Foundation;

namespace HoloViewer.macOS
{
    [Register(nameof(FeedBackInfo))]
    public class FeedBackInfo : NSObject
    {
        [Export(nameof(Version))]
        public string Version { get; } = FeedBackInfoValue.Version;

        [Export(nameof(ReleaseDate))]
        public string ReleaseDate { get; } = FeedBackInfoValue.ReleaseDate;

        [Export(nameof(SoftwareLicense))]
        public string SoftwareLicense { get; } = FeedBackInfoValue.SoftwareLicense;

        [Export(nameof(MailAddress))]
        public string MailAddress { get; } = FeedBackInfoValue.MailAddress;

        [Export(nameof(RepositoryPageUrl))]
        public string RepositoryPageUrl { get; } = FeedBackInfoValue.RepositoryPageUrl;

        [Export(nameof(TwitterProfilePageUrl))]
        public string TwitterProfilePageUrl { get; } = FeedBackInfoValue.TwitterProfilePageUrl;

        [Export(nameof(FeedBackDescription))]
        public string FeedBackDescription { get; } = FeedBackInfoValue.FeedBackDescription;
    }
}
