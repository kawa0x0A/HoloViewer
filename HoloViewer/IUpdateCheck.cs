using System;
using System.Threading.Tasks;

namespace HoloViewer
{
    public interface IUpdateCheck
    {
        protected const string UpdaterFileName = "UpdateChecker.exe";

        protected const string UpdateCaption = "Update";

        protected const string UpdateCompletedMessage = "アップデートが完了しました。";
        protected const string UpdateInfoErrorMessage = "アップデート情報の取得に失敗しました。";
        protected const string UpdateErrorMessage = "アップデートに失敗しました。";
        protected const string UpdateCancelMessage = "アップデートを中止しました。";

        protected const string UpdateStartStateText = "アップデート開始";
        protected const string UpdatingStateText = "アップデート中";
        protected const string UpdateCompletedStateText = "アップデート完了";

        protected const string InitializeFunctionText = "初期化中";
        protected const string DownloadFunctionText = "ダウンロード中";
        protected const string ExtractFunctionText = "展開中";
        protected const string CopyFunctionText = "コピー中";
        protected const string DeleteUpdateFileFunctionText = "アップデートファイル削除中";
        protected const string UpdateCompleteFunctionText = "アップデート完了";

        protected static string GetAskExecuteUpdateMessage (Version currentVersion, Version lastVersion)
        {
            return $"新しいバージョンが見つかりました。\nアップデートを行いますか?\n現在のバージョン {currentVersion}\n最新のバージョン {lastVersion}";
        }

        protected static string GetAskDownloadUpdateMessage (Version currentVersion, Version lastVersion)
        {
            return $"新しいバージョンが見つかりました。\nダウンロードしますか?\n現在のバージョン {currentVersion}\n最新のバージョン {lastVersion}";
        }

        protected static string GetDownloadInfoText (long downloadedSize, long contentSize)
        {
            float parcent = (contentSize == 0) ? 0 : ((float)downloadedSize / contentSize);

            return $"{downloadedSize} / {contentSize}\n{parcent.ToString("0%")}";
        }

        bool IsUpdateable ();

        bool AskExecuteUpdate ();

        Task Update ();

        void DeleteUpdateFiles ();

        void UpdateComplete ();
    }
}
