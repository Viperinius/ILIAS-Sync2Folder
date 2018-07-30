using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Wunder.ClickOnceUninstaller;

namespace IliasDL_Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null)
            {
                return;
            }
            else if (args.Count() < 3)
            {
                return;
            }
            else
            {
                string sDestDirectory = args[0];
                string sFileName = args[1];
                string sSeparator = args[2];

                //uninstall
                var uninstallInfo = UninstallInfo.Find("IliasSync2Folder");
                if (uninstallInfo != null)
                {
                    Uninstaller uninstaller = new Uninstaller();
                    uninstaller.Uninstall(uninstallInfo);
                }

                //launch new version's installer
                Process.Start(sDestDirectory + sSeparator + "setup.exe");


                //cleanup
                if (File.Exists(Path.GetTempPath() + sFileName))
                {

                    File.Delete(Path.GetTempPath() + sFileName);
                }
                if (Directory.Exists(sDestDirectory))
                {
                    Directory.Delete(sDestDirectory, recursive: true);
                }
            }            
        }
    }
}
