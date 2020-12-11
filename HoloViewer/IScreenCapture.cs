using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.MobileBlazorBindings.Elements;
using Xamarin.Forms;

namespace HoloViewer
{
    public interface IScreenCapture
    {
        protected const string CaptureYoutubePlayerScriptPath = ScriptPath + "CaptureYoutubePlayer.js";

        protected const string GetWidthYoutubePlayerScriptPath = ScriptPath + "GetWidthYoutubePlayer.js";

        protected const string GetHeightYoutubePlayerScriptPath = ScriptPath + "GetHeightYoutubePlayer.js";

        private const string ScriptPath = "./wwwroot/_content/HoloViewer/scripts/";

        private const string CaptureDirectoryName = "Capture";

        private const string CaptureFileNameFormat = "yyyyMMdd_HHmmss";

        private const string CaptureFileExtension = ".png";

        protected delegate Task<byte[]> CaptureMethod (BlazorWebView blazorWebView);

        private static string GetCaptureFileName (DateTime dateTime)
        {
            return (dateTime.ToString(CaptureFileNameFormat) + CaptureFileExtension);
        }

        private static string GetCaptureFileName (DateTime dateTime, int index)
        {
            return (dateTime.ToString(CaptureFileNameFormat) + "_" + index.ToString() + CaptureFileExtension);
        }

        public static string GetCaptureDirectoryFullPath ()
        {
            return Path.GetFullPath(CaptureDirectoryName);
        }

        public static string GetCaptureFullPath (DateTime dateTime)
        {
            return Path.Combine(GetCaptureDirectoryFullPath(), GetCaptureFileName(dateTime));
        }

        public static string GetCaptureFullPath (DateTime dateTime, int index)
        {
            return Path.Combine(GetCaptureDirectoryFullPath(), GetCaptureFileName(dateTime, index));
        }

        public static bool IsCaptureableYoutubePlayer (string url)
        {
            return Regex.IsMatch(url, @"https://www.youtube.com/watch\?.+");
        }

        protected static string ConvertUriToBase64 (string uri)
        {
            var match = Regex.Match(uri, @"""data:image/png;base64,(?<data>.+)""");

            return match.Groups["data"].Value;
        }

        public const int CaptureModeNone = 0;

        public enum CaptureScreenConvertType
        {
            Combine,
            Separate,
        }

        public enum ScreenPosition_Single
        {
            Window,
        }

        public enum ScreenPosition_Horizontal2
        {
            Top,
            Bottom,
        }

        public enum ScreenPosition_Vertical2
        {
            Left,
            Right,
        }

        public enum ScreenPosition_Horizontal3
        {
            Top,
            Center,
            Bottom,
        }

        public enum ScreenPosition_Vertical3
        {
            Left,
            Center,
            Right,
        }

        public enum ScreenPosition_Custom3_1
        {
            Top,
            BottomLeft,
            BottomRight,
        }

        public enum ScreenPosition_Custom3_2
        {
            TopLeft,
            TopRight,
            Bottom,
        }

        public enum ScreenPosition_Horizontal4
        {
            Top,
            CenterTop,
            CenterBottom,
            Bottom,
        }

        public enum ScreenPosition_Vertical4
        {
            Left,
            CenterLeft,
            CenterRight,
            Right,
        }

        public enum ScreenPosition_Custom4
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
        }

        [Flags]
        public enum CaptureMode_Single
        {
            WebView = 0x01,
        }

        [Flags]
        public enum CaptureMode_Horizontal2
        {
            Top = 0x01,
            Bottom = 0x02,
        }

        [Flags]
        public enum CaptureMode_Vertical2
        {
            Left = 0x01,
            Right = 0x02,
        }

        [Flags]
        public enum CaptureMode_Horizontal3
        {
            Top = 0x01,
            Center = 0x02,
            Bottom = 0x04,
        }

        [Flags]
        public enum CaptureMode_Vertical3
        {
            Left = 0x01,
            Center = 0x02,
            Right = 0x04,
        }

        [Flags]
        public enum CaptureMode_Custom3_1
        {
            Top = 0x01,
            BottomLeft = 0x02,
            BottomRight = 0x04,
        }

        [Flags]
        public enum CaptureMode_Custom3_2
        {
            TopLeft = 0x01,
            TopRight = 0x02,
            Bottom = 0x04,
        }

        [Flags]
        public enum CaptureMode_Horizontal4
        {
            Top = 0x01,
            CenterTop = 0x02,
            CenterBottom = 0x04,
            Bottom = 0x08,
        }

        [Flags]
        public enum CaptureMode_Vertical4
        {
            Left = 0x01,
            CenterLeft = 0x02,
            CenterRight = 0x04,
            Right = 0x08,
        }

        [Flags]
        public enum CaptureMode_Custom4
        {
            TopLeft = 0x01,
            TopRight = 0x02,
            BottomLeft = 0x04,
            BottomRight = 0x08,
        }

        public abstract class CaptureTargetBase
        {
            public BlazorWebView CaptureBlazorWebView { get; }

            public abstract Task<int> GetWidth ();

            public abstract Task<int> GetHeight ();

            public abstract Task<byte[]> GetCaptureData ();

            public CaptureTargetBase (BlazorWebView blazorWebView)
            {
                CaptureBlazorWebView = blazorWebView;
            }
        }

        public class CaptureWebView : CaptureTargetBase
        {
            public override Task<int> GetWidth () { return Task.Run(() => { return (int)Math.Round(CaptureBlazorWebView.NativeControl.Width); }); }

            public override Task<int> GetHeight () { return Task.Run(() => { return (int)Math.Round(CaptureBlazorWebView.NativeControl.Height); }); }

            public override async Task<byte[]> GetCaptureData () { return await DependencyService.Get<IScreenCapture>().GetCaptureWebViewData(CaptureBlazorWebView); }

            public CaptureWebView (BlazorWebView blazorWebView) : base(blazorWebView)
            {
            }
        }

        public class CaptureYoutubePlayer : CaptureTargetBase
        {
            public override async Task<int> GetWidth () { return await DependencyService.Get<IScreenCapture>().GetYoutubePlayerWidth(CaptureBlazorWebView); }

            public override async Task<int> GetHeight () { return await DependencyService.Get<IScreenCapture>().GetYoutubePlayerHeight(CaptureBlazorWebView); }

            public override async Task<byte[]> GetCaptureData () { return await DependencyService.Get<IScreenCapture>().GetCaptureYoutubePlayerData(CaptureBlazorWebView); }

            public CaptureYoutubePlayer (BlazorWebView blazorWebView) : base(blazorWebView)
            {
            }
        }

        public abstract class CaptureScreenBase
        {
            public BlazorWebView CaptureTargetBlazorWebView { get { return CaptureTarget.CaptureBlazorWebView; } }
            
            public async Task<int> GetWidthPixel () { return await CaptureTarget.GetWidth(); }

            public async Task<int> GetHeightPixel () { return await CaptureTarget.GetHeight(); }

            public async Task<byte[]> GetCaptureData () { return await CaptureTarget.GetCaptureData(); }

            public abstract CaptureTargetBase CaptureTarget { get; }

            public abstract Task<int> GetOffsetPixelX ();

            public abstract Task<int> GetOffsetPixelY ();
        }

        public abstract class CaptureScreenBase<ScreenPositionEnumType, CaptureMode> : CaptureScreenBase where ScreenPositionEnumType : Enum where CaptureMode : Enum
        {
            protected Dictionary<ScreenPositionEnumType, CaptureTargetBase> BlazorWebViews { get; set; }

            protected CaptureMode CurrentCaptureMode { get; set; }
        }

        public class CaptureScreenSingle : CaptureScreenBase<ScreenPosition_Single, CaptureMode_Single>
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Single.Window];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenSingle (Dictionary<ScreenPosition_Single, CaptureTargetBase> captureWebViews, CaptureMode_Single captureMode)
            {
                BlazorWebViews = captureWebViews;
                CurrentCaptureMode = captureMode;
            }

            public static CaptureScreenBase[] CreateCaptureScreens (CaptureSetting_Single captureSetting)
            {
                return new CaptureScreenBase[] { new CaptureScreenSingle(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode) };
            }
        }

        public abstract class CaptureScreenHorizontal2 : CaptureScreenBase<ScreenPosition_Horizontal2, CaptureMode_Horizontal2>
        {
            public CaptureScreenHorizontal2 (Dictionary<ScreenPosition_Horizontal2, CaptureTargetBase> captureWebViews, CaptureMode_Horizontal2 captureMode)
            {
                BlazorWebViews = captureWebViews;
                CurrentCaptureMode = captureMode;
            }

            public static CaptureScreenBase[] CreateCaptureScreens (CaptureSetting_Horizontal2Mode captureSetting)
            {
                var captureScreens = new List<CaptureScreenBase>();

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Horizontal2.Top))
                {
                    captureScreens.Add(new CaptureScreenHorizontal2Top(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Horizontal2.Bottom))
                {
                    captureScreens.Add(new CaptureScreenHorizontal2Bottom(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                return captureScreens.ToArray();
            }
        }

        public class CaptureScreenHorizontal2Top : CaptureScreenHorizontal2
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Horizontal2.Top];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenHorizontal2Top (Dictionary<ScreenPosition_Horizontal2, CaptureTargetBase> captureWebViews, CaptureMode_Horizontal2 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenHorizontal2Bottom : CaptureScreenHorizontal2
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Horizontal2.Bottom];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return (CurrentCaptureMode.HasFlag(CaptureMode_Horizontal2.Top) ? await BlazorWebViews[ScreenPosition_Horizontal2.Top].GetHeight() : 0); }

            public CaptureScreenHorizontal2Bottom (Dictionary<ScreenPosition_Horizontal2, CaptureTargetBase> captureWebViews, CaptureMode_Horizontal2 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public abstract class CaptureScreenVertical2 : CaptureScreenBase<ScreenPosition_Vertical2, CaptureMode_Vertical2>
        {
            public CaptureScreenVertical2 (Dictionary<ScreenPosition_Vertical2, CaptureTargetBase> captureWebViews, CaptureMode_Vertical2 captureMode)
            {
                BlazorWebViews = captureWebViews;
                CurrentCaptureMode = captureMode;
            }

            public static CaptureScreenBase[] CreateCaptureScreens (CaptureSetting_Vertical2Mode captureSetting)
            {
                var captureScreens = new List<CaptureScreenBase>();

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Vertical2.Left))
                {
                    captureScreens.Add(new CaptureScreenVertical2Left(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Vertical2.Right))
                {
                    captureScreens.Add(new CaptureScreenVertical2Right(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                return captureScreens.ToArray();
            }
        }

        public class CaptureScreenVertical2Left : CaptureScreenVertical2
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Vertical2.Left];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenVertical2Left (Dictionary<ScreenPosition_Vertical2, CaptureTargetBase> captureWebViews, CaptureMode_Vertical2 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenVertical2Right : CaptureScreenVertical2
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Vertical2.Right];

            public override async Task<int> GetOffsetPixelX () { return (CurrentCaptureMode.HasFlag(CaptureMode_Vertical2.Left) ? await BlazorWebViews[ScreenPosition_Vertical2.Left].GetWidth() : 0); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenVertical2Right (Dictionary<ScreenPosition_Vertical2, CaptureTargetBase> captureWebViews, CaptureMode_Vertical2 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public abstract class CaptureScreenHorizontal3 : CaptureScreenBase<ScreenPosition_Horizontal3, CaptureMode_Horizontal3>
        {
            public CaptureScreenHorizontal3 (Dictionary<ScreenPosition_Horizontal3, CaptureTargetBase> captureWebViews, CaptureMode_Horizontal3 captureMode)
            {
                BlazorWebViews = captureWebViews;
                CurrentCaptureMode = captureMode;
            }

            public static CaptureScreenBase[] CreateCaptureScreens (CaptureSetting_Horizontal3Mode captureSetting)
            {
                var captureScreens = new List<CaptureScreenBase>();

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Horizontal3.Top))
                {
                    captureScreens.Add(new CaptureScreenHorizontal3Top(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Horizontal3.Center))
                {
                    captureScreens.Add(new CaptureScreenHorizontal3Center(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Horizontal3.Bottom))
                {
                    captureScreens.Add(new CaptureScreenHorizontal3Bottom(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                return captureScreens.ToArray();
            }
        }

        public class CaptureScreenHorizontal3Top : CaptureScreenHorizontal3
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Horizontal3.Top];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenHorizontal3Top (Dictionary<ScreenPosition_Horizontal3, CaptureTargetBase> captureWebViews, CaptureMode_Horizontal3 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenHorizontal3Center : CaptureScreenHorizontal3
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Horizontal3.Center];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return (CurrentCaptureMode.HasFlag(CaptureMode_Horizontal3.Top) ? await BlazorWebViews[ScreenPosition_Horizontal3.Top].GetHeight() : 0); }

            public CaptureScreenHorizontal3Center (Dictionary<ScreenPosition_Horizontal3, CaptureTargetBase> captureWebViews, CaptureMode_Horizontal3 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenHorizontal3Bottom : CaptureScreenHorizontal3
        {
            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return ((CurrentCaptureMode.HasFlag(CaptureMode_Horizontal3.Top) ? await BlazorWebViews[ScreenPosition_Horizontal3.Top].GetHeight() : 0) + ((CurrentCaptureMode.HasFlag(CaptureMode_Horizontal3.Top) | CurrentCaptureMode.HasFlag(CaptureMode_Horizontal3.Center)) ? await BlazorWebViews[ScreenPosition_Horizontal3.Center].GetHeight() : 0)); }

            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Horizontal3.Bottom];

            public CaptureScreenHorizontal3Bottom (Dictionary<ScreenPosition_Horizontal3, CaptureTargetBase> captureWebViews, CaptureMode_Horizontal3 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public abstract class CaptureScreenVertical3 : CaptureScreenBase<ScreenPosition_Vertical3, CaptureMode_Vertical3>
        {
            public CaptureScreenVertical3 (Dictionary<ScreenPosition_Vertical3, CaptureTargetBase> captureWebViews, CaptureMode_Vertical3 captureMode)
            {
                BlazorWebViews = captureWebViews;
                CurrentCaptureMode = captureMode;
            }

            public static CaptureScreenBase[] CreateCaptureScreens (CaptureSetting_Vertical3Mode captureSetting)
            {
                var captureScreens = new List<CaptureScreenBase>();

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Vertical3.Left))
                {
                    captureScreens.Add(new CaptureScreenVertical3Left(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Vertical3.Center))
                {
                    captureScreens.Add(new CaptureScreenVertical3Center(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Vertical3.Right))
                {
                    captureScreens.Add(new CaptureScreenVertical3Right(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                return captureScreens.ToArray();
            }
        }

        public class CaptureScreenVertical3Left : CaptureScreenVertical3
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Vertical3.Left];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenVertical3Left (Dictionary<ScreenPosition_Vertical3, CaptureTargetBase> captureWebViews, CaptureMode_Vertical3 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenVertical3Center : CaptureScreenVertical3
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Vertical3.Center];

            public override async Task<int> GetOffsetPixelX () { return (CurrentCaptureMode.HasFlag(CaptureMode_Vertical3.Left) ? await BlazorWebViews[ScreenPosition_Vertical3.Left].GetWidth() : 0); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenVertical3Center (Dictionary<ScreenPosition_Vertical3, CaptureTargetBase> captureWebViews, CaptureMode_Vertical3 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenVertical3Right : CaptureScreenVertical3
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Vertical3.Right];

            public override async Task<int> GetOffsetPixelX () { return ((CurrentCaptureMode.HasFlag(CaptureMode_Vertical3.Left) ? await BlazorWebViews[ScreenPosition_Vertical3.Left].GetWidth() : 0) + ((CurrentCaptureMode.HasFlag(CaptureMode_Vertical3.Left) | CurrentCaptureMode.HasFlag(CaptureMode_Vertical3.Center)) ? await BlazorWebViews[ScreenPosition_Vertical3.Center].GetWidth() : 0)); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenVertical3Right (Dictionary<ScreenPosition_Vertical3, CaptureTargetBase> captureWebViews, CaptureMode_Vertical3 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public abstract class CaptureScreenCustom3_1 : CaptureScreenBase<ScreenPosition_Custom3_1, CaptureMode_Custom3_1>
        {
            public CaptureScreenCustom3_1 (Dictionary<ScreenPosition_Custom3_1, CaptureTargetBase> captureWebViews, CaptureMode_Custom3_1 captureMode)
            {
                BlazorWebViews = captureWebViews;
                CurrentCaptureMode = captureMode;
            }

            public static CaptureScreenBase[] CreateCaptureScreens (CaptureSetting_Custom3_1Mode captureSetting)
            {
                var captureScreens = new List<CaptureScreenBase>();

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Custom3_1.Top))
                {
                    captureScreens.Add(new CaptureScreenCustom3_1Top(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Custom3_1.BottomLeft))
                {
                    captureScreens.Add(new CaptureScreenCustom3_1BottomLeft(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Custom3_1.BottomRight))
                {
                    captureScreens.Add(new CaptureScreenCustom3_1BottomRight(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                return captureScreens.ToArray();
            }
        }

        public class CaptureScreenCustom3_1Top : CaptureScreenCustom3_1
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Custom3_1.Top];

            public override async Task<int> GetOffsetPixelX () { return ((CurrentCaptureMode.HasFlag(CaptureMode_Custom3_1.BottomLeft) && CurrentCaptureMode.HasFlag(CaptureMode_Custom3_1.BottomRight)) ? ((await BlazorWebViews[ScreenPosition_Custom3_1.BottomLeft].GetWidth() + await BlazorWebViews[ScreenPosition_Custom3_1.BottomRight].GetWidth() - await BlazorWebViews[ScreenPosition_Custom3_1.Top].GetWidth()) / 2 ) : 0); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenCustom3_1Top (Dictionary<ScreenPosition_Custom3_1, CaptureTargetBase> captureWebViews, CaptureMode_Custom3_1 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenCustom3_1BottomLeft : CaptureScreenCustom3_1
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Custom3_1.BottomLeft];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return (CurrentCaptureMode.HasFlag(CaptureMode_Custom3_1.Top) ? await BlazorWebViews[ScreenPosition_Custom3_1.Top].GetHeight() : 0); }

            public CaptureScreenCustom3_1BottomLeft (Dictionary<ScreenPosition_Custom3_1, CaptureTargetBase> captureWebViews, CaptureMode_Custom3_1 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenCustom3_1BottomRight : CaptureScreenCustom3_1
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Custom3_1.BottomRight];

            public override async Task<int> GetOffsetPixelX () { return (CurrentCaptureMode.HasFlag(CaptureMode_Custom3_1.BottomLeft) ? await BlazorWebViews[ScreenPosition_Custom3_1.BottomLeft].GetWidth() : 0); }

            public override async Task<int> GetOffsetPixelY () { return (CurrentCaptureMode.HasFlag(CaptureMode_Custom3_1.Top) ? await BlazorWebViews[ScreenPosition_Custom3_1.Top].GetHeight() : 0); }

            public CaptureScreenCustom3_1BottomRight (Dictionary<ScreenPosition_Custom3_1, CaptureTargetBase> captureWebViews, CaptureMode_Custom3_1 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public abstract class CaptureScreenCustom3_2 : CaptureScreenBase<ScreenPosition_Custom3_2, CaptureMode_Custom3_2>
        {
            public CaptureScreenCustom3_2 (Dictionary<ScreenPosition_Custom3_2, CaptureTargetBase> captureWebViews, CaptureMode_Custom3_2 captureMode)
            {
                BlazorWebViews = captureWebViews;
                CurrentCaptureMode = captureMode;
            }

            public static CaptureScreenBase[] CreateCaptureScreens (CaptureSetting_Custom3_2Mode captureSetting)
            {
                var captureScreens = new List<CaptureScreenBase>();

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Custom3_2.TopLeft))
                {
                    captureScreens.Add(new CaptureScreenCustom3_2TopLeft(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Custom3_2.TopRight))
                {
                    captureScreens.Add(new CaptureScreenCustom3_2TopRight(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Custom3_2.Bottom))
                {
                    captureScreens.Add(new CaptureScreenCustom3_2Bottom(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                return captureScreens.ToArray();
            }
        }

        public class CaptureScreenCustom3_2TopLeft : CaptureScreenCustom3_2
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Custom3_2.TopLeft];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenCustom3_2TopLeft (Dictionary<ScreenPosition_Custom3_2, CaptureTargetBase> captureWebViews, CaptureMode_Custom3_2 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenCustom3_2TopRight : CaptureScreenCustom3_2
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Custom3_2.TopRight];

            public override async Task<int> GetOffsetPixelX () { return (CurrentCaptureMode.HasFlag(CaptureMode_Custom3_2.TopLeft) ? await BlazorWebViews[ScreenPosition_Custom3_2.TopLeft].GetWidth() : 0); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenCustom3_2TopRight (Dictionary<ScreenPosition_Custom3_2, CaptureTargetBase> captureWebViews, CaptureMode_Custom3_2 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenCustom3_2Bottom : CaptureScreenCustom3_2
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Custom3_2.Bottom];

            public override async Task<int> GetOffsetPixelX () { return ((CurrentCaptureMode.HasFlag(CaptureMode_Custom3_2.TopLeft) && CurrentCaptureMode.HasFlag(CaptureMode_Custom3_2.TopRight)) ? ((await BlazorWebViews[ScreenPosition_Custom3_2.TopLeft].GetWidth() + await BlazorWebViews[ScreenPosition_Custom3_2.TopRight].GetWidth() - await BlazorWebViews[ScreenPosition_Custom3_2.Bottom].GetWidth()) / 2) : 0); }

            public override async Task<int> GetOffsetPixelY () { return Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Custom3_2.TopLeft) ? await BlazorWebViews[ScreenPosition_Custom3_2.TopLeft].GetHeight() : 0, (CurrentCaptureMode.HasFlag(CaptureMode_Custom3_2.TopRight)) ? await BlazorWebViews[ScreenPosition_Custom3_2.TopRight].GetHeight() : 0); }

            public CaptureScreenCustom3_2Bottom (Dictionary<ScreenPosition_Custom3_2, CaptureTargetBase> captureWebViews, CaptureMode_Custom3_2 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public abstract class CaptureScreenHorizontal4 : CaptureScreenBase<ScreenPosition_Horizontal4, CaptureMode_Horizontal4>
        {
            public CaptureScreenHorizontal4 (Dictionary<ScreenPosition_Horizontal4, CaptureTargetBase> captureWebViews, CaptureMode_Horizontal4 captureMode)
            {
                BlazorWebViews = captureWebViews;
                CurrentCaptureMode = captureMode;
            }

            public static CaptureScreenBase[] CreateCaptureScreens (CaptureSetting_Horizontal4Mode captureSetting)
            {
                var captureScreens = new List<CaptureScreenBase>();

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.Top))
                {
                    captureScreens.Add(new CaptureScreenHorizontal4Top(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.CenterTop))
                {
                    captureScreens.Add(new CaptureScreenHorizontal4CenterTop(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.CenterBottom))
                {
                    captureScreens.Add(new CaptureScreenHorizontal4CenterBottom(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.Bottom))
                {
                    captureScreens.Add(new CaptureScreenHorizontal4Bottom(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                return captureScreens.ToArray();
            }
        }

        public class CaptureScreenHorizontal4Top : CaptureScreenHorizontal4
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Horizontal4.Top];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenHorizontal4Top (Dictionary<ScreenPosition_Horizontal4, CaptureTargetBase> captureWebViews, CaptureMode_Horizontal4 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenHorizontal4CenterTop : CaptureScreenHorizontal4
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Horizontal4.CenterTop];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return (CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.Top) ? await BlazorWebViews[ScreenPosition_Horizontal4.Top].GetHeight() : 0); }

            public CaptureScreenHorizontal4CenterTop (Dictionary<ScreenPosition_Horizontal4, CaptureTargetBase> captureWebViews, CaptureMode_Horizontal4 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenHorizontal4CenterBottom : CaptureScreenHorizontal4
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Horizontal4.CenterBottom];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return (CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.Top) ? (await BlazorWebViews[ScreenPosition_Horizontal4.Top].GetHeight() + await BlazorWebViews[ScreenPosition_Horizontal4.CenterTop].GetHeight()) : CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.CenterTop) ? await BlazorWebViews[ScreenPosition_Horizontal4.CenterTop].GetHeight() : 0); }

            public CaptureScreenHorizontal4CenterBottom (Dictionary<ScreenPosition_Horizontal4, CaptureTargetBase> captureWebViews, CaptureMode_Horizontal4 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenHorizontal4Bottom : CaptureScreenHorizontal4
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Horizontal4.Bottom];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return (CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.Top) ? (await BlazorWebViews[ScreenPosition_Horizontal4.Top].GetHeight() + await BlazorWebViews[ScreenPosition_Horizontal4.CenterTop].GetHeight() + await BlazorWebViews[ScreenPosition_Horizontal4.CenterBottom].GetHeight()) : CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.CenterTop) ? (await BlazorWebViews[ScreenPosition_Horizontal4.CenterTop].GetHeight() + await BlazorWebViews[ScreenPosition_Horizontal4.CenterBottom].GetHeight()) : CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.CenterBottom) ? await BlazorWebViews[ScreenPosition_Horizontal4.CenterBottom].GetHeight() : 0); }

            public CaptureScreenHorizontal4Bottom (Dictionary<ScreenPosition_Horizontal4, CaptureTargetBase> captureWebViews, CaptureMode_Horizontal4 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public abstract class CaptureScreenVertical4 : CaptureScreenBase<ScreenPosition_Vertical4, CaptureMode_Vertical4>
        {
            public CaptureScreenVertical4 (Dictionary<ScreenPosition_Vertical4, CaptureTargetBase> captureWebViews, CaptureMode_Vertical4 captureMode)
            {
                BlazorWebViews = captureWebViews;
                CurrentCaptureMode = captureMode;
            }

            public static CaptureScreenBase[] CreateCaptureScreens (CaptureSetting_Vertical4Mode captureSetting)
            {
                var captureScreens = new List<CaptureScreenBase>();

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.Left))
                {
                    captureScreens.Add(new CaptureScreenVertical4Left(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.CenterLeft))
                {
                    captureScreens.Add(new CaptureScreenVertical4CenterLeft(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.CenterRight))
                {
                    captureScreens.Add(new CaptureScreenVertical4CenterRight(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.Right))
                {
                    captureScreens.Add(new CaptureScreenVertical4Right(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                return captureScreens.ToArray();
            }
        }

        public class CaptureScreenVertical4Left : CaptureScreenVertical4
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Vertical4.Left];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenVertical4Left (Dictionary<ScreenPosition_Vertical4, CaptureTargetBase> captureWebViews, CaptureMode_Vertical4 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenVertical4CenterLeft : CaptureScreenVertical4
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Vertical4.CenterLeft];

            public override async Task<int> GetOffsetPixelX () { return (CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.Left) ? await BlazorWebViews[ScreenPosition_Vertical4.Left].GetWidth() : 0); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenVertical4CenterLeft (Dictionary<ScreenPosition_Vertical4, CaptureTargetBase> captureWebViews, CaptureMode_Vertical4 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenVertical4CenterRight : CaptureScreenVertical4
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Vertical4.CenterRight];

            public override async Task<int> GetOffsetPixelX () { return (CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.Left) ? (await BlazorWebViews[ScreenPosition_Vertical4.Left].GetWidth() + await BlazorWebViews[ScreenPosition_Vertical4.CenterLeft].GetWidth()) : CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.CenterLeft) ? await BlazorWebViews[ScreenPosition_Vertical4.CenterLeft].GetWidth() : 0); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenVertical4CenterRight (Dictionary<ScreenPosition_Vertical4, CaptureTargetBase> captureWebViews, CaptureMode_Vertical4 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenVertical4Right : CaptureScreenVertical4
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Vertical4.Right];

            public override async Task<int> GetOffsetPixelX () { return (CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.Left) ? (await BlazorWebViews[ScreenPosition_Vertical4.Left].GetWidth() + await BlazorWebViews[ScreenPosition_Vertical4.CenterLeft].GetWidth() + await BlazorWebViews[ScreenPosition_Vertical4.CenterRight].GetWidth()) : CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.CenterLeft) ? (await BlazorWebViews[ScreenPosition_Vertical4.CenterLeft].GetWidth() + await BlazorWebViews[ScreenPosition_Vertical4.CenterRight].GetWidth()) : CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.CenterRight) ? await BlazorWebViews[ScreenPosition_Vertical4.CenterRight].GetWidth() : 0); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenVertical4Right (Dictionary<ScreenPosition_Vertical4, CaptureTargetBase> captureWebViews, CaptureMode_Vertical4 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public abstract class CaptureScreenCustom4 : CaptureScreenBase<ScreenPosition_Custom4, CaptureMode_Custom4>
        {
            public CaptureScreenCustom4 (Dictionary<ScreenPosition_Custom4, CaptureTargetBase> captureWebViews, CaptureMode_Custom4 captureMode)
            {
                BlazorWebViews = captureWebViews;
                CurrentCaptureMode = captureMode;
            }

            public static CaptureScreenBase[] CreateCaptureScreens (CaptureSetting_Custom4Mode captureSetting)
            {
                var captureScreens = new List<CaptureScreenBase>();

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Custom4.TopLeft))
                {
                    captureScreens.Add(new CaptureScreenCustom4TopLeft(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Custom4.TopRight))
                {
                    captureScreens.Add(new CaptureScreenCustom4TopRight(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Custom4.BottomLeft))
                {
                    captureScreens.Add(new CaptureScreenCustom4BottomLeft(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                if (captureSetting.CurrentCaptureMode.HasFlag(CaptureMode_Custom4.BottomRight))
                {
                    captureScreens.Add(new CaptureScreenCustom4BottomRight(captureSetting.BlazorWebViews, captureSetting.CurrentCaptureMode));
                }

                return captureScreens.ToArray();
            }
        }

        public class CaptureScreenCustom4TopLeft : CaptureScreenCustom4
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Custom4.TopLeft];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenCustom4TopLeft (Dictionary<ScreenPosition_Custom4, CaptureTargetBase> captureWebViews, CaptureMode_Custom4 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenCustom4TopRight : CaptureScreenCustom4
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Custom4.TopRight];

            public override async Task<int> GetOffsetPixelX () { return Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Custom4.TopLeft) ? await BlazorWebViews[ScreenPosition_Custom4.TopLeft].GetWidth() : 0, (CurrentCaptureMode.HasFlag(CaptureMode_Custom4.BottomLeft) ? await BlazorWebViews[ScreenPosition_Custom4.BottomLeft].GetWidth() : 0)); }

            public override async Task<int> GetOffsetPixelY () { return await Task.Run(() => { return 0; }); }

            public CaptureScreenCustom4TopRight (Dictionary<ScreenPosition_Custom4, CaptureTargetBase> captureWebViews, CaptureMode_Custom4 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenCustom4BottomLeft : CaptureScreenCustom4
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Custom4.BottomLeft];

            public override async Task<int> GetOffsetPixelX () { return await Task.Run(() => { return 0; }); }

            public override async Task<int> GetOffsetPixelY () { return Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Custom4.TopLeft) ? await BlazorWebViews[ScreenPosition_Custom4.TopLeft].GetHeight() : 0, (CurrentCaptureMode.HasFlag(CaptureMode_Custom4.TopRight) ? await BlazorWebViews[ScreenPosition_Custom4.TopRight].GetHeight() : 0)); }

            public CaptureScreenCustom4BottomLeft (Dictionary<ScreenPosition_Custom4, CaptureTargetBase> captureWebViews, CaptureMode_Custom4 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public class CaptureScreenCustom4BottomRight : CaptureScreenCustom4
        {
            public override CaptureTargetBase CaptureTarget => BlazorWebViews[ScreenPosition_Custom4.BottomRight];

            public override async Task<int> GetOffsetPixelX () { return Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Custom4.TopLeft) ? await BlazorWebViews[ScreenPosition_Custom4.TopLeft].GetWidth() : 0, (CurrentCaptureMode.HasFlag(CaptureMode_Custom4.BottomLeft) ? await BlazorWebViews[ScreenPosition_Custom4.BottomLeft].GetWidth() : 0)); }

            public override async Task<int> GetOffsetPixelY () { return Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Custom4.TopLeft) ? await BlazorWebViews[ScreenPosition_Custom4.TopLeft].GetHeight() : 0, (CurrentCaptureMode.HasFlag(CaptureMode_Custom4.TopRight) ? await BlazorWebViews[ScreenPosition_Custom4.TopRight].GetHeight() : 0)); }

            public CaptureScreenCustom4BottomRight (Dictionary<ScreenPosition_Custom4, CaptureTargetBase> captureWebViews, CaptureMode_Custom4 captureMode) : base(captureWebViews, captureMode)
            {
            }
        }

        public abstract class CaptureSettingBase
        {
            public CaptureScreenConvertType CurrentCaptureScreenConvertType { get; set; }

            public abstract Task<int> GetImageWidth ();

            public abstract Task<int> GetImageHeight ();
        }

        public abstract class CaptureSettingBase<ScreenPosition, CaptureMode> : CaptureSettingBase
        {
            public Dictionary<ScreenPosition, CaptureTargetBase> BlazorWebViews { get; set; }

            public CaptureMode CurrentCaptureMode { get; set; }
        }

        public class CaptureSetting_Single : CaptureSettingBase<ScreenPosition_Single, CaptureMode_Single>
        {
            public override async Task<int> GetImageWidth () { return await BlazorWebViews[ScreenPosition_Single.Window].GetWidth(); }

            public override async Task<int> GetImageHeight () { return await BlazorWebViews[ScreenPosition_Single.Window].GetHeight(); }
        }

        public class CaptureSetting_Horizontal2Mode : CaptureSettingBase<ScreenPosition_Horizontal2, CaptureMode_Horizontal2>
        {
            public override async Task<int> GetImageWidth () { return Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Horizontal2.Top) ? await BlazorWebViews[ScreenPosition_Horizontal2.Top].GetWidth() : 0, CurrentCaptureMode.HasFlag(CaptureMode_Horizontal2.Bottom) ? await BlazorWebViews[ScreenPosition_Horizontal2.Bottom].GetWidth() : 0); }

            public override async Task<int> GetImageHeight () { return ((CurrentCaptureMode.HasFlag(CaptureMode_Horizontal2.Top) ? await BlazorWebViews[ScreenPosition_Horizontal2.Top].GetHeight() : 0) + (CurrentCaptureMode.HasFlag(CaptureMode_Horizontal2.Bottom) ? await BlazorWebViews[ScreenPosition_Horizontal2.Bottom].GetHeight() : 0)); }
        }

        public class CaptureSetting_Vertical2Mode : CaptureSettingBase<ScreenPosition_Vertical2, CaptureMode_Vertical2>
        {
            public override async Task<int> GetImageWidth () { return ((CurrentCaptureMode.HasFlag(CaptureMode_Vertical2.Left) ? await BlazorWebViews[ScreenPosition_Vertical2.Left].GetWidth() : 0) + (CurrentCaptureMode.HasFlag(CaptureMode_Vertical2.Right) ? await BlazorWebViews[ScreenPosition_Vertical2.Right].GetWidth() : 0)); }

            public override async Task<int> GetImageHeight () { return Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Vertical2.Left) ? await BlazorWebViews[ScreenPosition_Vertical2.Left].GetHeight() : 0, CurrentCaptureMode.HasFlag(CaptureMode_Vertical2.Right) ? await BlazorWebViews[ScreenPosition_Vertical2.Right].GetHeight() : 0); }
        }

        public class CaptureSetting_Horizontal3Mode : CaptureSettingBase<ScreenPosition_Horizontal3, CaptureMode_Horizontal3>
        {
            public override async Task<int> GetImageWidth () { return Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Horizontal3.Top) ? await BlazorWebViews[ScreenPosition_Horizontal3.Top].GetWidth() : 0, Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Horizontal3.Center) ? await BlazorWebViews[ScreenPosition_Horizontal3.Center].GetWidth() : 0, (CurrentCaptureMode.HasFlag(CaptureMode_Horizontal3.Bottom) ? await BlazorWebViews[ScreenPosition_Horizontal3.Bottom].GetWidth() : 0))); }

            public override async Task<int> GetImageHeight () { return ((CurrentCaptureMode.HasFlag(CaptureMode_Horizontal3.Top) ? await BlazorWebViews[ScreenPosition_Horizontal3.Top].GetHeight() : 0) + ((CurrentCaptureMode.HasFlag(CaptureMode_Horizontal3.Center) | (CurrentCaptureMode.HasFlag(CaptureMode_Horizontal3.Top) && CurrentCaptureMode.HasFlag(CaptureMode_Horizontal3.Bottom))) ? await BlazorWebViews[ScreenPosition_Horizontal3.Center].GetHeight() : 0) + (CurrentCaptureMode.HasFlag(CaptureMode_Horizontal3.Bottom) ? await BlazorWebViews[ScreenPosition_Horizontal3.Bottom].GetHeight () : 0)); }
        }

        public class CaptureSetting_Vertical3Mode : CaptureSettingBase<ScreenPosition_Vertical3, CaptureMode_Vertical3>
        {
            public override async Task<int> GetImageWidth () { return ((CurrentCaptureMode.HasFlag(CaptureMode_Vertical3.Left) ? await BlazorWebViews[ScreenPosition_Vertical3.Left].GetWidth() : 0) + ((CurrentCaptureMode.HasFlag(CaptureMode_Vertical3.Center) | ((CurrentCaptureMode.HasFlag(CaptureMode_Vertical3.Left)) && (CurrentCaptureMode.HasFlag(CaptureMode_Vertical3.Right)))) ? await BlazorWebViews[ScreenPosition_Vertical3.Center].GetWidth() : 0) + (CurrentCaptureMode.HasFlag(CaptureMode_Vertical3.Right) ? await BlazorWebViews[ScreenPosition_Vertical3.Right].GetWidth() : 0)); }

            public override async Task<int> GetImageHeight () { return Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Vertical3.Left) ? await BlazorWebViews[ScreenPosition_Vertical3.Left].GetHeight() : 0, Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Vertical3.Center) ? await BlazorWebViews[ScreenPosition_Vertical3.Center].GetHeight() : 0, CurrentCaptureMode.HasFlag(CaptureMode_Vertical3.Right) ? await BlazorWebViews[ScreenPosition_Vertical3.Right].GetHeight() : 0)); }
        }

        public class CaptureSetting_Custom3_1Mode : CaptureSettingBase<ScreenPosition_Custom3_1, CaptureMode_Custom3_1>
        {
            public override async Task<int> GetImageWidth () { return Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Custom3_1.Top) ? await BlazorWebViews[ScreenPosition_Custom3_1.Top].GetWidth() : 0, ((CurrentCaptureMode.HasFlag(CaptureMode_Custom3_1.BottomLeft) ? await BlazorWebViews[ScreenPosition_Custom3_1.BottomLeft].GetWidth() : 0) + (CurrentCaptureMode.HasFlag(CaptureMode_Custom3_1.BottomRight) ? await BlazorWebViews[ScreenPosition_Custom3_1.BottomRight].GetWidth() : 0))); }

            public override async Task<int> GetImageHeight () { return ((CurrentCaptureMode.HasFlag(CaptureMode_Custom3_1.Top) ? await BlazorWebViews[ScreenPosition_Custom3_1.Top].GetHeight() : 0) + Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Custom3_1.BottomLeft) ? await BlazorWebViews[ScreenPosition_Custom3_1.BottomLeft].GetHeight() : 0, (CurrentCaptureMode.HasFlag(CaptureMode_Custom3_1.BottomRight) ? await BlazorWebViews[ScreenPosition_Custom3_1.BottomRight].GetHeight() : 0))); }
        }

        public class CaptureSetting_Custom3_2Mode : CaptureSettingBase<ScreenPosition_Custom3_2, CaptureMode_Custom3_2>
        {
            public override async Task<int> GetImageWidth () { return Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Custom3_2.Bottom) ? await BlazorWebViews[ScreenPosition_Custom3_2.Bottom].GetWidth() : 0, ((CurrentCaptureMode.HasFlag(CaptureMode_Custom3_2.TopLeft) ? await BlazorWebViews[ScreenPosition_Custom3_2.TopLeft].GetWidth() : 0) + (CurrentCaptureMode.HasFlag(CaptureMode_Custom3_2.TopRight) ? await BlazorWebViews[ScreenPosition_Custom3_2.TopRight].GetWidth() : 0))); }

            public override async Task<int> GetImageHeight () { return ((CurrentCaptureMode.HasFlag(CaptureMode_Custom3_2.Bottom) ? await BlazorWebViews[ScreenPosition_Custom3_2.Bottom].GetHeight() : 0) + Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Custom3_2.TopLeft) ? await BlazorWebViews[ScreenPosition_Custom3_2.TopLeft].GetHeight() : 0, CurrentCaptureMode.HasFlag(CaptureMode_Custom3_2.TopRight) ? await BlazorWebViews[ScreenPosition_Custom3_2.TopRight].GetHeight() : 0)); }
        }

        public class CaptureSetting_Horizontal4Mode : CaptureSettingBase<ScreenPosition_Horizontal4, CaptureMode_Horizontal4>
        {
            public override async Task<int> GetImageWidth () { return Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.Top) ? await BlazorWebViews[ScreenPosition_Horizontal4.Top].GetWidth() : 0, Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.CenterTop) ? await BlazorWebViews[ScreenPosition_Horizontal4.CenterTop].GetWidth() : 0, Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.CenterBottom) ? await BlazorWebViews[ScreenPosition_Horizontal4.CenterBottom].GetWidth() : 0, (CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.Bottom)) ? await BlazorWebViews[ScreenPosition_Horizontal4.Bottom].GetWidth() : 0))); }

            public override async Task<int> GetImageHeight () { return ((CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.Top) ? await BlazorWebViews[ScreenPosition_Horizontal4.Top].GetHeight() : 0) + ((CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.CenterTop) || (CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.Top) && (CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.CenterBottom) || CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.Bottom)))) ? await BlazorWebViews[ScreenPosition_Horizontal4.CenterTop].GetHeight() : 0) + ((CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.CenterBottom) || ((CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.Top) || CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.CenterTop)) && CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.Bottom))) ? await BlazorWebViews[ScreenPosition_Horizontal4.CenterBottom].GetHeight() : 0) + (CurrentCaptureMode.HasFlag(CaptureMode_Horizontal4.Bottom) ? await BlazorWebViews[ScreenPosition_Horizontal4.Bottom].GetHeight() : 0)); }
        }

        public class CaptureSetting_Vertical4Mode : CaptureSettingBase<ScreenPosition_Vertical4, CaptureMode_Vertical4>
        {
            public override async Task<int> GetImageWidth () { return ((CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.Left) ? await BlazorWebViews[ScreenPosition_Vertical4.Left].GetWidth() : 0) + ((CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.CenterLeft) || (CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.Left) && (CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.CenterRight) || CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.Right)))) ? await BlazorWebViews[ScreenPosition_Vertical4.CenterLeft].GetWidth() : 0) + ((CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.CenterRight) || (CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.Right) && (CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.Left) || CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.CenterLeft)))) ? await BlazorWebViews[ScreenPosition_Vertical4.CenterRight].GetWidth() : 0) + (CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.Right) ? await BlazorWebViews[ScreenPosition_Vertical4.Right].GetWidth() : 0)); }

            public override async Task<int> GetImageHeight () { return Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.Left) ? await BlazorWebViews[ScreenPosition_Vertical4.Left].GetHeight() : 0, Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.CenterLeft) ? await BlazorWebViews[ScreenPosition_Vertical4.CenterLeft].GetHeight() : 0, Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.CenterRight) ? await BlazorWebViews[ScreenPosition_Vertical4.CenterRight].GetHeight() : 0, (CurrentCaptureMode.HasFlag(CaptureMode_Vertical4.Right)) ? await BlazorWebViews[ScreenPosition_Vertical4.Right].GetHeight() : 0))); }
        }

        public class CaptureSetting_Custom4Mode : CaptureSettingBase<ScreenPosition_Custom4, CaptureMode_Custom4>
        {
            public override async Task<int> GetImageWidth () { return Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Custom4.TopLeft) ? await BlazorWebViews[ScreenPosition_Custom4.TopLeft].GetWidth() : 0, (CurrentCaptureMode.HasFlag(CaptureMode_Custom4.BottomLeft) ? await BlazorWebViews[ScreenPosition_Custom4.BottomLeft].GetWidth() : 0)) + Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Custom4.TopRight) ? await BlazorWebViews[ScreenPosition_Custom4.TopRight].GetWidth() : 0, (CurrentCaptureMode.HasFlag(CaptureMode_Custom4.BottomRight) ? await BlazorWebViews[ScreenPosition_Custom4.BottomRight].GetWidth() : 0)); }

            public override async Task<int> GetImageHeight () { return Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Custom4.TopLeft) ? await BlazorWebViews[ScreenPosition_Custom4.TopLeft].GetHeight() : 0, (CurrentCaptureMode.HasFlag(CaptureMode_Custom4.TopRight) ? await BlazorWebViews[ScreenPosition_Custom4.TopRight].GetHeight() : 0)) + Math.Max(CurrentCaptureMode.HasFlag(CaptureMode_Custom4.BottomLeft) ? await BlazorWebViews[ScreenPosition_Custom4.BottomLeft].GetHeight() : 0, (CurrentCaptureMode.HasFlag(CaptureMode_Custom4.BottomRight) ? await BlazorWebViews[ScreenPosition_Custom4.BottomRight].GetHeight() : 0)); }
        }

        Task<int> GetYoutubePlayerWidth (BlazorWebView blazorWebView);

        Task<int> GetYoutubePlayerHeight (BlazorWebView blazorWebView);

        Task<byte[]> GetCaptureWebViewData(BlazorWebView blazorWebView);

        Task<byte[]> GetCaptureYoutubePlayerData (BlazorWebView blazorWebView);

        Task CaptureSingle (CaptureSetting_Single captureSetting);

        Task CaptureHorizontal2 (CaptureSetting_Horizontal2Mode captureSetting);

        Task CaptureVertical2 (CaptureSetting_Vertical2Mode captureSetting);

        Task CaptureHorizontal3 (CaptureSetting_Horizontal3Mode captureSetting);

        Task CaptureVertical3 (CaptureSetting_Vertical3Mode captureSetting);

        Task CaptureCustom3_1 (CaptureSetting_Custom3_1Mode captureSetting);

        Task CaptureCustom3_2 (CaptureSetting_Custom3_2Mode captureSetting);

        Task CaptureHorizontal4 (CaptureSetting_Horizontal4Mode captureSetting);

        Task CaptureVertical4 (CaptureSetting_Vertical4Mode captureSetting);

        Task CaptureCustom4 (CaptureSetting_Custom4Mode captureSetting);
    }
}
