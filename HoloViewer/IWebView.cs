using Microsoft.MobileBlazorBindings.Elements;

namespace HoloViewer
{
    public interface IWebView
    {
        bool CanPageBack (BlazorWebView blazorWebView);

        bool CanPageForward (BlazorWebView blazorWebView);

        void PageBack (BlazorWebView blazorWebView);

        void PageForward (BlazorWebView blazorWebView);

        void Reload (BlazorWebView blazorWebView);

        void Stop (BlazorWebView blazorWebView);

        string GetUrl (BlazorWebView blazorWebView);
    }
}
