using System;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.MobileBlazorBindings.Elements;
using Xamarin.Forms;
using AppKit;
using SkiaSharp;

[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.macOS.ScreenCapture))]
namespace HoloViewer.macOS
{
    public class ScreenCapture : IScreenCapture
    {
        private static void SavePngFile(byte[] pngData, string pngFileFullPath)
        {
            if (!Directory.Exists(IScreenCapture.GetCaptureDirectoryFullPath()))
            {
                Directory.CreateDirectory(IScreenCapture.GetCaptureDirectoryFullPath());
            }

            using (var fileStream = new FileStream(pngFileFullPath, FileMode.Create))
            {
                fileStream.Write(pngData);
            }
        }

        private static async Task CaptureCombine(Rect rect, IScreenCapture.CaptureScreenBase[] captureScreens)
        {
            using(var skSurface = SKSurface.Create(new SKImageInfo((int)rect.Width, (int)rect.Height)))
            {
                foreach(var captureScreen in captureScreens)
                {
                    var pngData = await captureScreen.GetCaptureData();

                    using(var memoryStream = new MemoryStream(pngData))
                    {
                        var skBitmap = SKBitmap.Decode(pngData);

                        skSurface.Canvas.DrawBitmap(skBitmap, new SKPoint(await captureScreen.GetOffsetPixelX(), await captureScreen.GetOffsetPixelY()));
                    }
                }

                using (var skImage = skSurface.Snapshot())
                using (var skData = skImage.Encode(SKEncodedImageFormat.Png, 100))
                {
                    SavePngFile(skData.ToArray(), IScreenCapture.GetCaptureFullPath(DateTime.Now));
                }
            }
        }

        private static async Task CaptureSeparate(IScreenCapture.CaptureScreenBase[] captureScreens)
        {
            int imageFileSuffixNumber = 1;

            foreach(var captureScreen in captureScreens)
            {
                SavePngFile(await captureScreen.GetCaptureData(), IScreenCapture.GetCaptureFullPath(DateTime.Now, imageFileSuffixNumber));

                imageFileSuffixNumber++;
            }
        }

        private static async Task ExecuteCapture(IScreenCapture.CaptureSettingBase captureSetting, IScreenCapture.CaptureScreenBase[] captureScreens)
        {
            switch(captureSetting.CurrentCaptureScreenConvertType)
            {
                case IScreenCapture.CaptureScreenConvertType.Combine:
                    await CaptureCombine(new Rect(0, 0, await captureSetting.GetImageWidth(), await captureSetting.GetImageHeight()), captureScreens);
                    break;

                case IScreenCapture.CaptureScreenConvertType.Separate:
                    await CaptureSeparate(captureScreens);
                    break;
            }
        }

        public async Task<byte[]> GetCaptureWebViewData(BlazorWebView blazorWebView)
        {
            var image = await WebView.CastWebView(blazorWebView).TakeSnapshotAsync(new WebKit.WKSnapshotConfiguration());

            var nsBitmapImageRep = new NSBitmapImageRep(image.CGImage);
            var nsdata = nsBitmapImageRep.RepresentationUsingTypeProperties(NSBitmapImageFileType.Png);
            var data = new byte[nsdata.Length];

            Marshal.Copy(nsdata.Bytes, data, 0, System.Convert.ToInt32(nsdata.Length));

            return data;
        }

        public async Task<byte[]> GetCaptureYoutubePlayerData(BlazorWebView blazorWebView)
        {
            var pngDataUri = await WebView.ExecuteJavascript(blazorWebView, IScreenCapture.CaptureYoutubePlayerScriptPath);

            return Convert.FromBase64String(IScreenCapture.ConvertUriToBase64(pngDataUri));
        }

        public async Task<int> GetYoutubePlayerWidth(BlazorWebView blazorWebView)
        {
            return int.Parse(await WebView.ExecuteJavascript(blazorWebView, IScreenCapture.GetWidthYoutubePlayerScriptPath));
        }

        public async Task<int> GetYoutubePlayerHeight(BlazorWebView blazorWebView)
        {
            return int.Parse(await WebView.ExecuteJavascript(blazorWebView, IScreenCapture.GetHeightYoutubePlayerScriptPath));
        }

        public async Task CaptureSingle(IScreenCapture.CaptureSetting_Single captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenSingle.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureHorizontal2(IScreenCapture.CaptureSetting_Horizontal2Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenHorizontal2.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureVertical2(IScreenCapture.CaptureSetting_Vertical2Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenVertical2.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureHorizontal3(IScreenCapture.CaptureSetting_Horizontal3Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenHorizontal3.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureVertical3(IScreenCapture.CaptureSetting_Vertical3Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenVertical3.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureCustom3_1(IScreenCapture.CaptureSetting_Custom3_1Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenCustom3_1.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureCustom3_2(IScreenCapture.CaptureSetting_Custom3_2Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenCustom3_2.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureHorizontal4(IScreenCapture.CaptureSetting_Horizontal4Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenHorizontal4.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureVertical4(IScreenCapture.CaptureSetting_Vertical4Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenVertical4.CreateCaptureScreens(captureSetting));
        }

        public async Task CaptureCustom4(IScreenCapture.CaptureSetting_Custom4Mode captureSetting)
        {
            await ExecuteCapture(captureSetting, IScreenCapture.CaptureScreenCustom4.CreateCaptureScreens(captureSetting));
        }
    }
}
