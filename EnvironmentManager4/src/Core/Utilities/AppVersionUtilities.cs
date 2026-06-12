using EnvironmentManager4.src.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager4.src.Core.Utilities
{
    public class AppVersionUtilities
    {
        /// <summary>
        /// Reaches out to the fileserver to retrieve the latest generated build version
        /// </summary>
        /// <returns>The version of the latest generated build.</returns>
        public static string GetLatestVersion()
        {
            var directory = @"\\sp-fileserv-01\Team QA\Tools\Environment Manager\Installers";
            string pattern = "*.msi";
            string version;

            try
            {
                var dirInfo = new DirectoryInfo(directory);
                var file = (from f in dirInfo.GetFiles(pattern) orderby f.LastWriteTime descending select f).First();

                version = file.ToString().Substring(21, file.ToString().Length - 25);
            }
            catch (Exception e)
            {
                string extraMessage = "Possibly not connected to the SalesPad Network or VPN.";
                ErrorHandling.LogException(e, false, extraMessage);
                version = "Unable to Connect";
                Toasts.Toast(version, extraMessage);
            }
            return version;
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
        /// <returns>True if up to date, false if outdated</returns>
        public static bool IsUpToDate()
        {
            Version currentVersion = new Version(GetAppVersion());
            Version latestVersion = new Version(GetLatestVersion());

            // Handles launching the solution locally with a newer build number
            if (currentVersion >= latestVersion)
                return true;
            else
                return false;
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
