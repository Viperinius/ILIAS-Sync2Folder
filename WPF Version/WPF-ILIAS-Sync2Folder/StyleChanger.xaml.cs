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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro;

namespace WPF_ILIAS_Sync2Folder
{
    /// <summary>
    /// Interaktionslogik für StyleChanger.xaml
    /// </summary>
    public partial class StyleChanger : MetroWindow
    {
        public StyleChanger()
        {
            InitializeComponent();
        }

        private void AccentSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Accent selectedAccent = AccentSelector.SelectedItem as Accent;

            if (selectedAccent != null)
            {
                Tuple<AppTheme, Accent> style = ThemeManager.DetectAppStyle(Application.Current);
                ThemeManager.ChangeAppStyle(Application.Current, selectedAccent, style.Item1);
            }
        }

        private void BtnDarkTheme_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ChangeWindowThemeClick(object sender, RoutedEventArgs e)
        {
            Tuple<AppTheme, Accent> style = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, style.Item2, ThemeManager.GetAppTheme("Base" + ((Button)sender).Content));
        }
    }
}
