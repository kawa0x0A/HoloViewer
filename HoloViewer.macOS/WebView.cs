using System;
using System.Reflection;
using Microsoft.MobileBlazorBindings.Elements;
using Microsoft.MobileBlazorBindings.WebView.Elements;

[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.macOS.WebView))]
namespace HoloViewer.macOS
{
    public class WebView : IWebView
    {
        private WebViewExtended CastWebView(BlazorWebView blazorWebView)
        {
            var nativeControl = blazorWebView.NativeControl;
            var type = nativeControl.GetType();

            return (WebViewExtended)(type.BaseType.GetField("_webView", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(nativeControl));
        }

        public bool CanPageBack(BlazorWebView blazorWebView)
        {
            return CastWebView(blazorWebView).CanGoBack;
        }

        public bool CanPageForward(BlazorWebView blazorWebView)
        {
            return CastWebView(blazorWebView).CanGoForward;
        }

        public string GetUrl(BlazorWebView blazorWebView)
        {
            return ((Xamarin.Forms.UrlWebViewSource)CastWebView(blazorWebView).Source).Url;
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

        public void Stop(BlazorWebView blazorWebView)
        {
            throw new NotImplementedException();
        }
    }
}
