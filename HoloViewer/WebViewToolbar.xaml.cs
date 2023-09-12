using CommunityToolkit.Mvvm.ComponentModel;

namespace HoloViewer;

public partial class WebViewToolbar : ContentView
{
    private partial class ModelView : ObservableObject
    {
        [ObservableProperty]
        private bool canPageBack;

        [ObservableProperty]
        private bool canPageForward;

        [ObservableProperty]
        private string currentPageTitle;

        [ObservableProperty]
        private string currentUrl;

        [ObservableProperty]
        private string reloadButtonText;
    }

    private readonly ModelView BindingModelView = new();

#if WINDOWS
    private bool isLoading = false;
#elif MACCATALYST || MACOS
    private class NavigationDelegate : WebKit.WKNavigationDelegate
    {
        private readonly WebViewToolbar webViewToolbar;

        public NavigationDelegate(WebViewToolbar webViewToolbar)
        {
            this.webViewToolbar = webViewToolbar;
        }

        public override void DidStartProvisionalNavigation(WebKit.WKWebView webView, WebKit.WKNavigation navigation)
        {
            webViewToolbar.UpdateUrl();
            webViewToolbar.UpdateReloadButton();
        }

        public override void DidFailNavigation(WebKit.WKWebView webView, WebKit.WKNavigation navigation, Foundation.NSError error)
        {
            webViewToolbar.UpdateReloadButton();
        }

        public override void DidFinishNavigation(WebKit.WKWebView webView, WebKit.WKNavigation navigation)
        {
            webViewToolbar.UpdateTitle();
            webViewToolbar.UpdateReloadButton();
        }
    }
#endif

    public WebViewToolbar()
    {
        InitializeComponent();

        BindingContext = BindingModelView;
    }

#if WINDOWS
    public Microsoft.UI.Xaml.Controls.WebView2 WebView { get; set; }
#elif MACCATALYST || MACOS
    public WebKit.WKWebView WebView { get; set; }
#endif

    public void AddNavigationEvent()
    {
#if WINDOWS
        void Update()
        {
            UpdateUrl();
            UpdateTitle();
            UpdateReloadButton();
        }

        WebView.NavigationStarting += (sender, e) => { isLoading = true; Update(); };
        WebView.NavigationCompleted += (sender, e) => { isLoading = false; Update(); };
        WebView.CoreWebView2.ContentLoading += (sender, e) => { isLoading = true; Update(); };
        WebView.CoreWebView2.SourceChanged += (sender, e) => { isLoading = false; Update(); };
        WebView.CoreWebView2.DocumentTitleChanged += (sender, e) => { UpdateTitle(); };
#elif MACCATALYST || MACOS
        WebView.NavigationDelegate = new NavigationDelegate(this);
#endif
    }

    public void PageBack(object sender, EventArgs e)
    {
#if WINDOWS
        WebView?.GoBack();
#elif MACCATALYST || MACOS
        WebView?.GoBack();
#endif
    }

    public void PageForward(object sender, EventArgs e)
    {
#if WINDOWS
        WebView?.GoForward();
#elif MACCATALYST || MACOS
        WebView?.GoForward();
#endif
    }

    public void StopOrReload(object sender, EventArgs e)
    {
#if WINDOWS
        if (isLoading)
        {
            WebView?.CoreWebView2.Stop();
        }
        else
        {
            WebView?.CoreWebView2.Reload();
        }
#elif MACCATALYST || MACOS
        if (WebView.IsLoading)
        {
            WebView.StopLoading();
        }
        else
        {
            WebView.Reload();
        }
#endif
    }

    public void NavigationUrl(object sender, EventArgs e)
    {
#if WINDOWS
        WebView?.CoreWebView2.Navigate(BindingModelView.CurrentUrl);
#elif MACCATALYST || MACOS
        WebView?.LoadRequest(new Foundation.NSUrlRequest(new Foundation.NSUrl(BindingModelView.CurrentUrl)));
#endif
    }

    public void UpdateTitle()
    {
#if WINDOWS
        BindingModelView.CurrentPageTitle = WebView.CoreWebView2.DocumentTitle;
#elif MACCATALYST || MACOS
        BindingModelView.CurrentPageTitle = WebView.Title;
#endif
    }

    public void UpdateUrl()
    {
        BindingModelView.CurrentUrl = WebViewUtility.GetCurrentUrl(WebView);
    }

    public void UpdateReloadButton()
    {
#if WINDOWS || MACCATALYST || MACOS
        const string StopText = "Stop";
        const string ReloadText = "Reload";
#endif

#if WINDOWS
        BindingModelView.CanPageBack = WebView.CoreWebView2.CanGoBack;
        BindingModelView.CanPageForward = WebView.CoreWebView2.CanGoForward;
        BindingModelView.ReloadButtonText = isLoading ? StopText : ReloadText;
#elif MACCATALYST || MACOS
        BindingModelView.CanPageBack = WebView.CanGoBack;
        BindingModelView.CanPageForward = WebView.CanGoForward;
        BindingModelView.ReloadButtonText = WebView.IsLoading ? StopText : ReloadText;
#endif
    }

    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        var entry = (Entry)e.VisualElement;

        entry.CursorPosition = 0;
        entry.SelectionLength = entry.Text.Length;
    }

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        var entry = (Entry)e.VisualElement;

        entry.CursorPosition = entry.Text.Length;
    }
}
