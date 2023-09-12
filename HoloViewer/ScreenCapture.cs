using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Maui.Graphics.Skia;

namespace HoloViewer
{
    public partial class ScreenCapture
    {
        private static partial class CustomRegex
        {
            [GeneratedRegex(@"https://www.youtube.com/watch\?.+")]
            private static partial Regex RegexYoutubeUrl();

            [GeneratedRegex(@"""data:image/png;base64,(?<data>.+)""")]
            private static partial Regex RegexPngUri();

            public static bool IsMatchYoutubeUrl(string url)
            {
                return RegexYoutubeUrl().IsMatch(url);
            }

            public static string MatchPngUri(string uri)
            {
                return RegexPngUri().Match(uri).Groups["data"].Value;
            }
        }

        private static readonly string ScriptPath = $"{WebViewUtility.ScriptRootPath}/capture/";

        private static readonly string CaptureYoutubeVideoPlayerScriptPath = ScriptPath + "CaptureYoutubePlayer.js";

        private static readonly string CaptureYoutubeBackgroundSlateImageScriptPath = ScriptPath + "CaptureBackgroundSlateImage.js";

        private static readonly string LoadYoutubeBackgroundSlateImageScriptPath = ScriptPath + "LoadBackgroundSlateImage.js";

        private static readonly string IsEnableYoutubeVideoPlayerScriptPath = ScriptPath + "IsEnableYoutubeVideoPlayer.js";

        private static readonly string IsLoadCompleteImage = ScriptPath + "IsLoadCompleteImage.js";

        private static readonly string GetWidthYoutubePlayerScriptPath = ScriptPath + "GetWidthYoutubePlayer.js";

        private static readonly string GetHeightYoutubePlayerScriptPath = ScriptPath + "GetHeightYoutubePlayer.js";

        private const string CaptureDirectoryName = "Capture";

        private const string CaptureFileNameFormat = "yyyyMMdd_HHmmss";

        private const string CaptureFileExtension = ".png";

        private static string GetCaptureFileName(DateTime dateTime)
        {
            return (dateTime.ToString(CaptureFileNameFormat) + CaptureFileExtension);
        }

        private static string GetCaptureFileName(DateTime dateTime, int index)
        {
            return (dateTime.ToString(CaptureFileNameFormat) + "_" + index.ToString() + CaptureFileExtension);
        }

        public static string GetCaptureDirectoryFullPath()
        {
#if WINDOWS
            return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CaptureDirectoryName));
#elif MACCATALYST || MACOS
            return Path.Combine(Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)), CaptureDirectoryName);
#else
            return "";
#endif
        }

        public static string GetCaptureFullPath(DateTime dateTime)
        {
            if (Directory.Exists(ApplicationSettings.Current.CaptureSavePath))
            {
                return Path.Combine(ApplicationSettings.Current.CaptureSavePath, GetCaptureFileName(dateTime));
            }
            else
            {
                return Path.Combine(GetCaptureDirectoryFullPath(), GetCaptureFileName(dateTime));
            }
        }

        public static string GetCaptureFullPath(DateTime dateTime, int index)
        {
            if (Directory.Exists(ApplicationSettings.Current.CaptureSavePath))
            {
                return Path.Combine(ApplicationSettings.Current.CaptureSavePath, GetCaptureFileName(dateTime, index));
            }
            else
            {
                return Path.Combine(GetCaptureDirectoryFullPath(), GetCaptureFileName(dateTime, index));
            }
        }

        private static void SavePngFile(byte[] pngData, string pngFileFullPath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(pngFileFullPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(pngFileFullPath));
            }

            using var fileStream = new FileStream(pngFileFullPath, FileMode.Create);

            fileStream.Write(pngData);
        }

#if WINDOWS
        public static async Task CaptureCombine(Microsoft.UI.Xaml.Controls.WebView2[] webViews, bool isCaptureYoutubePlayerOnly, Rect[] rects)
#elif MACCATALYST || MACOS
        public static async Task CaptureCombine(WebKit.WKWebView[] webViews, bool isCaptureYoutubePlayerOnly, Rect[] rects)
#else
        public static void CaptureCombine(object[] webViews, bool isCaptureYoutubePlayerOnly, object[] rects)
#endif
        {
#if WINDOWS || MACCATALYST || MACOS
            int canvasWidth = rects.Select(r => (int)r.Right).Max();
            int canvasHeight = rects.Select(r => (int)r.Bottom).Max();

            using var skiaBitmap = new SkiaBitmapExportContext(canvasWidth, canvasHeight, 1.0f);

            for (int i = 0; i < webViews.Length; i++)
            {
                var imageByteData = (isCaptureYoutubePlayerOnly && CustomRegex.IsMatchYoutubeUrl(WebViewUtility.GetCurrentUrl(webViews[i]))) ? (await IsEnableYoutubeVideoPlayer(webViews[i])) ? await GetCaptureYoutubePlayerData(webViews[i]) : await GetCaptureWebViewData(webViews[i]) : await GetCaptureYoutubeBackgroundSlateImageData(webViews[i]);
                var image = new SkiaImage(SkiaSharp.SKBitmap.Decode(imageByteData));
                var rect = rects[i];

                skiaBitmap.Canvas.DrawImage(image, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
            }

            if (!Directory.Exists(GetCaptureDirectoryFullPath()))
            {
                Directory.CreateDirectory(GetCaptureDirectoryFullPath());
            }

            skiaBitmap.WriteToFile(GetCaptureFullPath(DateTime.Now));
#endif
        }

#if WINDOWS
        public static async Task CaptureSeparate(Microsoft.UI.Xaml.Controls.WebView2[] webViews, bool isCaptureYoutubePlayerOnly)
#elif MACCATALYST || MACOS
        public static async Task CaptureSeparate(WebKit.WKWebView[] webViews, bool isCaptureYoutubePlayerOnly)
#else
        public static void CaptureSeparate(object[] webViews, bool isCaptureYoutubePlayerOnly)
#endif
        {
#if WINDOWS || MACCATALYST || MACOS
            int imageFileSuffixNumber = 1;

            foreach (var webView in webViews)
            {
                SavePngFile((isCaptureYoutubePlayerOnly && CustomRegex.IsMatchYoutubeUrl(WebViewUtility.GetCurrentUrl(webView))) ? (await IsEnableYoutubeVideoPlayer(webView)) ? await GetCaptureYoutubePlayerData(webView) : await GetCaptureYoutubeBackgroundSlateImageData(webView) : await GetCaptureWebViewData(webView), GetCaptureFullPath(DateTime.Now, imageFileSuffixNumber));

                imageFileSuffixNumber++;
            }
#endif
        }

#if WINDOWS
        public static int GetWebViewWidth(Microsoft.UI.Xaml.Controls.WebView2 webView)
#elif MACCATALYST || MACOS
        public static int GetWebViewWidth(WebKit.WKWebView webView)
#else
        public static int GetWebViewWidth(object webView)
#endif
        {
#if WINDOWS
            return (int)webView.ActualWidth;
#elif MACCATALYST || MACOS
            return (int)webView.Frame.Width;
#else
            return 0;
#endif
        }

#if WINDOWS
        public static async Task<int> GetYoutubePlayerWidth (Microsoft.UI.Xaml.Controls.WebView2 webView)
#elif MACCATALYST || MACOS
        public static async Task<int> GetYoutubePlayerWidth (WebKit.WKWebView webView)
#else
        public static int GetYoutubePlayerWidth (object webView)
#endif
        {
#if WINDOWS || MACCATALYST || MACOS
            return int.Parse(await WebViewUtility.ExecuteJavaScriptAsync(webView, GetWidthYoutubePlayerScriptPath));
#else
            return 0;
#endif
        }

#if WINDOWS
        public static int GetWebViewHeight(Microsoft.UI.Xaml.Controls.WebView2 webView)
#elif MACCATALYST || MACOS
        public static int GetWebViewHeight(WebKit.WKWebView webView)
#else
        public static int GetWebViewHeight(object webView)
#endif
        {
#if WINDOWS
            return (int)webView.ActualHeight;
#elif MACCATALYST || MACOS
            return (int)webView.Frame.Height;
#else
            return 0;
#endif
        }

#if WINDOWS
        public static async Task<int> GetYoutubePlayerHeight (Microsoft.UI.Xaml.Controls.WebView2 webView)
#elif MACCATALYST || MACOS
        public static async Task<int> GetYoutubePlayerHeight (WebKit.WKWebView webView)
#else
        public static int GetYoutubePlayerHeight (object webView)
#endif
        {
#if WINDOWS || MACCATALYST || MACOS
            return int.Parse(await WebViewUtility.ExecuteJavaScriptAsync(webView, GetHeightYoutubePlayerScriptPath));
#else
            return 0;
#endif
        }

#if WINDOWS
        public static async Task<bool> IsEnableYoutubeVideoPlayer (Microsoft.UI.Xaml.Controls.WebView2 webView)
#elif MACCATALYST || MACOS
        public static async Task<bool> IsEnableYoutubeVideoPlayer (WebKit.WKWebView webView)
#else
        public static bool IsEnableYoutubeVideoPlayer (object webView)
#endif
        {
#if WINDOWS || MACCATALYST || MACOS
            return bool.Parse(await WebViewUtility.ExecuteJavaScriptAsync(webView, IsEnableYoutubeVideoPlayerScriptPath));
#else
            return false;
#endif
        }

#if WINDOWS
        public static async Task<byte[]> GetCaptureWebViewData (Microsoft.UI.Xaml.Controls.WebView2 webView)
#elif MACCATALYST || MACOS
        public static async Task<byte[]> GetCaptureWebViewData (WebKit.WKWebView webView)
#else
        public static byte[] GetCaptureWebViewData (object webView)
#endif
        {
            using var memoryStream = new MemoryStream();
#if WINDOWS
            using var randomAccessStream = new Windows.Storage.Streams.InMemoryRandomAccessStream();

            await webView.CoreWebView2.CapturePreviewAsync(Microsoft.Web.WebView2.Core.CoreWebView2CapturePreviewImageFormat.Png, randomAccessStream);

            await randomAccessStream.AsStream().CopyToAsync(memoryStream);
#elif MACCATALYST || MACOS
            using var image = await webView.TakeSnapshotAsync(new WebKit.WKSnapshotConfiguration());
            using var png = image.AsPNG();

            var data = new byte[png.Length];

            System.Runtime.InteropServices.Marshal.Copy(png.Bytes, data, 0, Convert.ToInt32(data.Length));

            memoryStream.Write(data, 0, data.Length);
#endif
            return memoryStream.ToArray();
        }

#if WINDOWS
        public static async Task<byte[]> GetCaptureYoutubePlayerData (Microsoft.UI.Xaml.Controls.WebView2 webView)
#elif MACCATALYST || MACOS
        public static async Task<byte[]> GetCaptureYoutubePlayerData (WebKit.WKWebView webView)
#else
        public static byte[] GetCaptureYoutubePlayerData (object webView)
#endif
        {
#if WINDOWS || MACCATALYST || MACOS
            var pngDataUri = await WebViewUtility.ExecuteJavaScriptAsync(webView, CaptureYoutubeVideoPlayerScriptPath);

            return Convert.FromBase64String(CustomRegex.MatchPngUri(pngDataUri));
#else
            return null;
#endif
        }

#if WINDOWS
        public static async Task<byte[]> GetCaptureYoutubeBackgroundSlateImageData (Microsoft.UI.Xaml.Controls.WebView2 webView)
#elif MACCATALYST || MACOS
        public static async Task<byte[]> GetCaptureYoutubeBackgroundSlateImageData (WebKit.WKWebView webView)
#else
        public static byte[] GetCaptureYoutubeBackgroundSlateImageData (object webView)
#endif
        {
#if WINDOWS || MACCATALYST || MACOS
            await WebViewUtility.ExecuteJavaScriptAsync(webView, LoadYoutubeBackgroundSlateImageScriptPath);

            while (!bool.Parse(await WebViewUtility.ExecuteJavaScriptAsync(webView, IsLoadCompleteImage))) { }

            var pngDataUri = await WebViewUtility.ExecuteJavaScriptAsync(webView, CaptureYoutubeBackgroundSlateImageScriptPath);

            return Convert.FromBase64String(CustomRegex.MatchPngUri(pngDataUri));
#else
            return null;
#endif
        }
    }
}
