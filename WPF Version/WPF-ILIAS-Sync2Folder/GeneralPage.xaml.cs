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
using IliasDL;

namespace WPF_ILIAS_Sync2Folder
{
    /// <summary>
    /// Interaktionslogik für GeneralPage.xaml
    /// </summary>
    public partial class GeneralPage : UserControl
    {
        CConfig config = new CConfig();

        public GeneralPage()
        {
            InitializeComponent();
        }

        private void RadioEnglish_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RadioGerman_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCheckLink_Click(object sender, RoutedEventArgs e)
        {
            string sServer = "";
            string sClient = "";

            //...

            config.SetServer(sServer);
            config.SetClient(sClient);
        }

        private void BtnChooseExportPath_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnChooseImportPath_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
