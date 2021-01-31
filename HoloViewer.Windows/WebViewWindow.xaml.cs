using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Web.WebView2.Wpf;

namespace HoloViewer.Windows
{
    /// <summary>
    /// WebViewWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class WebViewWindow : Window
    {
        public class WebViewWindowDataSet
        {
            private string sourceUrl;

            public string SourceUrl { get { return sourceUrl; } set { sourceUrl = value; BindingWebBrowser?.CoreWebView2?.Navigate(value); } }

            public WebView2 BindingWebBrowser { get; set; }
        }

        public WebViewWindowDataSet CurrentWebViewWindowDataSet { get; set; } = new WebViewWindowDataSet();

        public bool IsInitializationComplated { get; set; } = false;

        public WebViewWindow ()
        {
            InitializeComponent();

            InitializeWebview2();

            CurrentWebViewWindowDataSet.BindingWebBrowser = WebBrowserView;

            DataContext = CurrentWebViewWindowDataSet;
        }

        public async void InitializeWebview2 ()
        {
            await WebBrowserView.EnsureCoreWebView2Async();
        }

        private void WebBrowserView_CoreWebView2InitializationCompleted (object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            IsInitializationComplated = true;
        }
    }
}
