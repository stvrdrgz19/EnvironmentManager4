using EnvironmentManager4.src.Core.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4.src.Core.Utilities
{
    public class AppVersionUtilities
    {
        public const string latestReleaseApi = "https://api.github.com/repos/stvrdrgz19/EnvironmentManager4/releases/latest";
        public const string releasesPage = "https://github.com/stvrdrgz19/EnvironmentManager4/releases/latest";
        public const string networkShare = @"\\sp-fileserv-01\Team QA";
        public const string unableToConnect = "Unable to Connect";

        private static readonly HttpClient client = CreateClient();
        private static bool releaseChecked;
        private static string latestVersion;
        private static string latestInstallerUrl;

        private static HttpClient CreateClient()
        {
            var http = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
            // GitHub's API rejects requests without a User-Agent
            http.DefaultRequestHeaders.UserAgent.ParseAdd("EnvironmentManager4");
            return http;
        }

        /// <summary>
        /// Queries GitHub for the latest release once per session and caches its version and MSI download link
        /// </summary>
        private static void GetLatestRelease()
        {
            if (releaseChecked)
                return;
            releaseChecked = true;

            try
            {
                string json = client.GetStringAsync(latestReleaseApi).GetAwaiter().GetResult();
                JObject release = JObject.Parse(json);

                latestVersion = ((string)release["tag_name"]).TrimStart('v', 'V');
                latestInstallerUrl = release["assets"]
                    .Select(a => (string)a["browser_download_url"])
                    .FirstOrDefault(url => url.EndsWith(".msi", StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception e)
            {
                ErrorHandling.LogException(e, false, "Unable to reach GitHub to check for updates.");
                latestVersion = null;
                latestInstallerUrl = null;
            }
        }

        /// <summary>
        /// Retrieves the version of the latest published GitHub release
        /// </summary>
        /// <returns>The version of the latest release, or "Unable to Connect" if it could not be retrieved.</returns>
        public static string GetLatestVersion()
        {
            GetLatestRelease();
            return latestVersion ?? unableToConnect;
        }

        /// <summary>
        /// Retrieves the download link for the latest release's installer
        /// </summary>
        /// <returns>The MSI download URL, or null if unavailable</returns>
        public static string GetLatestInstallerUrl()
        {
            GetLatestRelease();
            return latestInstallerUrl;
        }

        /// <summary>
        /// Downloads the latest release's installer to the temp directory
        /// </summary>
        /// <returns>The local path of the downloaded MSI</returns>
        public static string DownloadLatestInstaller()
        {
            string url = GetLatestInstallerUrl();
            if (url == null)
                throw new InvalidOperationException("The latest release does not have an installer attached.");

            string path = Path.Combine(Path.GetTempPath(), String.Format("Environment Manager v{0}.msi", latestVersion));
            using (var download = new HttpClient { Timeout = TimeSpan.FromMinutes(5) })
            {
                download.DefaultRequestHeaders.UserAgent.ParseAdd("EnvironmentManager4");
                byte[] bytes = download.GetByteArrayAsync(url).GetAwaiter().GetResult();
                File.WriteAllBytes(path, bytes);
            }
            return path;
        }

        /// <summary>
        /// Checks whether the QA file share is reachable, which indicates a connection to the company network or VPN
        /// </summary>
        public static bool IsConnectedToNetwork()
        {
            return Directory.Exists(networkShare);
        }

        /// <summary>
        /// Pulls the version number of the current installation of Environment Manager
        /// </summary>
        /// <returns>The version number of the currently installed Environment Manager build</returns>
        public static string GetAppVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Checks the current build number and compares it against the latest available build
        /// </summary>
        /// <returns>True if up to date or the latest version can't be determined, false if outdated</returns>
        public static bool IsUpToDate()
        {
            Version currentVersion = new Version(GetAppVersion());
            if (!Version.TryParse(GetLatestVersion(), out Version latest))
                return true;

            // Handles launching the solution locally with a newer build number
            return currentVersion >= latest;
        }

        public static void CheckForUpdatesAndPrompt(AppSettings settings)
        {
            if (!AppVersionUtilities.IsUpToDate())
            {
                UpdatePrompt update = new UpdatePrompt();
                UpdatePrompt.OpenFromStartup = true;
                update.ShowDialog();
            }
        }
    }
}
