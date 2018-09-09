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
using IliasDL;

namespace WPF_ILIAS_Sync2Folder
{
    /// <summary>
    /// Interaktionslogik für CoursePage.xaml
    /// </summary>
    public partial class CoursePage : UserControl
    {
        ObservableCollection<CourseInfo> lCourseInfos = new ObservableCollection<CourseInfo>();
        CConfig config = new CConfig();

        public CoursePage()
        {
            InitializeComponent();

            listviewCourse.ItemsSource = lCourseInfos;
            //listviewCourse.DataContext = changedPropertyNotifier;

            lCourseInfos.Add(new CourseInfo() { CourseChecked = true, CourseName = "ET-BS2", CourseOwnName = "BS2", CourseId = "28374" });
            lCourseInfos.Add(new CourseInfo() { CourseChecked = false, CourseName = "ET", CourseOwnName = "Bla", CourseId = "28323" });
        }

        private void ResizeGridViewColumns()
        {
            foreach (var column in gridViewCourse.Columns)
            {
                if (double.IsNaN(column.Width))
                {
                    column.Width = column.ActualWidth;
                }
                column.Width = double.NaN;
            }
        }

        private void CoursePage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeGridViewColumns();
        }

        private void ToggleSyncAll_Checked(object sender, RoutedEventArgs e)
        {
            config.SetSyncAll(true);
            foreach (CourseInfo course in lCourseInfos)
            {
                course.CourseChecked = true;
            }
        }

        private void ToggleSyncAll_Unchecked(object sender, RoutedEventArgs e)
        {
            config.SetSyncAll(false);
        }

        private void ToggleUseOwnNames_Checked(object sender, RoutedEventArgs e)
        {
            config.SetUseOwnNames(true);
        }

        private void ToggleUseOwnNames_Unchecked(object sender, RoutedEventArgs e)
        {
            config.SetUseOwnNames(false);
        }

        private void BtnDeSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (lCourseInfos.Count > 0)
            {
                if (lCourseInfos.First().CourseChecked)
                {
                    foreach (CourseInfo course in lCourseInfos)
                    {
                        course.CourseChecked = false;
                    }
                }
                else
                {
                    foreach (CourseInfo course in lCourseInfos)
                    {
                        course.CourseChecked = true;
                    }
                }
            }
        }

        private void BtnEditCourse_Click(object sender, RoutedEventArgs e)
        {
            if (listviewCourse.SelectedItems.Count > 0)
            {
                CourseInfo item = (CourseInfo)listviewCourse.SelectedItems[0];
                txtCourse.Text = item.CourseName;
                txtOwnName.Text = item.CourseOwnName;
            }
            else
            {
                txtCourse.Text = "Please select a course first.";
            }
        }

        private void BtnSaveCourse_Click(object sender, RoutedEventArgs e)
        {
            if (txtCourse.Text != "Please select a course first." && txtCourse.Text != "")
            {
                CourseInfo item = lCourseInfos.SingleOrDefault(x => x.CourseName == txtCourse.Text);
                item.CourseOwnName = txtOwnName.Text;
                config.SetCourseName(item.CourseId, item.CourseOwnName);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (CourseInfo course in lCourseInfos)
            {
                if (course.CourseChecked)
                {
                    config.SetCourse(course.CourseId);
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (CourseInfo course in lCourseInfos)
            {
                if (!course.CourseChecked)
                {
                    config.ClearCoursesSettings(course.CourseId);
                }
            }
        }

        private void CoursePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (config.GetSyncAll() == "true")
            {
                toggleSyncAll.IsChecked = true;
            }
            else
            {
                toggleSyncAll.IsChecked = false;
            }

            if (config.GetUseOwnNames() == "true")
            {
                toggleUseOwnNames.IsChecked = true;
            }
            else
            {
                toggleUseOwnNames.IsChecked = false;
            }
        }
    }

    
}
