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

        private GitHubClient client = new GitHubClient(new ProductHeaderValue("ILIAS-Sync2Folder"));

        private string sRelease;
        private int iReleaseId;

        public CUpdate(NotifyIcon notify)
        {
            notifyIcon = notify;

            notifyIcon.Text = @"ILIAS Sync2Folder";
            notifyIcon.Visible = true;

            iReleaseId = 0;
            sRelease = "v0.0.0.0";
        }

        public void DisplayNotification(string sTitle, string sText)
        {
            notifyIcon.BalloonTipTitle = sTitle;
            notifyIcon.BalloonTipText = sText;
            notifyIcon.ShowBalloonTip(100);
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
                string sAssetUrl = "";

                foreach (var asset in assets)
                {
                    if (asset.Name == sRelease + ".zip")
                    {
                        sAssetUrl = asset.BrowserDownloadUrl;
                    }
                }

                //download the file
                string sFileName = sRelease + ".zip";
                WebClient webClient = new WebClient();
                webClient.DownloadFile(sAssetUrl, Path.GetTempPath() + sFileName);

                //unzip
                string sDestDirectory = Path.GetTempPath() + sRelease;
               
                if (Directory.Exists(sDestDirectory))
                {
                    Directory.Delete(sDestDirectory, recursive: true);
                }

                Directory.CreateDirectory(sDestDirectory);
                ZipFile.ExtractToDirectory(Path.GetTempPath() + sFileName, sDestDirectory);

                string sAppPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                string sFormattedPath = new Uri(Path.GetDirectoryName(sAppPath)).LocalPath;

                string sSeparator = AddPathSeparator(sFormattedPath);
                if (sSeparator != "")
                {
                    sFormattedPath = sFormattedPath + sSeparator;
                    Process.Start(sFormattedPath + @"IliasDL_Updater.exe", sDestDirectory + " " + sFileName + " " + sSeparator);    //needs to be tested!
                    return true;
                }                
            }
            return false;
        }

        private string AddPathSeparator(string sPath)
        {
            if (sPath.Contains(Path.DirectorySeparatorChar))
            {
                return Path.DirectorySeparatorChar.ToString();
            }
            else if (sPath.Contains(Path.AltDirectorySeparatorChar))
            {
                return Path.AltDirectorySeparatorChar.ToString();
            }
            return "";
        }
    }
}
