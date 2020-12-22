using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.MobileBlazorBindings.Elements;
using Microsoft.Web.WebView2.Wpf;
using Microsoft.Web.WebView2.Core;

[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.Windows.WebView))]
namespace HoloViewer.Windows
{
    class WebView : IWebView
    {
        private bool isNavigating = false;
        private EventHandler<CoreWebView2NavigationStartingEventArgs> navigationStartingAction;
        private EventHandler<CoreWebView2NavigationCompletedEventArgs> navigationComplatedAction;
        private EventHandler<CoreWebView2SourceChangedEventArgs> sourceChangedAction;

        public static WebView2 CastWebView (BlazorWebView blazorWebView)
        {
            var content = ((Microsoft.MobileBlazorBindings.WebView.Elements.MobileBlazorBindingsBlazorWebView)blazorWebView.NativeControl).Content;
            var type = content.GetType();

            var webViewExtendedAnaheimRenderer = (Microsoft.MobileBlazorBindings.WebView.Windows.WebViewExtendedAnaheimRenderer)type.GetProperty("EffectControlProvider").GetValue(content);

            return webViewExtendedAnaheimRenderer.Control;
        }

        public static async Task<string> ExecuteJavascript (BlazorWebView blazorWebView, string filePath)
        {
            string script = "";

            using (var streamReader = new StreamReader(filePath))
            {
                script = streamReader.ReadToEnd();
            }

            return await CastWebView(blazorWebView).ExecuteScriptAsync(script);
        }

        public async Task InitializeAsync (BlazorWebView blazorWebView)
        {
            await CastWebView(blazorWebView).EnsureCoreWebView2Async();
        }

        public void AddEventFunction (BlazorWebView blazorWebView, WebViewToolbar webViewToolbar)
        {
            navigationStartingAction = (sender, e) => { isNavigating = true; webViewToolbar.UpdateUrl(); webViewToolbar.UpdateReloadButton(); webViewToolbar.UpdateStateHasChanged(); };
            navigationComplatedAction = (sender, e) => { isNavigating = false; webViewToolbar.UpdateTitle(); webViewToolbar.UpdateReloadButton(); webViewToolbar.UpdateStateHasChanged(); };
            sourceChangedAction = (sender, e) => { webViewToolbar.UpdateTitle(); webViewToolbar.UpdateUrl(); webViewToolbar.UpdateStateHasChanged(); };

            var webview2 = CastWebView(blazorWebView);

            webview2.NavigationStarting += navigationStartingAction;
            webview2.NavigationCompleted += navigationComplatedAction;
            webview2.SourceChanged += sourceChangedAction;
            webview2.CoreWebView2.FrameNavigationStarting += navigationStartingAction;
            webview2.CoreWebView2.FrameNavigationCompleted += navigationComplatedAction;
        }

        public void RemoveEventFunction (BlazorWebView blazorWebView)
        {
            var webview2 = CastWebView(blazorWebView);

            if (navigationStartingAction != null)
            {
                webview2.NavigationStarting -= navigationStartingAction;
                webview2.CoreWebView2.FrameNavigationStarting -= navigationStartingAction;
            }

            if (navigationComplatedAction != null)
            {
                webview2.NavigationCompleted -= navigationComplatedAction;
                webview2.CoreWebView2.FrameNavigationCompleted -= navigationComplatedAction;
            }

            if (sourceChangedAction != null)
            {
                webview2.SourceChanged -= sourceChangedAction;
            }
        }

        public bool CanPageBack (BlazorWebView blazorWebView)
        {
            return (CastWebView(blazorWebView)).CanGoBack;
        }

        public bool CanPageForward (BlazorWebView blazorWebView)
        {
            return (CastWebView(blazorWebView)).CanGoForward;
        }

        public void PageBack (BlazorWebView blazorWebView)
        {
            (CastWebView(blazorWebView)).GoBack();
        }

        public void PageForward (BlazorWebView blazorWebView)
        {
            (CastWebView(blazorWebView)).GoForward();
        }

        public void Reload (BlazorWebView blazorWebView)
        {
            (CastWebView(blazorWebView)).Reload();
        }

        public void Stop (BlazorWebView blazorWebView)
        {
            (CastWebView(blazorWebView)).Stop();
        }

        public bool IsLoading (BlazorWebView blazorWebView)
        {
            return isNavigating;
        }

        public string GetUrl (BlazorWebView blazorWebView)
        {
            return (CastWebView(blazorWebView)).Source.AbsoluteUri;
        }

        public string GetTitle (BlazorWebView blazorWebView)
        {
            return (CastWebView(blazorWebView)).CoreWebView2.DocumentTitle;
        }

        public void Navigate (BlazorWebView blazorWebView, string url)
        {
            (CastWebView(blazorWebView)).CoreWebView2.Navigate(url);
        }
    }
}
