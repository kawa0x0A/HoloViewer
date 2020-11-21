using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.macOS.BillingPage))]
namespace HoloViewer.macOS
{
    class BillingPage : IBillingPage
    {
        public async void OpenPage ()
        {
            await Browser.OpenAsync(IBillingPage.BillingPageUrl);
        }
    }
}
