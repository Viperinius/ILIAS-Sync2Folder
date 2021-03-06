﻿using System;
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
using System.Collections.ObjectModel;
using System.Windows.Shell;
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
        private System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        private System.Windows.Forms.ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
        private ChangedPropertyNotifier changedPropertyNotifier = new ChangedPropertyNotifier();
        private CConfig config = new CConfig();
        private CUpdate updater;
        private CIliasHandling iliasHandling;

        private StyleChanger styleChanger;

        private BackgroundWorker workerLogin;
        private BackgroundWorker workerSync;
        private BackgroundWorker workerCourses;
        private BackgroundWorker workerSyncOverwrite;

        private string sUsername;
        private string sPassword;
        private bool bLoginSuccess;

        public MainWindow()
        {
            
            InitializeComponent();

            BtnLogin.Tag = PackIconOcticonsKind.SignIn;

            textblockLogin.DataContext = changedPropertyNotifier;
            rectLoginIcon.DataContext = changedPropertyNotifier;

            iliasHandling = new CIliasHandling(this, changedPropertyNotifier);

            tabSync.Content = new SyncPage(this, iliasHandling, changedPropertyNotifier);
            tabCourseConfig.Content = new CoursePage(iliasHandling, changedPropertyNotifier);
            tabFolderConfig.Content = new FolderPage();
            tabGeneralConfig.Content = new GeneralPage();
            tabInfo.Content = new HelpPage();

            styleChanger = new StyleChanger(config);
            styleChanger.Hide();

            notifyIcon.Icon = Properties.Resources.dliconWHITEsquare;
            notifyIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(NotifyIcon_MouseDown);
            notifyIcon.DoubleClick += delegate (object sender, EventArgs e)
                                    {
                                        this.Show();
                                        WindowState = WindowState.Normal;
                                    };
            notifyIcon.BalloonTipClicked += new EventHandler(NotifyIcon_BalloonTipClicked);
            updater = new CUpdate(notifyIcon, contextMenu);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized && config.GetShowTrayIcon() == "true")
            {
                this.Hide();
            }
            base.OnStateChanged(e);
        }

        private async void NotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            //if update notification popped up
            if (notifyIcon.BalloonTipTitle == "Update available")
            {
                var metroWindow = (Application.Current.MainWindow as MetroWindow);
                var result = await metroWindow.ShowMessageAsync("Update available", "An update has been found. Would you like to download it?", MessageDialogStyle.AffirmativeAndNegative);

                if (result == MessageDialogResult.Affirmative)
                {
                    //call updater programme
                    if (!updater.GetUpdate())
                    {
                        await metroWindow.ShowMessageAsync("Error", "An error occured while updating...", MessageDialogStyle.Affirmative);
                        return;
                    }
                }
            }
        }

        private void NotifyIcon_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ContextMenu menu = (ContextMenu)this.FindResource("NotifyContextMenu");
                menu.IsOpen = true;
            }
        }

        private void ContextOpen_Click(object sender, RoutedEventArgs e)
        {
            if (this.Visibility == Visibility.Hidden)
            {
                this.Show();
                WindowState = WindowState.Normal;
            }
            Activate();
        }

        private void ContextLogin_Click(object sender, RoutedEventArgs e)
        {
            if (this.Visibility == Visibility.Hidden)
            {
                this.Show();
                WindowState = WindowState.Normal;
                Activate();
            }
            if (!iliasHandling.bLoggedIn)
            {
                ShowLoginDialog();
            }
            else
            {
                ShowLogoutDialog();
            }
        }

        private void ContextExit_Click(object sender, RoutedEventArgs e)
        {
            if (workerSync.IsBusy) { workerSync.CancelAsync(); }
            if (workerCourses.IsBusy) { workerCourses.CancelAsync(); }
            if (workerLogin.IsBusy) { workerLogin.CancelAsync(); }

            Application.Current.Shutdown();
            
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!iliasHandling.bLoggedIn)
            {
                ShowLoginDialog();               
            }
            else
            {
                ShowLogoutDialog();
            }
        }

        private void BtnStyle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                styleChanger.Show();
            }
            catch (InvalidOperationException)
            {
                styleChanger = new StyleChanger(config);
                styleChanger.Show();
            }
        }

        /// <summary>
        /// Open the login dialog
        /// </summary>
        private async void ShowLoginDialog()
        {
            var metroWindow = (Application.Current.MainWindow as MetroWindow);
            LoginDialogData result;
            if (config.GetUser() != "__NO_VAL__")
            {
                result = await metroWindow.ShowLoginAsync("ILIAS Login", "Please log in with your ILIAS credentials", new LoginDialogSettings { InitialUsername = config.GetUser() });
            }
            else
            {
                result = await metroWindow.ShowLoginAsync("ILIAS Login", "Please log in with your ILIAS credentials", new LoginDialogSettings { InitialUsername = "" });
            }
            //Console.WriteLine(result.Username + " " + result.Password);

            if (result != null)
            {
                sUsername = result.Username;
                sPassword = result.Password;

                workerLogin.RunWorkerAsync();
            }            
        }

        /// <summary>
        /// Open a dialog for logout
        /// </summary>
        private async void ShowLogoutDialog()
        {
            var metroWindow = (Application.Current.MainWindow as MetroWindow);
            var result = await metroWindow.ShowMessageAsync("Logout", "Do you want to log out?", MessageDialogStyle.AffirmativeAndNegative);

            if (result == MessageDialogResult.Affirmative)
            {
                bool bSuccess = iliasHandling.IliasLogout();

                if (!bSuccess)
                {
                    await metroWindow.ShowMessageAsync("Error", "An error occured while logging out...", MessageDialogStyle.Affirmative);
                }
                else
                {
                    changedPropertyNotifier.ChangeBtnLoginText();
                    BtnLogin.Tag = PackIconOcticonsKind.SignIn;
                    progressRing.Visibility = Visibility.Visible;
                    iconCheck.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem == tabCourseConfig)
            {
                if (!iliasHandling.lCourseInfos.Any())
                {
                    if (!workerCourses.IsBusy)
                    {
                        workerCourses.RunWorkerAsync();
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Wow, you were not supposed to click this...", "Woah", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            workerLogin = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            workerLogin.DoWork += new DoWorkEventHandler(WorkerLogin_DoWork);
            workerLogin.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerLogin_RunWorkerCompleted);
            workerLogin.ProgressChanged += new ProgressChangedEventHandler(WorkerLogin_ProgressChanged);

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

            workerSyncOverwrite = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = false
            };

            workerSyncOverwrite.DoWork += new DoWorkEventHandler(WorkerSyncOverwrite_DoWork);
            workerSyncOverwrite.ProgressChanged += new ProgressChangedEventHandler(WorkerSyncOverwrite_ProgressChanges);
            workerSyncOverwrite.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerSyncOverwrite_RunWorkerCompleted);

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

            //-----------------------------
            //  check for any set theme
            //-----------------------------

            if (config.GetWindowTheme() != "__NO_VAL__")
            {
                styleChanger.ChangeWindowTheme(config.GetWindowTheme());
            }
            if (config.GetWindowAccent() != "__NO_VAL__")
            {                
                styleChanger.ChangeWindowAccent(ThemeManager.GetAccent(config.GetWindowAccent()));                
            }
        }

        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            styleChanger.Close();
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
        }

        public async Task<MessageDialogResult> ShowMetroMessageBox(string sTitle, string sMessage, MessageDialogStyle dialogStyle)
        {
            return await this.ShowMessageAsync(sTitle, sMessage, dialogStyle);
        }

        //-----------------------------
        //      worker functions
        //-----------------------------

        private void WorkerLogin_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!iliasHandling.bLoggedIn)
            {
                bLoginSuccess = iliasHandling.IliasLogin(sUsername, sPassword);
            }
            else
            {
                bLoginSuccess = true;
            }

            workerLogin.ReportProgress(0);
        }

        private void WorkerLogin_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressRing.Visibility = Visibility.Collapsed;
            iconCheck.Visibility = Visibility.Visible;
        }

        private void WorkerLogin_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!bLoginSuccess)
            {
                this.ShowMessageAsync("Error", "An error occured while logging in...", MessageDialogStyle.Affirmative);
                progressRing.Visibility = Visibility.Visible;
                iconCheck.Visibility = Visibility.Collapsed;
            }
            else
            {
                changedPropertyNotifier.ChangeBtnLoginText();
                BtnLogin.Tag = PackIconOcticonsKind.SignOut;
            }
        }

        private void WorkerSyncOverwrite_DoWork(object sender, DoWorkEventArgs e)
        {
            iliasHandling.bSyncRunning = true;
            changedPropertyNotifier.BtnSyncIconSpin = true;

            if (!iliasHandling.bLoggedIn)
            {
                return;
            }

            //get arguments
            List<object> tempList = e.Argument as List<object>; //file info first, then mode selector

            if (tempList[1].ToString() == "single")
            {
                //get new file from ilias
                iliasHandling.GetSingleFile((FileInfo)tempList[0]);
            }
            else if (tempList[1].ToString() == "multiple")
            {
                //get new file from ilias
                foreach (FileInfo fileInfo in iliasHandling.lFiles)
                {
                    if (fileInfo.FileStatus == "Update available!")
                    {
                        iliasHandling.GetSingleFile(fileInfo);
                    }
                }
            }
        }

        private void WorkerSyncOverwrite_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                taskbarItemInfo.ProgressState = TaskbarItemProgressState.Paused;
            }
            else if (e.Error != null)
            {
                taskbarItemInfo.ProgressState = TaskbarItemProgressState.Error;
            }
            else
            {
                taskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
            }

            iliasHandling.bSyncRunning = false;
            changedPropertyNotifier.BtnSyncIconSpin = false;
        }

        private void WorkerSyncOverwrite_ProgressChanges(object sender, ProgressChangedEventArgs e)
        {
            /*
            if (e.ProgressPercentage == 501)
            {
                FileInfo currentCollectionFile = iliasHandling.lFiles[Int32.Parse(e.UserState.ToString())];
                FileInfo currentListFile = iliasHandling.listFiles[Int32.Parse(e.UserState.ToString())];

                currentCollectionFile.FileStatus = currentListFile.FileStatus;
                currentCollectionFile.FileIsVisible = currentListFile.FileIsVisible;
            }*/

            taskbarItemInfo.ProgressValue = 1;
        }

        public void WorkerSyncOverwrite_RunAsync(FileInfo file)
        {
            List<object> lArguments = new List<object>
            {
                file,
                "single"
            };

            workerSyncOverwrite.RunWorkerAsync(lArguments);
            taskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
        }

        /// <summary>
        /// Run the overwrite sync worker
        /// </summary>
        /// <param name="file">File info, set to null if multiple files should be overwritten</param>
        /// <param name="sMode">Mode selector ("single" or "multiple")</param>
        public void WorkerSyncOverwrite_RunAsync(FileInfo file, string sMode)
        {
            List<object> lArguments = new List<object>
            {
                file,
                sMode
            };

            workerSyncOverwrite.RunWorkerAsync(lArguments);
            taskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
        }

        public void WorkerSyncOverwrite_ChangeProgress(int iPercentage, object userState)
        {
            workerSyncOverwrite.ReportProgress(iPercentage, userState);            
        }

        private void WorkerSync_DoWork(object sender, DoWorkEventArgs e)
        {
            iliasHandling.bSyncRunning = true;

            if (!iliasHandling.bLoggedIn)
            {
                workerSync.CancelAsync();
                return;
            }

            //clear any old data
            iliasHandling.listFiles.Clear();
            //reset progress
            workerSync.ReportProgress(0, 0);
            iliasHandling.iFilePercentage = 0;
            iliasHandling.iCoursePercentage = 0;
            iliasHandling.iCurrentCourseNum = 0;
            changedPropertyNotifier.NewFilesCount = 0;

            if (!iliasHandling.listCourseInfos.Any())
            {
                iliasHandling.GetCourses();
                iliasHandling.GetCourseNames();
            }

            if (!workerSync.CancellationPending && !iliasHandling.bCoursesDone)
            {
                workerSync.ReportProgress(iliasHandling.iFilePercentage, iliasHandling.iCoursePercentage);

                iliasHandling.iCurrentCourseNum = 1;
                if (config.GetSyncAll() == "true")
                {
                    foreach (CourseInfo course in iliasHandling.listCourseInfos)
                    {
                        iliasHandling.GetCourseFiles(Int32.Parse(course.CourseId));
                        if (workerSync.CancellationPending)
                        {
                            return;
                        }
                        iliasHandling.iCurrentCourseNum++;                        
                    }
                }
                else if (config.GetSyncAll() == "false")
                {
                    foreach (CourseInfo course in iliasHandling.listCourseInfos)
                    {
                        if (config.GetCourse(course.CourseId) == course.CourseId)
                        {
                            iliasHandling.GetCourseFiles(Int32.Parse(course.CourseId));
                            if (workerSync.CancellationPending)
                            {
                                return;
                            }
                            iliasHandling.iCurrentCourseNum++;
                        }
                    }
                }

                iliasHandling.bCoursesDone = true;
                if (!workerSync.CancellationPending)
                {
                    iliasHandling.iCoursePercentage = 100;
                }
                workerSync.ReportProgress(iliasHandling.iFilePercentage, iliasHandling.iCoursePercentage);
            }



        }

        private void WorkerSync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                taskbarItemInfo.ProgressState = TaskbarItemProgressState.Paused;
            }
            else if (e.Error != null)
            {
                taskbarItemInfo.ProgressState = TaskbarItemProgressState.Error;
            }
            else
            {
                taskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
            }

            iliasHandling.bCoursesDone = false;
            if (!iliasHandling.lCourseInfos.Any())
            {
                if (!workerCourses.IsBusy)
                {
                    workerCourses.RunWorkerAsync();
                }
                /*
                foreach (CourseInfo course in iliasHandling.listCourseInfos)
                {
                    iliasHandling.lCourseInfos.Add(course);
                }
                */
            }

            iliasHandling.bSyncRunning = false;
            changedPropertyNotifier.BtnSyncIconSpin = false;
            changedPropertyNotifier.BtnSyncText = "Start synchronisation";

            if (config.GetSyncNotify() == "true")
            {
                updater.DisplayNotification("Synchronisation finished", "The synchronisation of the selected courses has finished!");
            }
        }

        private void WorkerSync_ProgressChanges(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage > 100 && e.UserState is string)
            {
                if (e.ProgressPercentage == 500)
                {
                    iliasHandling.lFiles.Add(iliasHandling.listFiles[Int32.Parse(e.UserState.ToString())]);
                }
                else if (e.ProgressPercentage == 501)
                {
                    /*
                    FileInfo currentCollectionFile = iliasHandling.lFiles[Int32.Parse(e.UserState.ToString())];
                    FileInfo currentListFile = iliasHandling.listFiles[Int32.Parse(e.UserState.ToString())];

                    currentCollectionFile.FileStatus = currentListFile.FileStatus;
                    currentCollectionFile.FileIsVisible = currentListFile.FileIsVisible;
                    */
                }
            }
            else
            {
                changedPropertyNotifier.SyncProgBarFileVal = e.ProgressPercentage;
                changedPropertyNotifier.SyncLbProgFile = e.ProgressPercentage.ToString() + " %";
                if (e.UserState is int)
                {
                    changedPropertyNotifier.SyncProgBarCourseVal = (int)e.UserState;
                    changedPropertyNotifier.SyncLbProgCourses = e.UserState.ToString() + " %";
                    taskbarItemInfo.ProgressValue = Convert.ToDouble(e.UserState) / 100;
                }
            }
        }

        public void WorkerSync_RunAsync()
        {
            workerSync.RunWorkerAsync();
            taskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
        }

        public void WorkerSync_ChangeProgress(int iPercentage)
        {
            workerSync.ReportProgress(iPercentage);
        }

        public void WorkerSync_ChangeProgress(int iPercentage, object userState)
        {
            workerSync.ReportProgress(iPercentage, userState);
        }

        public bool WorkerSync_IsCancelPending()
        {
            return workerSync.CancellationPending;
        }

        public void WorkerSync_Cancel()
        {
            workerSync.CancelAsync();
            iliasHandling.bSyncRunning = false;
        }

        private void WorkerCourses_DoWork(object sender, DoWorkEventArgs e)
        {
            if (iliasHandling.bLoggedIn)
            {
                //get courses
                iliasHandling.GetCourses();
                workerCourses.ReportProgress(33);
                iliasHandling.GetCourseNames();
                workerCourses.ReportProgress(66);
                string sTemp = "";
                foreach (CourseInfo course in iliasHandling.listCourseInfos)
                {
                    if ((config.GetCourse(course.CourseId) == course.CourseId) || config.GetSyncAll() == "true")
                    {
                        course.CourseChecked = true;
                    }

                    sTemp = config.GetCourseName(course.CourseId);
                    if (sTemp != "__NO_VAL__")
                    {
                        course.CourseOwnName = sTemp;
                    }
                }
                workerCourses.ReportProgress(100);
            }
        }

        private void WorkerCourses_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void WorkerCourses_ProgressChanges(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage > 99)
            {
                foreach (CourseInfo course in iliasHandling.listCourseInfos)
                {
                    iliasHandling.lCourseInfos.Add(course);
                }

                changedPropertyNotifier.CourseProgBarVal = 100;
            }
            else
            {
                changedPropertyNotifier.CourseProgBarVal = e.ProgressPercentage;
            }
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
