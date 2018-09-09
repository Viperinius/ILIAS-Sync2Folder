using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using IliasDL;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace WPF_ILIAS_Sync2Folder
{
    /// <summary>
    /// Interaktionslogik für GeneralPage.xaml
    /// </summary>
    public partial class GeneralPage : UserControl
    {
        CConfig config = new CConfig();

        private string sExportPath = "";
        private string sImportPath = "";

        public GeneralPage()
        {
            InitializeComponent();

            iconCheck.Visibility = Visibility.Hidden;
            iconFail.Visibility = Visibility.Hidden;
        }

        private void RadioEnglish_Checked(object sender, RoutedEventArgs e)
        {
            config.SetLanguage("English");
        }

        private void RadioGerman_Checked(object sender, RoutedEventArgs e)
        {
            config.SetLanguage("German");
        }

        private async void BtnCheckLink_Click(object sender, RoutedEventArgs e)
        {
            string sServer = "";
            string sClient = "";

            sServer = config.FormatIliasUrlToWebServiceLink(txtLoginLink.Text, ref sClient);
            if (sServer != "")
            {
                txtClientId.Text = sClient;

                iconCheck.Visibility = Visibility.Visible;
                iconFail.Visibility = Visibility.Hidden;
                iconQuestion.Visibility = Visibility.Hidden;

                config.SetServer(sServer);
                config.SetServerLoginLink(txtLoginLink.Text);
                config.SetClient(sClient);
                config.SetIliasReference(sServer);
            }
            else
            {
                iconCheck.Visibility = Visibility.Hidden;
                iconQuestion.Visibility = Visibility.Hidden;
                iconFail.Visibility = Visibility.Visible;

                txtClientId.Text = "";

                var metroWindow = (Application.Current.MainWindow as MetroWindow);
                var result = await metroWindow.ShowMessageAsync("Error", "Link could not be resolved. Is the correct link specified?", MessageDialogStyle.Affirmative);
            }            
        }

        private void BtnChooseExportPath_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtExportPath.Text = folderBrowserDialog.SelectedPath;
                    sExportPath = txtExportPath.Text;
                }
            }
        }

        private void BtnChooseImportPath_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtImportPath.Text = folderBrowserDialog.SelectedPath;
                    sImportPath = txtImportPath.Text;
                }
            }
            
        }

        private async void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            string sSettingsOrigin = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string sSettingsPath = new Uri(System.IO.Path.GetDirectoryName(sSettingsOrigin)).LocalPath;

            string sSeparator = AddPathSeparator(sExportPath);
            if (sSeparator != "")
            {
                sExportPath = sExportPath + sSeparator;
                sSettingsPath = sSettingsPath + sSeparator;
            }
            else
            {
                var metroWindow = (Application.Current.MainWindow as MetroWindow);
                var result = await metroWindow.ShowMessageAsync("Error", "Error in file path.", MessageDialogStyle.Affirmative);
                return;
            }

            if (File.Exists(sSettingsPath + "WPF-ILIAS-Sync2Folder.exe.config"))
            {
                File.Copy(sSettingsPath + "WPF-ILIAS-Sync2Folder.exe.config", sExportPath + "WPF-ILIAS-Sync2Folder.exe.config", overwrite: true);
            }
            else
            {
                var metroWindow = (Application.Current.MainWindow as MetroWindow);
                var result = await metroWindow.ShowMessageAsync("Error", "Error in settings path.", MessageDialogStyle.Affirmative);
                return;
            }
        }

        private async void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            string sSettingsOrigin = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string sSettingsPath = new Uri(System.IO.Path.GetDirectoryName(sSettingsOrigin)).LocalPath;

            string sSeparator = AddPathSeparator(sImportPath);
            if (sSeparator != "")
            {
                sImportPath = sImportPath + sSeparator;
                sSettingsPath = sSettingsPath + sSeparator;
            }
            else
            {
                var metroWindow = (Application.Current.MainWindow as MetroWindow);
                var result = await metroWindow.ShowMessageAsync("Error", "Error in file path.", MessageDialogStyle.Affirmative);
                return;
            }

            if (File.Exists(sSettingsPath + "WPF-ILIAS-Sync2Folder.exe.config") && File.Exists(sImportPath + "WPF-ILIAS-Sync2Folder.exe.config"))
            {
                File.Copy(sImportPath + "WPF-ILIAS-Sync2Folder.exe.config", sSettingsPath + "WPF-ILIAS-Sync2Folder.exe.config", overwrite: true);
            }
            else
            {
                var metroWindow = (Application.Current.MainWindow as MetroWindow);
                var result = await metroWindow.ShowMessageAsync("Error", "Error in file path. Does the import file exist?", MessageDialogStyle.Affirmative);
                return;
            }
        }

        private void ToggleTrayIcon_Checked(object sender, RoutedEventArgs e)
        {
            config.SetShowTrayIcon(true);
            
        }

        private void ToggleTrayIcon_Unchecked(object sender, RoutedEventArgs e)
        {
            config.SetShowTrayIcon(false);
        }

        private void ToggleUpdates_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleUpdates_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleSyncNotify_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleSyncNotify_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void GeneralPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (config.GetLanguage() == "English")
            {
                radioEnglish.IsChecked = true;
            }
            else
            {
                radioGerman.IsChecked = true;
            }

            txtLoginLink.Text = config.GetServerLoginLink();
            txtClientId.Text = config.GetClient();
        }

        private string AddPathSeparator(string sPath)
        {
            if (sPath.Contains(System.IO.Path.DirectorySeparatorChar))
            {
                return System.IO.Path.DirectorySeparatorChar.ToString();
            }
            else if (sPath.Contains(System.IO.Path.AltDirectorySeparatorChar))
            {
                return System.IO.Path.AltDirectorySeparatorChar.ToString();
            }
            return "";
        }
    }
}
