namespace HoloViewer.Windows
{
    public class FeedBackInfo
    {
        public string Version { get; } = FeedBackInfoValue.VersionString;

        public string ReleaseDate { get; } = FeedBackInfoValue.ReleaseDate;

        public string SoftwareLicense { get; } = FeedBackInfoValue.SoftwareLicense;

        public string MailAddress { get; } = FeedBackInfoValue.MailAddress;

        public string RepositoryPageUrl { get; } = FeedBackInfoValue.RepositoryPageUrl;

        public string TwitterProfilePageUrl { get; } = FeedBackInfoValue.TwitterProfilePageUrl;

        public string FeedBackDescription { get; } = FeedBackInfoValue.FeedBackDescription;
    }
}
