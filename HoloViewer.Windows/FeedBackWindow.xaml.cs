using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HoloViewer.Windows
{
    /// <summary>
    /// FeedBackWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class FeedBackWindow : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_MINIMIZEBOX = 0x00020000;
        private const int WS_MAXIMIZEBOX = 0x00010000;

        public FeedBackWindow ()
        {
            InitializeComponent();

            DataContext = new FeedBackInfo();
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr (IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint="SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr (IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        private void Window_Loaded (object sender, RoutedEventArgs e)
        {
            var handle = (new WindowInteropHelper(this)).Handle;
            var style = GetWindowLongPtr(handle, GWL_STYLE);
            var styleValue = style.ToInt64();

            styleValue &= (~(WS_MAXIMIZEBOX | WS_MINIMIZEBOX));

            SetWindowLongPtr(handle, GWL_STYLE, new IntPtr(styleValue));
        }

        private void Hyperlink_RequestNavigate (object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        }

        private void Button_Click (object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
