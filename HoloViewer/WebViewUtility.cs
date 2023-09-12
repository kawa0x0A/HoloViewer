namespace HoloViewer
{
    public static class WebViewUtility
    {
#if WINDOWS
        public static readonly string ScriptRootPath = $"{AppDomain.CurrentDomain.BaseDirectory}/wwwroot/scripts";
#elif MACCATALYST || MACOS
        public static readonly string ScriptRootPath = $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../Resources")}/wwwroot/scripts";
#endif

#if WINDOWS
        public static string GetCurrentUrl(Microsoft.UI.Xaml.Controls.WebView2 webView)
#elif MACCATALYST || MACOS
        public static string GetCurrentUrl(WebKit.WKWebView webView)
#else
        public static string GetCurrentUrl(object webView)
#endif
        {
#if WINDOWS
            return ((UrlWebViewSource)webView.CoreWebView2.Source).Url;
#elif MACCATALYST || MACOS
            return webView.Url.ToString();
#else
            return "";
#endif
        }

#if WINDOWS
        public static async Task<string> ExecuteJavaScriptAsync(Microsoft.UI.Xaml.Controls.WebView2 webView, string executeJavaScriptFilePath)
#elif MACCATALYST || MACOS
        public static async Task<string> ExecuteJavaScriptAsync(WebKit.WKWebView webView, string executeJavaScriptFilePath)
#else
        public static async Task<string> ExecuteJavaScriptAsync(object wevView, string executeJavaScriptFilePath)
#endif
        {
            using var streamReader = new StreamReader(executeJavaScriptFilePath);

            var script = await streamReader.ReadToEndAsync();

#if WINDOWS
            return await webView.ExecuteScriptAsync(script);
#elif MACCATALYST || MACOS
            return (await webView.EvaluateJavaScriptAsync(script)).ToString();
#else
            return await Task.Run(() => { return ""; });
#endif
        }
    }
}
