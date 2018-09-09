//==================================================================
//
//    (c) Copyright by Viperinius
//
//==================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Windows;
using Octokit;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace IliasDL
{
    class CUpdate
    {
        private System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();

        private CConfig config = new CConfig();

        private GitHubClient client = new GitHubClient(new ProductHeaderValue("ILIAS-Sync2Folder"));

        private string sRelease;
        private int iReleaseId;
        private string sSeparator;

        public CUpdate(System.Windows.Forms.NotifyIcon notify)
        {
            notifyIcon = notify;

            notifyIcon.Text = @"ILIAS Sync2Folder";
            if (config.GetShowTrayIcon() == "true")
            {
                notifyIcon.Visible = true;
            }
            else
            {
                notifyIcon.Visible = false;
            }

            iReleaseId = 0;
            sRelease = "v0.0.0.0";
        }

        /// <summary>
        /// Show a notification
        /// </summary>
        /// <param name="sTitle">Title of the tip</param>
        /// <param name="sText">Text to display</param>
        public void DisplayNotification(string sTitle, string sText)
        {
            notifyIcon.BalloonTipTitle = sTitle;
            notifyIcon.BalloonTipText = sText;
            notifyIcon.ShowBalloonTip(100);
        }

        /// <summary>
        /// Checks if the updater programme exists in temp path
        /// </summary>
        /// <returns>True if updater is found</returns>
        public bool CheckForUpdater()
        {
            if (File.Exists(Path.GetTempPath() + @"IliasDL_Updater.exe"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Examines the latest release version on Github
        /// </summary>
        /// <returns>Latest version or 0.0.0.0 in case of errors</returns>
        public Version GetReleaseVersion()
        {          
            try
            {
                //var latest = client.Repository.Release.GetLatest("Viperinius", "ILIAS-Sync2Folder").Result;
                var latest = client.Repository.Release.GetAll("Viperinius", "ILIAS-Sync2Folder").Result;

                Console.WriteLine(latest[0].TagName);
                Console.WriteLine(latest[0].Name);
                //Console.WriteLine(latest.TagName);

                iReleaseId = latest[0].Id;
                sRelease = latest[0].TagName;

                var formatted = latest[0].TagName.Split('v');

                Version version = new Version(formatted[1]);
                //Version version = new Version(latest.TagName);

                return version;
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException.Message == "Not Found")
                {
                    MessageBox.Show("No Github releases found!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                Version version = new Version("0.0.0.0");

                return version;
            }                                  
        }

        /// <summary>
        /// Checks if the version running is outdated
        /// </summary>
        /// <returns>True if update available</returns>
        public bool CheckForUpdate()
        {
            Version versionGithub = GetReleaseVersion();
            Version versionLocal = new Version(System.Windows.Forms.Application.ProductVersion);

            if (versionGithub > versionLocal)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Downloads the new version and the updater and launches the latter
        /// </summary>
        /// <returns>False if something fails</returns>
        public bool GetUpdate()
        {
            if (iReleaseId != 0)
            {
                var assets = client.Repository.Release.GetAllAssets("Viperinius", "ILIAS-Sync2Folder", iReleaseId).Result;
                string sAssetUrlRelease = "";
                string sAssetUrlUpdater = "";

                string sFileNameRelease = sRelease + ".zip";
                string sFileNameUpdater = @"IliasDL_Updater.exe";

                foreach (var asset in assets)
                {
                    if (asset.Name == sFileNameRelease)
                    {
                        sAssetUrlRelease = asset.BrowserDownloadUrl;
                    }
                    else if (asset.Name == sFileNameUpdater)
                    {
                        sAssetUrlUpdater = asset.BrowserDownloadUrl;
                    }
                }

                //download the files
                WebClient webClient = new WebClient();
                webClient.DownloadFile(sAssetUrlRelease, Path.GetTempPath() + sFileNameRelease);
                webClient.DownloadFile(sAssetUrlUpdater, Path.GetTempPath() + sFileNameUpdater);

                //unzip
                string sDestDirectory = Path.GetTempPath() + sRelease;
               
                if (Directory.Exists(sDestDirectory))
                {
                    Directory.Delete(sDestDirectory, recursive: true);
                }

                Directory.CreateDirectory(sDestDirectory);
                ZipFile.ExtractToDirectory(Path.GetTempPath() + sFileNameRelease, sDestDirectory);

                //start updater
                string sUpdaterPath = Path.GetTempPath() + sFileNameUpdater;
                if (CheckForUpdater())
                {
                    Process.Start(sUpdaterPath, sDestDirectory + " " + sFileNameRelease + " " + sSeparator);    //needs to be tested!
                    return true;
                }
            }
            return false;
        }
    }
}
