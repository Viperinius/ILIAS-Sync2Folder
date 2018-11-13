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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using IliasDL;
using MahApps.Metro.Controls.Dialogs;

namespace WPF_ILIAS_Sync2Folder
{
    /// <summary>
    /// Interaktionslogik für SyncPage.xaml
    /// </summary>
    public partial class SyncPage : UserControl
    {
        ChangedPropertyNotifier changedPropertyNotifier;
        CConfig config = new CConfig();
        CIliasHandling iliasHandling;
        MainWindow window;

        ObservableCollection<FileInfo> lFiles = new ObservableCollection<FileInfo>();

        public SyncPage(MainWindow mainWindow, CIliasHandling mainIliasHandling, ChangedPropertyNotifier changedPropNotifier)
        {
            InitializeComponent();

            window = mainWindow;
            iliasHandling = mainIliasHandling;
            changedPropertyNotifier = changedPropNotifier;

            stackSyncButton.DataContext = changedPropertyNotifier;
            iconSync.DataContext = changedPropertyNotifier;
            progBarFile.DataContext = changedPropertyNotifier;
            progBarCourses.DataContext = changedPropertyNotifier;
            lbProgFile.DataContext = changedPropertyNotifier;
            lbProgCourses.DataContext = changedPropertyNotifier;
            lbNewFilesCount.DataContext = changedPropertyNotifier;

            listViewSync.ItemsSource = iliasHandling.lFiles;
            listViewSync.DataContext = changedPropertyNotifier;

        }

        private void ResizeGridViewColumns()
        {
            foreach (var column in gridViewSync.Columns)
            {
                if (double.IsNaN(column.Width))
                {
                    column.Width = column.ActualWidth;
                }
                column.Width = double.NaN;
            }
        }

        private void BtnSync_Click(object sender, RoutedEventArgs e)
        {
            changedPropertyNotifier.ChangeBtnSyncText();
            if (iliasHandling.bSyncRunning)
            {
                changedPropertyNotifier.BtnSyncIconSpin = false;
                window.WorkerSync_Cancel();
            }
            else
            {
                changedPropertyNotifier.BtnSyncIconSpin = true;
                iliasHandling.lFiles.Clear();
                window.WorkerSync_RunAsync();
            }

        }

        private void SyncPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {            
            ResizeGridViewColumns();
        }

        private void CheckShowOnly_Checked(object sender, RoutedEventArgs e)
        {
            config.SetShowOnly(true);
        }

        private void CheckShowOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            config.SetShowOnly(false);
        }

        private void CheckShowOnlyNew_Checked(object sender, RoutedEventArgs e)
        {
            config.SetShowOnlyNew(true);
        }

        private void CheckShowOnlyNew_Unchecked(object sender, RoutedEventArgs e)
        {
            config.SetShowOnlyNew(false);
        }

        private void CheckOverwriteAll_Checked(object sender, RoutedEventArgs e)
        {
            config.SetOverwriteAll(true);
            if (checkOverwriteNone.IsChecked == true)
            {
                checkOverwriteNone.IsChecked = false;
                config.SetOverwriteNone(false);
            }
            foreach (FileInfo fileInfo in iliasHandling.lFiles)
            {
                fileInfo.FileIgnore = "Not ignored";
                config.ClearFileIgnore(fileInfo.FileId);
            }
        }

        private void CheckOverwriteNone_Checked(object sender, RoutedEventArgs e)
        {
            config.SetOverwriteNone(true);
            if (checkOverwriteAll.IsChecked == true)
            {
                checkOverwriteAll.IsChecked = false;
                config.SetOverwriteAll(false);
            }
            foreach (FileInfo fileInfo in iliasHandling.lFiles)
            {
                fileInfo.FileIgnore = "Ignored";
                config.SetFileIgnore(fileInfo.FileId);
            }
        }

        private void CheckOverwriteAll_Unchecked(object sender, RoutedEventArgs e)
        {
            config.SetOverwriteAll(false);
        }

        private void CheckOverwriteNone_Unchecked(object sender, RoutedEventArgs e)
        {
            config.SetOverwriteNone(false);
        }

        private async void SyncContextOverwrite_Click(object sender, RoutedEventArgs e)
        {
            if (listViewSync.SelectedIndex > -1)
            {
                FileInfo fileInfo = (FileInfo) listViewSync.SelectedItem;
                if (fileInfo.FileStatus == "Update available")
                {
                    var result = await window.ShowMetroMessageBox("Warning", "Do you really want to overwrite this file? This ist not reversible.", MessageDialogStyle.AffirmativeAndNegative);
                    if (result == MessageDialogResult.Negative)
                    {
                        return;
                    }
                }

                //start worker for current file
                window.WorkerSyncOneFile_RunAsync(fileInfo);
            }
        }

        private void SyncContextIgnore_Click(object sender, RoutedEventArgs e)
        {
            if (listViewSync.SelectedIndex > -1)
            {
                FileInfo fileInfo = (FileInfo)listViewSync.SelectedItem;
                if (config.GetFileIgnore(fileInfo.FileId) == fileInfo.FileId)
                {
                    config.ClearFileIgnore(fileInfo.FileId);                    
                    fileInfo.FileIgnore = "Not ignored";
                }
                else
                {
                    config.SetFileIgnore(fileInfo.FileId);
                    fileInfo.FileIgnore = "Ignored";
                }
            }
        }

        private void ListViewSync_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (listViewSync.SelectedIndex > -1)
            {
                FileInfo fileInfo = (FileInfo)listViewSync.SelectedItem;
                if (fileInfo.FileIgnore == "Ignored")
                {
                    changedPropertyNotifier.SyncContextIgnoreHeader = "Remove ignore rule";
                }
                else
                {
                    changedPropertyNotifier.SyncContextIgnoreHeader = "Ignore updates for this file";
                }

                if (fileInfo.FileStatus.StartsWith("Update available"))
                {
                    changedPropertyNotifier.SyncContextShowOverwrite = true;
                }
                else
                {
                    changedPropertyNotifier.SyncContextShowOverwrite = false;
                }
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listViewSync.SelectedIndex > -1)
            {
                FileInfo fileInfo = (FileInfo)listViewSync.SelectedItem;
                if (File.Exists(System.IO.Path.Combine(fileInfo.FilePath, fileInfo.FileName)))
                {
                    Process.Start(System.IO.Path.Combine(fileInfo.FilePath, fileInfo.FileName));
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (config.GetShowOnly() == "true")
            {
                checkShowOnly.IsChecked = true;
            }

            if (config.GetShowOnlyNew() == "true")
            {
                checkShowOnlyNew.IsChecked = true;
            }

            if (config.GetOverwriteAll() == "true")
            {
                checkOverwriteAll.IsChecked = true;
            }

            if (config.GetOverwriteNone() == "true")
            {
                checkOverwriteNone.IsChecked = true;
            }
        }

        
    }

    
}
