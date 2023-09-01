using Microsoft.AspNetCore.Components.WebView.Maui;

namespace HoloViewer;

public partial class ScreenCustom3_1 : ContentPage
{
    public ScreenCustom3_1()
    {
        InitializeComponent();

        captureToolbar.AllocateCaptureTargetWebview(3);

        captureToolbar.BindingModelView.IsCapture = new bool[3] { true, true, true };

        captureToolbar.CapturePositionFunction = CalcScreenCapturePosition;

        captureButtons.BindingContext = captureToolbar.BindingModelView;
    }

    private void BlazorWebView1_BlazorWebViewInitialized(object sender, Microsoft.AspNetCore.Components.WebView.BlazorWebViewInitializedEventArgs e)
    {
        captureToolbar.CaptureTargetWebviews[0] = e.WebView;

        webviewToolbar1.WebView = e.WebView;

        ((BlazorWebView)sender).Loaded += (_, _) => webviewToolbar1.AddNavigationEvent();
    }

    private void BlazorWebView2_BlazorWebViewInitialized(object sender, Microsoft.AspNetCore.Components.WebView.BlazorWebViewInitializedEventArgs e)
    {
        captureToolbar.CaptureTargetWebviews[1] = e.WebView;

        webviewToolbar2.WebView = e.WebView;

        ((BlazorWebView)sender).Loaded += (_, _) => webviewToolbar2.AddNavigationEvent();
    }

    private void BlazorWebView3_BlazorWebViewInitialized(object sender, Microsoft.AspNetCore.Components.WebView.BlazorWebViewInitializedEventArgs e)
    {
        captureToolbar.CaptureTargetWebviews[2] = e.WebView;

        webviewToolbar3.WebView = e.WebView;

        ((BlazorWebView)sender).Loaded += (_, _) => webviewToolbar3.AddNavigationEvent();
    }

    private void BlazorWebView_UrlLoading(object sender, Microsoft.AspNetCore.Components.WebView.UrlLoadingEventArgs e)
    {
        e.UrlLoadingStrategy = Microsoft.AspNetCore.Components.WebView.UrlLoadingStrategy.OpenInWebView;
    }

    private async Task<Rect[]> CalcScreenCapturePosition()
    {
        var positions = new List<Rect>();

        if (captureToolbar.BindingModelView.IsCapture[0])
        {
            var w = captureToolbar.BindingModelView.IsCaptureYoutubePlayerOnly ? await ScreenCapture.GetYoutubePlayerWidth(captureToolbar.CaptureTargetWebviews[0]) : ScreenCapture.GetWebViewWidth(captureToolbar.CaptureTargetWebviews[0]);
            var h = captureToolbar.BindingModelView.IsCaptureYoutubePlayerOnly ? await ScreenCapture.GetYoutubePlayerHeight(captureToolbar.CaptureTargetWebviews[0]) : ScreenCapture.GetWebViewHeight(captureToolbar.CaptureTargetWebviews[0]);

            positions.Add(new Rect(0, 0, w, h));
        }

        if (captureToolbar.BindingModelView.IsCapture[1])
        {
            var x = 0;
            var y = captureToolbar.BindingModelView.IsCapture[0] ? captureToolbar.BindingModelView.IsCaptureYoutubePlayerOnly ? await ScreenCapture.GetYoutubePlayerHeight(captureToolbar.CaptureTargetWebviews[0]) : ScreenCapture.GetWebViewHeight(captureToolbar.CaptureTargetWebviews[0]) : 0;
            var w = captureToolbar.BindingModelView.IsCaptureYoutubePlayerOnly ? await ScreenCapture.GetYoutubePlayerWidth(captureToolbar.CaptureTargetWebviews[1]) : ScreenCapture.GetWebViewWidth(captureToolbar.CaptureTargetWebviews[1]);
            var h = captureToolbar.BindingModelView.IsCaptureYoutubePlayerOnly ? await ScreenCapture.GetYoutubePlayerHeight(captureToolbar.CaptureTargetWebviews[1]) : ScreenCapture.GetWebViewHeight(captureToolbar.CaptureTargetWebviews[1]);

            positions.Add(new Rect(x, y, w, h));
        }

        if (captureToolbar.BindingModelView.IsCapture[2])
        {
            var x = captureToolbar.BindingModelView.IsCapture[1] ? captureToolbar.BindingModelView.IsCaptureYoutubePlayerOnly ? await ScreenCapture.GetYoutubePlayerWidth(captureToolbar.CaptureTargetWebviews[1]) : ScreenCapture.GetWebViewWidth(captureToolbar.CaptureTargetWebviews[1]) : 0;
            var y = captureToolbar.BindingModelView.IsCapture[0] ? captureToolbar.BindingModelView.IsCaptureYoutubePlayerOnly ? await ScreenCapture.GetYoutubePlayerHeight(captureToolbar.CaptureTargetWebviews[0]) : ScreenCapture.GetWebViewHeight(captureToolbar.CaptureTargetWebviews[0]) : 0;
            var w = captureToolbar.BindingModelView.IsCaptureYoutubePlayerOnly ? await ScreenCapture.GetYoutubePlayerWidth(captureToolbar.CaptureTargetWebviews[2]) : ScreenCapture.GetWebViewWidth(captureToolbar.CaptureTargetWebviews[2]);
            var h = captureToolbar.BindingModelView.IsCaptureYoutubePlayerOnly ? await ScreenCapture.GetYoutubePlayerHeight(captureToolbar.CaptureTargetWebviews[2]) : ScreenCapture.GetWebViewHeight(captureToolbar.CaptureTargetWebviews[2]);

            positions.Add(new Rect(x, y, w, h));
        }

        return positions.ToArray();
    }
}
