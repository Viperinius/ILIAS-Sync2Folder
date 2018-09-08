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
    /// Interaktionslogik für CoursePage.xaml
    /// </summary>
    public partial class CoursePage : UserControl
    {
        List<CourseInfo> lCourseInfos = new List<CourseInfo>();
        CConfig config = new CConfig();

        public CoursePage()
        {
            InitializeComponent();

            listviewCourse.ItemsSource = lCourseInfos;

            lCourseInfos.Add(new CourseInfo() { CourseChecked = true, CourseName = "ET-BS2", CourseOwnName = "BS2", CourseId = "28374" });
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

        }

        private void BtnEditCourse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSaveCourse_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class CourseInfo
    {
        public bool CourseChecked { get; set; }

        public string CourseName { get; set; }

        public string CourseOwnName { get; set; }

        public string CourseId { get; set; }
    }
}
