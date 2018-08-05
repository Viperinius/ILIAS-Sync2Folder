using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Diagnostics;
using Octokit;

namespace IliasDL
{
    class CUpdate
    {
        private NotifyIcon notifyIcon = new NotifyIcon();

        private CConfig config = new CConfig();

        private GitHubClient client = new GitHubClient(new ProductHeaderValue("ILIAS-Sync2Folder"));

        private string sRelease;
        private int iReleaseId;
        private string sSeparator;

        public CUpdate(NotifyIcon notify)
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

        public void DisplayNotification(string sTitle, string sText)
        {
            notifyIcon.BalloonTipTitle = sTitle;
            notifyIcon.BalloonTipText = sText;
            notifyIcon.ShowBalloonTip(100);
        }

        public bool CheckForUpdater()
        {
            if (File.Exists(Path.GetTempPath() + @"IliasDL_Updater.exe"))
            {
                return true;
            }
            return false;
        }

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
                    MessageBox.Show("Keine Github-Releases gefunden!", "Warnung", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                Version version = new Version("0.0.0.0");

                return version;
            }                                  
        }

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
