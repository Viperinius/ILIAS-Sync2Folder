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
using IliasDL;

namespace WPF_ILIAS_Sync2Folder
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public bool bLoggedIn;

        private System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        private ChangedPropertyNotifier changedPropertyNotifier = new ChangedPropertyNotifier();
        private CConfig config = new CConfig();
        private CUpdate updater;

        private BackgroundWorker workerLogin;
        private BackgroundWorker workerSync;
        private BackgroundWorker workerCourses;

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


            notifyIcon.Icon = Properties.Resources.dliconWHITEsquare;
            updater = new CUpdate(notifyIcon);
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

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            workerLogin = new BackgroundWorker
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = true
            };

            workerLogin.DoWork += new DoWorkEventHandler(WorkerLogin_DoWork);
            workerLogin.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerLogin_RunWorkerCompleted);

            if (workerLogin.IsBusy)
            {
                workerLogin.CancelAsync();
            }

            workerSync = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            workerSync.DoWork += new DoWorkEventHandler(WorkerSync_DoWork);
            workerSync.ProgressChanged += new ProgressChangedEventHandler(WorkerSync_ProgressChanges);
            workerSync.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerSync_RunWorkerCompleted);

            if (workerSync.IsBusy)
            {
                workerSync.CancelAsync();
            }

            workerCourses = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            workerCourses.DoWork += new DoWorkEventHandler(WorkerCourses_DoWork);
            workerCourses.ProgressChanged += new ProgressChangedEventHandler(WorkerCourses_ProgressChanges);
            workerCourses.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCourses_RunWorkerCompleted);

            if (workerCourses.IsBusy)
            {
                workerCourses.CancelAsync();
            }

            //-----------------------------
            //      check for update
            //-----------------------------

            if (config.GetUpdateCheck() == "true")
            {
                if (updater.CheckForUpdate())
                {
                    //notify user
                    updater.DisplayNotification("Update available", "A new update for ILIAS Sync2Folder has been found!");
                }
            }
        }



        //-----------------------------
        //      worker functions
        //-----------------------------

        private void WorkerLogin_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void WorkerLogin_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void WorkerSync_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void WorkerSync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void WorkerSync_ProgressChanges(object sender, ProgressChangedEventArgs e)
        {

        }

        public void WorkerSync_ChangeProgress(int iPercentage)
        {
            workerSync.ReportProgress(iPercentage);
        }

        public bool WorkerSync_IsCancelPending()
        {
            return workerSync.CancellationPending;
        }

        public void WorkerSync_Cancel()
        {
            workerSync.CancelAsync();
        }

        private void WorkerCourses_DoWork(object sender, DoWorkEventArgs e)
        {
            //get courses


            //print courses

        }

        private void WorkerCourses_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void WorkerCourses_ProgressChanges(object sender, ProgressChangedEventArgs e)
        {

        }

        public void WorkerCourses_ChangeProgress(int iPercentage)
        {
            workerCourses.ReportProgress(iPercentage);
        }

        public bool WorkerCourses_IsCancelPending()
        {
            return workerCourses.CancellationPending;
        }
    }
}
