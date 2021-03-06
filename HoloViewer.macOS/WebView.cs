using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using Microsoft.MobileBlazorBindings.Elements;
using WebKit;

[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.macOS.WebView))]
namespace HoloViewer.macOS
{
    public class WebView : IWebView
    {
        private class NavigationDelegate : WKNavigationDelegate
        {
            private readonly WebViewToolbar webViewToolbar;

            public NavigationDelegate(WebViewToolbar webViewToolbar)
            {
                this.webViewToolbar = webViewToolbar;
            }

            public override void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
            {
                webViewToolbar.UpdateUrl();
                webViewToolbar.UpdateReloadButton();
                webViewToolbar.UpdateStateHasChanged();
            }

            public override void DidFailNavigation(WKWebView webView, WKNavigation navigation, NSError error)
            {
                webViewToolbar.UpdateReloadButton();
                webViewToolbar.UpdateStateHasChanged();
            }

            public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
            {
                webViewToolbar.UpdateTitle();
                webViewToolbar.UpdateReloadButton();
                webViewToolbar.UpdateStateHasChanged();
            }
        }

        public static WKWebView CastWebView(BlazorWebView blazorWebView)
        {
            var content = ((Microsoft.MobileBlazorBindings.WebView.Elements.MobileBlazorBindingsBlazorWebView)blazorWebView.NativeControl).Content;
            var type = content.GetType();

            var webViewExtendedWKWebViewRenderer = (Microsoft.MobileBlazorBindings.WebView.macOS.WebViewExtendedWKWebViewRenderer)type.GetProperty("EffectControlProvider").GetValue(content);

            return webViewExtendedWKWebViewRenderer.Control;
        }

        public static async Task<string> ExecuteJavascript(BlazorWebView blazorWebView, string filePath)
        {
            string script = "";

            using(var streamReader = new StreamReader(filePath))
            {
                script = streamReader.ReadToEnd();
            }

            return (await CastWebView(blazorWebView).EvaluateJavaScriptAsync(script)).ToString();
        }

        public Task InitializeAsync (BlazorWebView blazorWebView)
        {
            return Task.Run(() => { });
        }

        public void AddEventFunction (BlazorWebView blazorWebView, WebViewToolbar webViewToolbar)
        {
            var wkWebView = CastWebView(blazorWebView);

            wkWebView.NavigationDelegate = new NavigationDelegate(webViewToolbar);
        }

        public void RemoveEventFunction (BlazorWebView blazorWebView)
        {
        }

        public bool CanPageBack(BlazorWebView blazorWebView)
        {
            return CastWebView(blazorWebView).CanGoBack;
        }

        public bool CanPageForward(BlazorWebView blazorWebView)
        {
            return CastWebView(blazorWebView).CanGoForward;
        }

        public string GetTitle (BlazorWebView blazorWebView)
        {
            return CastWebView(blazorWebView).Title;
        }

        public string GetUrl(BlazorWebView blazorWebView)
        {
            return CastWebView(blazorWebView).Url.ToString();
        }

        public void PageBack(BlazorWebView blazorWebView)
        {
            CastWebView(blazorWebView).GoBack();
        }

        public void PageForward(BlazorWebView blazorWebView)
        {
            CastWebView(blazorWebView).GoForward();
        }

        public void Reload(BlazorWebView blazorWebView)
        {
            CastWebView(blazorWebView).Reload();
        }

        public bool IsLoading (BlazorWebView blazorWebView)
        {
            return CastWebView(blazorWebView).IsLoading;
        }

        public void Stop(BlazorWebView blazorWebView)
        {
            CastWebView(blazorWebView).StopLoading();
        }

        public void Navigate (BlazorWebView blazorWebView, string url)
        {
            CastWebView(blazorWebView).LoadRequest(new Foundation.NSUrlRequest(new Foundation.NSUrl(url)));
        }
    }
}
