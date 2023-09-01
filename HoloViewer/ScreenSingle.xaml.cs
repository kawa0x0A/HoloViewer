using Microsoft.AspNetCore.Components.WebView.Maui;

namespace HoloViewer;

public partial class ScreenSingle : ContentPage
{
    public ScreenSingle()
    {
        InitializeComponent();

        captureToolbar.AllocateCaptureTargetWebview(1);

        captureToolbar.CapturePositionFunction = CalcScreenCapturePosition;
    }

    private void BlazorWebView_BlazorWebViewInitialized(object sender, Microsoft.AspNetCore.Components.WebView.BlazorWebViewInitializedEventArgs e)
    {
        captureToolbar.CaptureTargetWebviews[0] = e.WebView;

        webviewToolbar.WebView = e.WebView;

        ((BlazorWebView)sender).Loaded += (_, _) => webviewToolbar.AddNavigationEvent();
    }

    private void BlazorWebView_UrlLoading(object sender, Microsoft.AspNetCore.Components.WebView.UrlLoadingEventArgs e)
    {
        e.UrlLoadingStrategy = Microsoft.AspNetCore.Components.WebView.UrlLoadingStrategy.OpenInWebView;
    }

    private async Task<Rect[]> CalcScreenCapturePosition()
    {
        var positions = new List<Rect>();

        var w = captureToolbar.BindingModelView.IsCaptureYoutubePlayerOnly ? await ScreenCapture.GetYoutubePlayerWidth(captureToolbar.CaptureTargetWebviews[0]) : ScreenCapture.GetWebViewWidth(captureToolbar.CaptureTargetWebviews[0]);
        var h = captureToolbar.BindingModelView.IsCaptureYoutubePlayerOnly ? await ScreenCapture.GetYoutubePlayerHeight(captureToolbar.CaptureTargetWebviews[0]) : ScreenCapture.GetWebViewHeight(captureToolbar.CaptureTargetWebviews[0]);

        positions.Add(new Rect(0, 0, w, h));

        return positions.ToArray();
    }
}
