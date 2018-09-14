﻿using System;
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
using IliasDL;

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

        private bool bSyncStatus;

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

            listViewSync.ItemsSource = iliasHandling.lFiles;

            //iliasHandling.lFiles.Add(new FileInfo() { FileStatus = "Done", FileName = "Test.pdf", FilePath = @"Test\Test2\", FileDate = "19.08.2018", FileSize = "12 KB", FileId = "12345" });
            //iliasHandling.lFiles.Add(new FileInfo() { FileStatus = "Done", FileName = "First.pdf", FilePath = @"Test\OMG\", FileDate = "19.08.2018", FileSize = "12 KB", FileId = "882184", FileIsVisible = false });
            //iliasHandling.lFiles.Add(new FileInfo() { FileStatus = "Syncing", FileName = "Ohreally.pdf", FilePath = @"Test\Test2\", FileDate = "01.01.9999", FileSize = "420 MB", FileId = "9875" });
            
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
            if (bSyncStatus)
            {
                iconSync.Spin = false;
                bSyncStatus = false;
                window.WorkerSync_Cancel();
            }
            else
            {
                iconSync.Spin = true;
                bSyncStatus = true;
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
        }
    }

    
}
