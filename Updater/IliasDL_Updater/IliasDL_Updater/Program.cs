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
                    Console.WriteLine("Uninstalling current ILIAS Sync2Folder version...");
                    Uninstaller uninstaller = new Uninstaller();
                    uninstaller.Uninstall(uninstallInfo);
                    Console.WriteLine("Uninstalling done.");
                }

                //launch new version's installer
                Console.WriteLine("Launching new installer...");
                var installer = Process.Start(sDestDirectory + sSeparator + "setup.exe");
                

                installer.WaitForExit(milliseconds: 20000);
                Process[] processes = Process.GetProcessesByName("dfsvc");
                if (processes.Length != 0)
                {
                    Console.WriteLine("ClickOnce found running");

                    Console.WriteLine("Waiting for the installer to close...");
                    processes[0].WaitForExit(milliseconds: 20000);
                    Console.WriteLine("Installer exited.");
                }
                else
                {
                    Console.WriteLine("ClickOnce not running");
                }

                

                //cleanup
                Console.WriteLine("Removing temporary install files...");
                if (File.Exists(Path.GetTempPath() + sFileName))
                {

                    File.Delete(Path.GetTempPath() + sFileName);
                }
                if (Directory.Exists(sDestDirectory))
                {
                    Directory.Delete(sDestDirectory, recursive: true);
                }
                Console.WriteLine("Done.");
            }            
        }
    }
}
