using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.MobileBlazorBindings.Elements;

[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.Windows.ScreenCapture))]
namespace HoloViewer.Windows
{
    class ScreenCapture : IScreenCapture
    {
        private static int CalcStride (int widthPixel)
        {
            var bytesPerPixel = (PixelFormats.Bgra32.BitsPerPixel + 7) / 8;

            return (widthPixel * bytesPerPixel);
        }

        private static int CalcPixelBufferSize (int widthPixel, int heightPixel)
        {
            return (heightPixel * CalcStride(widthPixel));
        }

        private static async Task<byte[]> GetPixelBytesAsync (IScreenCapture.CaptureScreenBase captureScreen)
        {
            using var captureMemoryStream = new MemoryStream();

            captureMemoryStream.Write(await captureScreen.GetCaptureData());

            var pngBitmapDecoder = new PngBitmapDecoder(captureMemoryStream, BitmapCreateOptions.None, BitmapCacheOption.None);

            var captureBitmapSource = (BitmapSource)pngBitmapDecoder.Frames[0];

            var buffer = new byte[CalcPixelBufferSize(captureBitmapSource.PixelWidth, captureBitmapSource.PixelHeight)];

            captureBitmapSource.CopyPixels(buffer, CalcStride(captureBitmapSource.PixelWidth), 0);

            return buffer;
        }

        private static void SavePngFile (RenderTargetBitmap renderTargetBitmap, string pngFileFullPath)
        {
            var pngBitmapEncoder = new PngBitmapEncoder();

            pngBitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            SavePngFile(pngBitmapEncoder, pngFileFullPath);
        }

        private static void SavePngFile (byte[] pngData, string pngFileFullPath)
        {
            var pngBitmapEncoder = new PngBitmapEncoder();

            using var memoryStream = new MemoryStream(pngData, 0, pngData.Length);

            pngBitmapEncoder.Frames.Add(BitmapFrame.Create(memoryStream));

            SavePngFile(pngBitmapEncoder, pngFileFullPath);
        }

        private static void SavePngFile (PngBitmapEncoder pngBitmapEncoder, string pngFileFullPath)
        {
            if (!Directory.Exists(IScreenCapture.GetCaptureDirectoryFullPath()))
            {
                Directory.CreateDirectory(IScreenCapture.GetCaptureDirectoryFullPath());
            }

            using var fileStream = new FileStream(pngFileFullPath, FileMode.Create);

            pngBitmapEncoder.Save(fileStream);
        }

        private static async Task CaptureCombine (Rect rect, IScreenCapture.CaptureScreenBase[] captureWebViews)
        {
            var matrix = PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformToDevice;

            const int DPI = 96;

            double dpiX = DPI * matrix.M11;
            double dpiY = DPI * matrix.M22;

            var renderTargetBitmap = new RenderTargetBitmap((int)rect.Width, (int)rect.Height, dpiX, dpiY, PixelFormats.Pbgra32);

            foreach (var captureWebView in captureWebViews)
            {
                using var memoryStream = new MemoryStream();

                memoryStream.Write(await GetPixelBytesAsync(captureWebView));

                var bitmapSource = BitmapSource.Create(await captureWebView.GetWidthPixel(), await captureWebView.GetHeightPixel(), dpiX, dpiY, PixelFormats.Bgra32, null, memoryStream.ToArray(), CalcStride(await captureWebView.GetWidthPixel()));

                var drawingVisual = new DrawingVisual();

                using (var drawingContext = drawingVisual.RenderOpen())
                {
                    drawingContext.DrawImage(bitmapSource, new Rect(await captureWebView.GetOffsetPixelX(), await captureWebView.GetOffsetPixelY(), bitmapSource.PixelWidth, bitmapSource.PixelHeight));
                }

                renderTargetBitmap.Render(drawingVisual);
            }

            SavePngFile(renderTargetBitmap, IScreenCapture.GetCaptureFullPath(DateTime.Now));
        }

        private static async Task CaptureSeparate (IScreenCapture.CaptureScreenBase[] captureWebViews)
        {
            int imageFileSuffixNumber = 1;

            foreach (var captureWebView in captureWebViews)
            {
                SavePngFile(await captureWebView.GetCaptureData(), IScreenCapture.GetCaptureFullPath(DateTime.Now, imageFileSuffixNumber));

                imageFileSuffixNumber++;
            }
        }

        public async Task<int> GetYoutubePlayerWidth (BlazorWebView blazorWebView)
        {
            return int.Parse(await WebView.ExecuteJavascript(blazorWebView, IScreenCapture.GetWidthYoutubePlayerScriptPath));
        }

        public async Task<int> GetYoutubePlayerHeight (BlazorWebView blazorWebView)
        {
            return int.Parse(await WebView.ExecuteJavascript(blazorWebView, IScreenCapture.GetHeightYoutubePlayerScriptPath));
        }

        public async Task<bool> IsEnableYoutubeVideoPlayer (BlazorWebView blazorWebView)
        {
            return bool.Parse(await WebView.ExecuteJavascript(blazorWebView, IScreenCapture.IsEnableYoutubeVideoPlayerScriptPath));
        }

        public async Task<byte[]> GetCaptureWebViewData (BlazorWebView blazorWebView)
        {
            using var memoryStream = new MemoryStream();

            await WebView.CastWebView(blazorWebView).CoreWebView2.CapturePreviewAsync(Microsoft.Web.WebView2.Core.CoreWebView2CapturePreviewImageFormat.Png, memoryStream);

            return memoryStream.ToArray();
        }

        public async Task<byte[]> GetCaptureYoutubePlayerData (BlazorWebView blazorWebView)
        {
            var pngDataUri = await WebView.ExecuteJavascript(blazorWebView, IScreenCapture.CaptureYoutubeVideoPlayerScriptPath);

            return Convert.FromBase64String(IScreenCapture.ConvertUriToBase64(pngDataUri));
        }

        public async Task<byte[]> GetCaptureYoutubeBackgroundSlateImageData (BlazorWebView blazorWebView)
        {
            await WebView.ExecuteJavascript(blazorWebView, IScreenCapture.LoadYoutubeBackgroundSlateImageScriptPath);

            while (!bool.Parse(await WebView.ExecuteJavascript(blazorWebView, IScreenCapture.IsLoadCompleteImage))) { }

            var pngDataUri = await WebView.ExecuteJavascript(blazorWebView, IScreenCapture.CaptureYoutubeBackgroundSlateImageScriptPath);

            return Convert.FromBase64String(IScreenCapture.ConvertUriToBase64(pngDataUri));
        }

        private static async Task ExecuteCapture (IScreenCapture.CaptureSettingBase captureSetting, IScreenCapture.CaptureScreenBase[] captureScreens)
        {
            switch (captureSetting.CurrentCaptureScreenConvertType)
            {
                case IScreenCapture.CaptureScreenConvertType.Combine:
                    await CaptureCombine(new Rect(0, 0, await captureSetting.GetImageWidth(), await captureSetting.GetImageHeight()), captureScreens.ToArray());
                    break;

                case IScreenCapture.CaptureScreenConvertType.Separate:
                    await CaptureSeparate(captureScreens.ToArray());
                    break;
            }
        }

        public async Task CaptureSingle (IScreenCapture.CaptureSetting_Single captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenSingle.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureHorizontal2 (IScreenCapture.CaptureSetting_Horizontal2Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenHorizontal2.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureVertical2 (IScreenCapture.CaptureSetting_Vertical2Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenVertical2.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureHorizontal3 (IScreenCapture.CaptureSetting_Horizontal3Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenHorizontal3.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureVertical3 (IScreenCapture.CaptureSetting_Vertical3Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenVertical3.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureCustom3_1 (IScreenCapture.CaptureSetting_Custom3_1Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenCustom3_1.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureCustom3_2 (IScreenCapture.CaptureSetting_Custom3_2Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenCustom3_2.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureHorizontal4 (IScreenCapture.CaptureSetting_Horizontal4Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenHorizontal4.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureVertical4 (IScreenCapture.CaptureSetting_Vertical4Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenVertical4.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureCustom4 (IScreenCapture.CaptureSetting_Custom4Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenCustom4.CreateCaptureScreens(captureSetting));
        }
    }
}
