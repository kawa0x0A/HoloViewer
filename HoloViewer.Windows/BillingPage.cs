[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.Windows.BillingPage))]
namespace HoloViewer.Windows
{
    class BillingPage : IBillingPage
    {
        public void OpenPage ()
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(IBillingPage.BillingPageUrl) { UseShellExecute = true });
        }
    }
}
