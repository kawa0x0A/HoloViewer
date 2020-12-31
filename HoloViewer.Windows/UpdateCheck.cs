using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(HoloViewer.Windows.UpdateCheck))]
namespace HoloViewer.Windows
{
    class UpdateCheck : IUpdateCheck
    {
        private UpdateChecker.Program updateCheckerProgram = new UpdateChecker.Program();
        private static readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public bool IsUpdateable ()
        {
            try
            {
                return updateCheckerProgram.IsUpdateable(FeedBackInfoValue.VersionString);
            }
            catch
            {
                MessageBox.Show(IUpdateCheck.UpdateInfoErrorMessage);

                return false;
            }
        }

        public bool AskExecuteUpdate ()
        {
            var result = MessageBox.Show(IUpdateCheck.GetAskExecuteUpdateMessage(FeedBackInfoValue.Version, updateCheckerProgram.GetLastVersion()), IUpdateCheck.UpdateCaption, MessageBoxButton.YesNo);

            return (result == MessageBoxResult.Yes);
        }

        public async Task Update ()
        {
            var updateWindow = new UpdateWindow();

            WindowUtility.SetLocateCenter(updateWindow);

            updateWindow.Show();

            await Task.Run(() =>
            {
                updateWindow.UpdateDataSetValue.StateProgressValue = 0;
                updateWindow.UpdateDataSetValue.StateTextValue = IUpdateCheck.UpdateStartStateText;
            });

            try
            {
                await Task.Run(() =>
                {
                    updateWindow.UpdateDataSetValue.StateTextValue = IUpdateCheck.UpdatingStateText;
                    updateWindow.UpdateDataSetValue.FunctionTextValue = IUpdateCheck.InitializeFunctionText;
                });

                updateCheckerProgram.RefreshUpdateData();

                await Task.Run(() =>
                {
                    updateWindow.UpdateDataSetValue.StateProgressValue = 25.0f;
                    updateWindow.UpdateDataSetValue.FunctionTextValue = IUpdateCheck.DownloadFunctionText;
                });

#pragma warning disable 4014
                Task.Run(async () => { await updateCheckerProgram.DownloadLastReleaseArchive(Device.WPF); }, cancellationTokenSource.Token);
#pragma warning restore

                await Task.Run(() =>
                {
                    while ((updateCheckerProgram.ContentSize == 0) || (updateCheckerProgram.DownloadedSize != updateCheckerProgram.ContentSize))
                    {
                        if (updateWindow.UpdateDataSetValue.IsCancelDownload)
                        {
                            cancellationTokenSource.Cancel();
                            updateCheckerProgram.CancelDownload();
                            break;
                        }

                        updateWindow.UpdateDataSetValue.FunctionProgressValue = (updateCheckerProgram.ContentSize == 0) ? 0 : (((float)updateCheckerProgram.DownloadedSize / updateCheckerProgram.ContentSize) * 100);
                        updateWindow.UpdateDataSetValue.FunctionProgressTextValue = IUpdateCheck.GetDownloadInfoText(updateCheckerProgram.DownloadedSize, updateCheckerProgram.ContentSize);
                    }

                    updateWindow.UpdateDataSetValue.FunctionProgressValue = 100.0f;
                });

                await Task.Run(() =>
                {
                    updateWindow.UpdateDataSetValue.StateProgressValue = 50.0f;
                    updateWindow.UpdateDataSetValue.FunctionTextValue = IUpdateCheck.ExtractFunctionText;
                });

                updateCheckerProgram.ExtractArchive();

                await Task.Run(() =>
                {
                    updateWindow.UpdateDataSetValue.StateProgressValue = 75.0f;
                    updateWindow.UpdateDataSetValue.FunctionTextValue = IUpdateCheck.CopyFunctionText;
                });

                var updaterPath = UpdateChecker.Program.GetUpdaterPath();

                if (updaterPath != null)
                {
                    var newUpdaterPath = Path.Combine(Directory.GetCurrentDirectory(), updaterPath);

                    if (File.Exists(newUpdaterPath))
                    {
                        var oldUpdaterPath = Path.Combine(Directory.GetCurrentDirectory(), IUpdateCheck.UpdaterFileName);

                        File.Copy(oldUpdaterPath, newUpdaterPath, true);
                    }
                }

                await Task.Run(() =>
                {
                    updateWindow.UpdateDataSetValue.StateProgressValue = 100.0f;
                    updateWindow.UpdateDataSetValue.FunctionTextValue = IUpdateCheck.UpdateCompleteFunctionText;
                });

                var process = Process.Start(new ProcessStartInfo("UpdateChecker.exe", $"/Update {Process.GetCurrentProcess().Id}"));

                process.WaitForExit();
            }
            catch
            {
                if (updateWindow.UpdateDataSetValue.IsCancelDownload)
                {
                    MessageBox.Show(IUpdateCheck.UpdateCancelMessage);
                }
                else
                {
                    MessageBox.Show(IUpdateCheck.UpdateErrorMessage);
                }
            }
            finally
            {
                updateWindow.Close();
            }
        }

        public void DeleteUpdateFiles ()
        {
            updateCheckerProgram.RefreshUpdateData();
            
        }

        public void UpdateComplete ()
        {
            MessageBox.Show(System.Windows.Application.Current.MainWindow, IUpdateCheck.UpdateCompletedMessage);
        }
    }
}
