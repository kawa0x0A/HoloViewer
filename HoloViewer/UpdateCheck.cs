using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HoloViewer
{
    public class UpdateCheck
    {
        private const string UpdaterFileName = "UpdateChecker.exe";

        private const string UpdateCaption = "Update";

        private const string UpdateCompletedMessage = "アップデートが完了しました。";
        private const string UpdateInfoErrorMessage = "アップデート情報の取得に失敗しました。";
        private const string UpdateErrorMessage = "アップデートに失敗しました。";
        private const string UpdateCancelMessage = "アップデートを中止しました。";

        private const string UpdateStartStateText = "アップデート開始";
        private const string UpdatingStateText = "アップデート中";
        private const string UpdateCompletedStateText = "アップデート完了";

        private const string InitializeFunctionText = "初期化中";
        private const string DownloadFunctionText = "ダウンロード中";
        private const string ExtractFunctionText = "展開中";
        private const string CopyFunctionText = "コピー中";
        private const string DeleteUpdateFileFunctionText = "アップデートファイル削除中";
        private const string UpdateCompleteFunctionText = "アップデート完了";

        private static string GetAskExecuteUpdateMessage(Version currentVersion, Version lastVersion)
        {
            return $"新しいバージョンが見つかりました。\nアップデートを行いますか?\n現在のバージョン {currentVersion}\n最新のバージョン {lastVersion}";
        }

        private static string GetAskDownloadUpdateMessage(Version currentVersion, Version lastVersion)
        {
            return $"新しいバージョンが見つかりました。\nダウンロードしますか?\n現在のバージョン {currentVersion}\n最新のバージョン {lastVersion}";
        }

        private static string GetDownloadInfoText(long downloadedSize, long contentSize)
        {
            float parcent = (contentSize == 0) ? 0 : ((float)downloadedSize / contentSize);

            return $"{downloadedSize} / {contentSize}\n{parcent:0%}";
        }

        private readonly UpdateCheckLibrary.Program updateCheckerProgram = new();
        private static readonly CancellationTokenSource cancellationTokenSource = new();

        public async Task<bool> IsUpdateable ()
        {
            try
            {
                return updateCheckerProgram.IsUpdateable(FeedBackInfoValue.VersionString);
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert(UpdateCaption, UpdateInfoErrorMessage, "OK");

                return false;
            }
        }

        public async Task<bool> AskExecuteUpdate (DevicePlatform devicePlatform)
        {
            if (devicePlatform == DevicePlatform.WinUI)
            {
                return await Application.Current.MainPage.DisplayAlert(UpdateCaption, GetAskExecuteUpdateMessage(FeedBackInfoValue.Version, updateCheckerProgram.GetLastVersion()), "Yes", "No");
            }
            else if ((devicePlatform == DevicePlatform.MacCatalyst) || (devicePlatform == DevicePlatform.macOS))
            {
                return await Application.Current.MainPage.DisplayAlert(UpdateCaption, GetAskDownloadUpdateMessage(FeedBackInfoValue.Version, updateCheckerProgram.GetLastVersion()), "Yes", "No");
            }
            else
            {
                return false;
            }
        }

        public async Task Update ()
        {
            var updatePage = new UpdatePage();

            await Application.Current.MainPage.Navigation.PushAsync(updatePage);

            await Task.Run(() =>
            {
                updatePage.BindingUpdateDataSet.StateProgressValue = 0;
                updatePage.BindingUpdateDataSet.StateTextValue = UpdateStartStateText;
            });

            try
            {
                await Task.Run(() =>
                {
                    updatePage.BindingUpdateDataSet.StateTextValue = UpdatingStateText;
                    updatePage.BindingUpdateDataSet.FunctionTextValue = InitializeFunctionText;
                });

                UpdateCheckLibrary.Program.RefreshUpdateData();

                await Task.Run(() =>
                {
                    updatePage.BindingUpdateDataSet.StateProgressValue = 0.25f;
                    updatePage.BindingUpdateDataSet.FunctionTextValue = DownloadFunctionText;
                });

#pragma warning disable CS4014
                Task.Run(async () => { await updateCheckerProgram.DownloadLastReleaseArchiveAsync(DeviceInfo.Current.Platform); }, cancellationTokenSource.Token);
#pragma warning restore CS4014

                await Task.Run(() =>
                {
                    while ((updateCheckerProgram.ContentSize == 0) || (updateCheckerProgram.DownloadedSize != updateCheckerProgram.ContentSize))
                    {
                        if (updatePage.BindingUpdateDataSet.IsCancelDownload)
                        {
                            cancellationTokenSource.Cancel();
                            updateCheckerProgram.CancelDownload();
                            break;
                        }

                        updatePage.BindingUpdateDataSet.FunctionProgressValue = (updateCheckerProgram.ContentSize == 0) ? 0 : (((float)updateCheckerProgram.DownloadedSize / updateCheckerProgram.ContentSize));
                        updatePage.BindingUpdateDataSet.FunctionProgressTextValue = GetDownloadInfoText(updateCheckerProgram.DownloadedSize, updateCheckerProgram.ContentSize);
                    }

                    updatePage.BindingUpdateDataSet.FunctionProgressValue = 1.0f;
                });

                await Task.Run(() =>
                {
                    updatePage.BindingUpdateDataSet.StateProgressValue = 0.50f;
                    updatePage.BindingUpdateDataSet.FunctionTextValue = ExtractFunctionText;
                });

                UpdateCheckLibrary.Program.ExtractArchive();

                await Task.Run(() =>
                {
                    updatePage.BindingUpdateDataSet.StateProgressValue = 0.75f;
                    updatePage.BindingUpdateDataSet.FunctionTextValue = CopyFunctionText;
                });

                var updaterPath = UpdateCheckLibrary.Program.GetUpdaterPath();

                if (updaterPath != null)
                {
                    var newUpdaterPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, updaterPath);

                    if (File.Exists(newUpdaterPath))
                    {
                        var oldUpdaterPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UpdaterFileName);

                        File.Copy(newUpdaterPath, oldUpdaterPath, true);
                    }
                }

                await Task.Run(() =>
                {
                    updatePage.BindingUpdateDataSet.StateProgressValue = 1.0f;
                    updatePage.BindingUpdateDataSet.FunctionTextValue = UpdateCompleteFunctionText;
                });

                var process = Process.Start(new ProcessStartInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UpdaterFileName), $"/Update {Process.GetCurrentProcess().Id}"));

                process.WaitForExit();
            }
            catch
            {
                if (updatePage.BindingUpdateDataSet.IsCancelDownload)
                {
                    await Application.Current.MainPage.DisplayAlert(UpdateCaption, UpdateCancelMessage, "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(UpdateCaption, UpdateErrorMessage, "OK");
                }
            }
            finally
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        public static void DeleteUpdateFiles ()
        {
            UpdateCheckLibrary.Program.RefreshUpdateData();
        }

        public static async Task UpdateComplete ()
        {
            await Application.Current.MainPage.DisplayAlert(UpdateCaption, UpdateCompletedMessage, "OK");
        }
    }
}
