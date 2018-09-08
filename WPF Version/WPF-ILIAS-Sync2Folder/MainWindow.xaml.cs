using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
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
using System.Runtime.CompilerServices;
using MahApps.Metro.Controls;
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;

namespace WPF_ILIAS_Sync2Folder
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public bool bLoggedIn;

        ChangedPropertyNotifier changedPropertyNotifier = new ChangedPropertyNotifier();
        
        public MainWindow()
        {
            
            InitializeComponent();

            BtnLogin.Tag = PackIconOcticonsKind.SignIn;

            textblockLogin.DataContext = changedPropertyNotifier;
            rectLoginIcon.DataContext = changedPropertyNotifier;

            tabSync.Content = new SyncPage();
            tabCourseConfig.Content = new CoursePage();
            tabFolderConfig.Content = new FolderPage();
            tabGeneralConfig.Content = new GeneralPage();
            tabInfo.Content = new HelpPage();

        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!bLoggedIn)
            {
                ShowLoginDialog();
                changedPropertyNotifier.ChangeBtnLoginText();
                bLoggedIn = true;
                BtnLogin.Tag = PackIconOcticonsKind.SignOut;
            }
            else
            {
                var metroWindow = (Application.Current.MainWindow as MetroWindow);
                var result = await metroWindow.ShowMessageAsync("Logout", "Do you want to log out?", MessageDialogStyle.AffirmativeAndNegative);

                if (result == MessageDialogResult.Affirmative)
                {
                    changedPropertyNotifier.ChangeBtnLoginText();
                    bLoggedIn = false;
                    BtnLogin.Tag = PackIconOcticonsKind.SignIn;
                }
            }            
        }

        private void BtnStyle_Click(object sender, RoutedEventArgs e)
        {
            StyleChanger styleChanger = new StyleChanger();
            styleChanger.Show();
        }

        public async void ShowLoginDialog()
        {
            var metroWindow = (Application.Current.MainWindow as MetroWindow);
            var result = await metroWindow.ShowLoginAsync("ILIAS Login", "Please log in with your ILIAS credentials");
            Console.WriteLine("User: " + result.Username);
            Console.WriteLine("Pass: " + result.Password);
        }

        

        
    }
}
