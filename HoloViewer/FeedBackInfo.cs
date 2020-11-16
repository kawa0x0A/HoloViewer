using Foundation;

namespace HoloViewer
{
    [Register(nameof(FeedBackInfo))]
    public class FeedBackInfo : NSObject
    {
        [Export(nameof(Version))]
        public string Version { get; } = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [Export(nameof(RepositoryPageUrl))]
        public string RepositoryPageUrl { get; } = @"https://github.com/kawa0x0A/HoloViewer";

        [Export(nameof(TwitterProfilePageUrl))]
        public string TwitterProfilePageUrl { get; } = @"https://twitter.com/kawa0x0A";

        [Export(nameof(FeedBackDescription))]
        public string FeedBackDescription { get; } = "アプリケーションに関するご意見・ご要望などがありましたら\nTwitterで製作者までリプライを送っていただくか\nハッシュタグ #ホロビューワー か #HoloViewer を付けて投稿してください。\n(頂いたご要望に対応できない場合があります。ご了承ください。)";
    }
}
