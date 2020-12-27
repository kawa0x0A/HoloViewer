using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using AppKit;

[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.macOS.UpdateCheck))]
namespace HoloViewer.macOS
{
    public class UpdateCheck : IUpdateCheck
    {
        private UpdateCheckLibrary.Program program = new UpdateCheckLibrary.Program();

        public bool IsUpdateable()
        {
            try
            {
                return program.IsUpdateable(FeedBackInfoValue.VersionString);
            }
            catch
            {
                return false;
            }
        }

        public bool AskExecuteUpdate()
        {
            var alert = new NSAlert() {
                AlertStyle = NSAlertStyle.Informational,
                MessageText = IUpdateCheck.GetAskDownloadUpdateMessage(FeedBackInfoValue.Version, program.GetLastVersion()),
            };

            alert.AddButton("OK");
            alert.AddButton("Cancel");

            var result = alert.RunModal();

            return (result == 1000);
        }

        public async Task Update()
        {
            await Browser.OpenAsync(program.GetDownloadArchiveUrl(Device.macOS));
        }

        public void DeleteUpdateFiles()
        {
        }

        public void UpdateComplete()
        {
        }
    }
}
