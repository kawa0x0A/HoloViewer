using Microsoft.AspNetCore.Components.WebView.Maui;

namespace HoloViewer;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        Initialized();

        captureToolbar.AllocateCaptureTargetWebview(1);

        captureToolbar.CapturePositionFunction = CalcScreenCapturePosition;
    }

    private static void Initialized()
    {
        if (ApplicationSettings.ExistsApplicationSettingsFile())
        {
            ApplicationSettings.Load();
        }

        if (!string.IsNullOrEmpty(ApplicationSettings.Current.StartupPageUrl))
        {
            Pages.Root.StartUpUrl = ApplicationSettings.Current.StartupPageUrl;
        }

        Application.Current.UserAppTheme = ApplicationSettings.Current.Theme;
    }

    private static async Task OnInitializedAsync()
    {
        var args = Environment.GetCommandLineArgs();

        if ((args.Length > 1) && (args[1] == "/Update"))
        {
            UpdateCheck.DeleteUpdateFiles();
            await UpdateCheck.UpdateComplete();
        }
        else
        {
            try
            {
                var update = new UpdateCheck();

                if (ApplicationSettings.Current.IsEnableUpdateCheck && (await update.IsUpdateable()) && (await update.AskExecuteUpdate(DeviceInfo.Current.Platform)))
                {
                    if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
                    {
                        await update.Update();
                    }
                    else if ((DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst) || (DeviceInfo.Current.Platform == DevicePlatform.macOS))
                    {
                        var program = new UpdateCheckLibrary.Program();

                        await Browser.Default.OpenAsync(program.GetDownloadArchiveUrl(DevicePlatform.macOS));
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await OnInitializedAsync();
    }

    private void BlazorWebView_BlazorWebViewInitialized(object sender, Microsoft.AspNetCore.Components.WebView.BlazorWebViewInitializedEventArgs e)
    {
        captureToolbar.CaptureTargetWebviews[0] = e.WebView;

        webviewToolbar.WebView = e.WebView;

        ((BlazorWebView)sender).Loaded += (_, _) => { webviewToolbar.AddNavigationEvent(); };
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
