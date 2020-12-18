using System.Threading.Tasks;
using System.IO;
using Microsoft.MobileBlazorBindings.Elements;
using Microsoft.Web.WebView2.Wpf;

[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.Windows.WebView))]
namespace HoloViewer.Windows
{
    class WebView : IWebView
    {
        private bool isNavigating = false;

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

        public void Navigate (BlazorWebView blazorWebView, string url)
        {
            (CastWebView(blazorWebView)).CoreWebView2.Navigate(url);
        }
    }
}
