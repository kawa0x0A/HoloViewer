using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Octokit;

namespace UpdateCheckLibrary
{
    public class Program : IDisposable
    {
        private readonly GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("HoloViewer"));
        private readonly HttpClient httpClient = new HttpClient();

        private const string UpdateDirectoryName = "Update";
        private const string DownloadDirectoryName = "Download";
        private const string ExtractDirectoryName = "Extract";

        private static string DownloadDirectoryPath { get { return Path.Combine(UpdateDirectoryName, DownloadDirectoryName); } }
        private static string ExtractDirectoryPath { get { return Path.Combine(UpdateDirectoryName, ExtractDirectoryName); } }

        public long DownloadedSize { get; private set; }

        public long ContentSize { get; private set; }

        void IDisposable.Dispose ()
        {
            httpClient.Dispose();
        }

        private Release GetLastRelease ()
        {
            try
            {
                var releases = gitHubClient.Repository.Release.GetAll("kawa0x0A", "HoloViewer").Result;

                if (releases.Count > 0)
                {
                    return releases.OrderByDescending(release => release.CreatedAt).First();
                }
            }
            catch
            {
                throw;
            }

            return null;
        }

        public Version GetLastVersion ()
        {
            var lastRelease = GetLastRelease();

            return Version.Parse(lastRelease.TagName);
        }

        public bool IsUpdateable (string currentVersionString)
        {
            try
            {
                var lastRelease = GetLastRelease();

                if (lastRelease == null)
                {
                    return false;
                }

                var lastReleaseVersion = Version.Parse(lastRelease.TagName);
                var currentVersion = Version.Parse(currentVersionString);

                if (lastReleaseVersion > currentVersion)
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }

            return false;
        }

        public void RefreshUpdateData ()
        {
            if (Directory.Exists(UpdateDirectoryName))
            {
                Directory.Delete(UpdateDirectoryName, true);
            }
        }

        public async Task DownloadLastReleaseArchive (string platform)
        {
            var lastRelease = GetLastRelease();

            string assetName = "";

            switch (platform)
            {
                case Device.WPF:
                    assetName = "HoloViewer_Windows_x64.zip";
                    break;

                case Device.macOS:
                    assetName = "HoloViewer_macOS.pkg";
                    break;
            }

            var releaseAsset = lastRelease.Assets.SingleOrDefault(asset => asset.Name == assetName);

            Directory.CreateDirectory(DownloadDirectoryPath);

            var archivePath = Path.Combine(DownloadDirectoryPath, releaseAsset.Name);

            httpClient.Timeout = System.Threading.Timeout.InfiniteTimeSpan;

            try
            {
                var httpResponseMessage = await httpClient.GetAsync(releaseAsset.BrowserDownloadUrl, HttpCompletionOption.ResponseHeadersRead);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    ContentSize = (long)httpResponseMessage.Content.Headers.ContentLength;

                    using (var content = httpResponseMessage.Content)
                    using (var stream = await content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream(archivePath, System.IO.FileMode.Create))
                    {
                        while (true)
                        {
                            var buffer = new byte[1024];
                            var readSize = await stream.ReadAsync(buffer, 0, buffer.Length);

                            if (readSize == 0)
                            {
                                break;
                            }

                            await fileStream.WriteAsync(buffer, 0, readSize);

                            DownloadedSize = fileStream.Length;
                        }
                    }
                }
                else
                {
                    throw new HttpRequestException(httpResponseMessage.StatusCode.ToString());
                }
            }
            catch
            {
                throw;
            }
        }

        public void CancelDownload ()
        {
            httpClient.CancelPendingRequests();
        }

        public void ExtractArchive ()
        {
            var files = Directory.GetFiles(DownloadDirectoryPath);
            var zipFiles = files.Where(filePath => Path.GetExtension(filePath) == ".zip").ToArray();

            foreach (var zipFile in zipFiles)
            {
                ZipFile.ExtractToDirectory(zipFile, Path.Combine(ExtractDirectoryPath, Path.GetFileNameWithoutExtension(zipFile)));
            }
        }

        public static string GetUpdaterPath ()
        {
            var archiveExtractedDirectoryPath = Directory.GetDirectories(ExtractDirectoryPath).SingleOrDefault();

            var files = Directory.GetFiles(archiveExtractedDirectoryPath);

            return files.SingleOrDefault(filePath => filePath.IndexOf("UpdateChecker.exe") >= 0);
        }

        protected static void CopyExtractFiles ()
        {
            var archiveExtractedDirectoryPath = Directory.GetDirectories(ExtractDirectoryPath).SingleOrDefault();

            var files = Directory.GetFiles(archiveExtractedDirectoryPath);

            foreach (var filePath in files.Where(path => !(path.IndexOf("UpdateChecker.exe") >= 0)))
            {
                var destFilePath = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileName(filePath));

                File.Copy(filePath, destFilePath, true);
            }

            var directories = Directory.GetDirectories(archiveExtractedDirectoryPath);

            foreach (var directoryPath in directories)
            {
                var oldDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), Path.GetFileName(Path.GetDirectoryName(directoryPath + Path.DirectorySeparatorChar)));

                Directory.Delete(oldDirectoryPath, true);

                CopyDirectory(directoryPath, oldDirectoryPath);
            }
        }

        private static void CopyDirectory (string sourceDirectoryPath, string distDirectoryPath)
        {
            if (!Directory.Exists(distDirectoryPath))
            {
                Directory.CreateDirectory(distDirectoryPath);
            }

            const string ExtractRootName = "HoloViewer_Windows_x64";

            foreach (var filePath in Directory.GetFiles(sourceDirectoryPath))
            {
                var distPath = Path.Combine(Directory.GetCurrentDirectory(), filePath.Substring(filePath.IndexOf(ExtractRootName) + ExtractRootName.Length + 1));

                File.Copy(filePath, distPath, true);
            }

            foreach (var directoryPath in Directory.GetDirectories(sourceDirectoryPath))
            {
                var oldDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), directoryPath.Substring(directoryPath.IndexOf(ExtractRootName) + ExtractRootName.Length + 1));

                CopyDirectory(directoryPath, oldDirectoryPath);
            }
        }
    }
}
