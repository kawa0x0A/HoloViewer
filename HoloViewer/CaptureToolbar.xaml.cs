using CommunityToolkit.Mvvm.ComponentModel;

namespace HoloViewer;

public partial class CaptureToolbar : ContentView
{
    public partial class ModelView : ObservableObject
    {
        [ObservableProperty]
        private bool[] isCapture;

        [ObservableProperty]
        private bool isCaptureYoutubePlayerOnly = true;

        [ObservableProperty]
        private bool isCapturedImageCombine = false;
    }

    public ModelView BindingModelView { get; } = new ModelView();

    public StackLayout CaptureButtons { get { return Buttons; } }

#if WINDOWS
    public Microsoft.UI.Xaml.Controls.WebView2[] CaptureTargetWebviews { get; private set; } = new Microsoft.UI.Xaml.Controls.WebView2[1];
#elif MACCATALYST || MACOS
    public WebKit.WKWebView[] CaptureTargetWebviews { get; private set; } = new WebKit.WKWebView[1];
#endif

    public Func<Task<Rect[]>> CapturePositionFunction { get; set; } = null;

    public CaptureToolbar()
    {
        InitializeComponent();

        BindingContext = BindingModelView;
    }

    public void AllocateCaptureTargetWebview(int webviewCount)
    {
#if WINDOWS
        CaptureTargetWebviews = new Microsoft.UI.Xaml.Controls.WebView2[webviewCount];
#elif MACCATALYST || MACOS
        CaptureTargetWebviews = new WebKit.WKWebView[webviewCount];
#endif
    }

    private async void Capture(object sender, EventArgs e)
    {
#if WINDOWS || MACCATALYST || MACOS
        if ((BindingModelView.IsCapturedImageCombine) || (CaptureTargetWebviews.Length == 1))
        {
            await ScreenCapture.CaptureCombine(CaptureTargetWebviews.ToArray(), await CapturePositionFunction());
        }
        else
        {
            await ScreenCapture.CaptureSeparate(CaptureTargetWebviews.ToArray());
        }
#endif
    }

    private async void Tweet(object sender, EventArgs e)
    {
        var youtubeHashTags = new List<string>();

        if (ApplicationSettings.Current.IsEnableAutoInsertHashTagYoutubeTag)
        {
#if WINDOWS || MACCATALYST || MACOS
            foreach (var webView in CaptureTargetWebviews)
            {
                string ScriptPath = $"{WebViewUtility.ScriptRootPath}/tweet/";
                string GetHashTagsScriptPath = ScriptPath + "GetHashTags.js";

                var hashTagArray = await WebViewUtility.ExecuteJavaScriptAsync(webView, GetHashTagsScriptPath);

                youtubeHashTags.AddRange(TwitterUtility.ConvertStringToHashTagArray(hashTagArray).Where(hashTagName => !string.IsNullOrWhiteSpace(hashTagName)));
            }
#endif
        }

        var hashTags = new List<string>();

        foreach (var hashTagName in youtubeHashTags)
        {
            var hashTag = ApplicationSettings.Current.HashTags.SingleOrDefault(tag => tag.HashTagName == hashTagName);

            if (hashTag == null)
            {
                hashTags.Add(hashTagName);

                ApplicationSettings.Current.HashTags.Add(new HashTag() { IsUseHashTag = true, HashTagName = hashTagName});
            }
            else
            {
                if (hashTag.IsUseHashTag)
                {
                    hashTags.Add(hashTagName);
                }
            }
        }

        ApplicationSettings.Save();

        if (ApplicationSettings.Current.IsEnableAutoInsertHashTagHoloViewer)
        {
            hashTags.Add("HoloViewer");
            hashTags.Add("ホロビューワー");
        }

        await Browser.Default.OpenAsync(TwitterUtility.CreateTweetUrl(hashTags.Distinct().ToArray()));
    }
}
