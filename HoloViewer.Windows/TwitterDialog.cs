using System.Linq;
using System.Threading.Tasks;
using Microsoft.MobileBlazorBindings.Elements;

[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.Windows.TwitterDialog))]
namespace HoloViewer.Windows
{
    class TwitterDialog : ITwitterDialog
    {
        public async Task<string[]> GetHashTags (BlazorWebView blazorWebView)
        {
            var hashTagArray = await WebView.ExecuteJavascript(blazorWebView, ITwitterDialog.GetHashTagsScriptPath);

            return TwitterUtility.ConvertStringToHashTagArray(hashTagArray);
        }

        public async Task Tweet (string[] hashTags)
        {
            var tweetWindow = new WebViewWindow();

            tweetWindow.Show();

            await Task.Run(() => { while (!tweetWindow.IsInitializationComplated) { } });

            tweetWindow.CurrentWebViewWindowDataSet.SourceUrl = TwitterUtility.CreateTweetUrl(hashTags);
        }
    }
}
