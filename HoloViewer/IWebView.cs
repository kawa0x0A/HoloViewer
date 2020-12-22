using Microsoft.MobileBlazorBindings.Elements;
using System.Threading.Tasks;

namespace HoloViewer
{
    public interface IWebView
    {
        Task InitializeAsync (BlazorWebView blazorWebView);

        void AddEventFunction (BlazorWebView blazorWebView, WebViewToolbar webViewToolbar);

        void RemoveEventFunction (BlazorWebView blazorWebView);

        bool CanPageBack (BlazorWebView blazorWebView);

        bool CanPageForward (BlazorWebView blazorWebView);

        void PageBack (BlazorWebView blazorWebView);

        void PageForward (BlazorWebView blazorWebView);

        void Reload (BlazorWebView blazorWebView);

        void Stop (BlazorWebView blazorWebView);

        bool IsLoading (BlazorWebView blazorWebView);

        string GetTitle (BlazorWebView blazorWebView);

        string GetUrl (BlazorWebView blazorWebView);

        void Navigate (BlazorWebView blazorWebView, string url);
    }
}
