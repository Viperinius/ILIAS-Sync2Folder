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
using System.Diagnostics;
using IliasDL;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace WPF_ILIAS_Sync2Folder
{
    /// <summary>
    /// Interaktionslogik für FolderPage.xaml
    /// </summary>
    public partial class FolderPage : UserControl
    {
        CConfig config = new CConfig();

        public FolderPage()
        {
            InitializeComponent();
        }

        private void BtnChoosePath_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void BtnOpenPath_Click(object sender, RoutedEventArgs e)
        {
            string sPath = config.GetPath();
            if (Directory.Exists(sPath))
            {
                Process.Start("explorer.exe", sPath);
            }
            else
            {
                var metroWindow = (Application.Current.MainWindow as MetroWindow);
                var result = await metroWindow.ShowMessageAsync("Error", "Path not found.", MessageDialogStyle.Affirmative);
            }
        }

        private void ToggleFolderStructure_Checked(object sender, RoutedEventArgs e)
        {
            config.SetUseOwnStructure(true);
        }

        private void ToggleFolderStructure_Unchecked(object sender, RoutedEventArgs e)
        {
            config.SetUseOwnStructure(false);
        }

        private void ToggleUseYear_Checked(object sender, RoutedEventArgs e)
        {
            config.SetUseYearInStructure(true);
        }

        private void ToggleUseYear_Unchecked(object sender, RoutedEventArgs e)
        {
            config.SetUseYearInStructure(false);
        }
    }
}
