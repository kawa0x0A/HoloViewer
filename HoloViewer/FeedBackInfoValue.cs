namespace HoloViewer
{
    public static class FeedBackInfoValue
    {
        public static readonly System.Version Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        public static readonly string VersionString = Version.ToString();

        public const string ReleaseDate = "2021/2/7";

        public const string SoftwareLicense = "MIT";

        public const string MailAddress = "holoviewer.contact@gmail.com";

        public const string RepositoryPageUrl = @"https://github.com/kawa0x0A/HoloViewer";

        public const string TwitterProfilePageUrl = @"https://twitter.com/kawa0x0A";

        public const string FeedBackDescription = "アプリケーションに関するご意見・ご要望などがありましたら\nメールかTwitterで開発者までリプライを送っていただくか\nハッシュタグ #ホロビューワー か #HoloViewer を付けてご投稿ください。\n(頂いたご要望などに対応できない場合があります。ご了承ください。)";
    }
}
