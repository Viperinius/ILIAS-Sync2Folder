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
using IliasDL;

namespace WPF_ILIAS_Sync2Folder
{
    /// <summary>
    /// Interaktionslogik für StyleChanger.xaml
    /// </summary>
    public partial class StyleChanger : MetroWindow
    {
        private CConfig config;

        public StyleChanger(CConfig _config)
        {
            InitializeComponent();

            config = _config;
        }

        private void AccentSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Accent selectedAccent = AccentSelector.SelectedItem as Accent;

            if (selectedAccent != null)
            {
                ChangeWindowAccent(selectedAccent);
                config.SetWindowAccent(selectedAccent.Name);
            }
        }

        private void ChangeWindowThemeClick(object sender, RoutedEventArgs e)
        {
            string sTheme = "Base" + ((Button)sender).Content;

            ChangeWindowTheme(sTheme);

            if (sTheme == "BaseLight")
            {
                config.SetWindowTheme(true);
            }
            else if (sTheme == "BaseDark")
            {
                config.SetWindowTheme(false);
            }
        }

        public void ChangeWindowAccent(Accent accent)
        {
            //get current app style
            Tuple<AppTheme, Accent> style = ThemeManager.DetectAppStyle(Application.Current);
            //set specific accent
            ThemeManager.ChangeAppStyle(Application.Current, accent, style.Item1);
        }

        public void ChangeWindowTheme(string sTheme)
        {
            //get current app style
            Tuple<AppTheme, Accent> style = ThemeManager.DetectAppStyle(Application.Current);
            //set specific theme
            ThemeManager.ChangeAppStyle(Application.Current, style.Item2, ThemeManager.GetAppTheme(sTheme));
        }
    }
}
