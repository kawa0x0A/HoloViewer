#if __MACOS__
using Foundation;
#endif

namespace HoloViewer
{
#if NETSTANDARD
    public class FeedBackInfo
#elif __MACOS__
    [Register(nameof(FeedBackInfo))]
    public class FeedBackInfo : NSObject
#endif
    {
#if __MACOS__
        [Export(nameof(Version))]
#endif
        public string Version { get; } = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

#if __MACOS__
        [Export(nameof(RepositoryPageUrl))]
#endif
        public string RepositoryPageUrl { get; } = @"https://github.com/kawa0x0A/HoloViewer";

#if __MACOS__
        [Export(nameof(TwitterProfilePageUrl))]
#endif
        public string TwitterProfilePageUrl { get; } = @"https://twitter.com/kawa0x0A";

#if __MACOS__
        [Export(nameof(FeedBackDescription))]
#endif
        public string FeedBackDescription { get; } = "アプリケーションに関するご意見・ご要望などがありましたら\nTwitterで製作者までリプライを送っていただくか\nハッシュタグ #ホロビューワー か #HoloViewer を付けてご投稿ください。\n(頂いたご要望に対応できない場合があります。ご了承ください。)";
    }
}
